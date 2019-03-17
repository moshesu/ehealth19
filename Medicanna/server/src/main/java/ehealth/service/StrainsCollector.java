package ehealth.service;

import com.google.gson.GsonBuilder;
import com.google.gson.internal.LinkedTreeMap;
import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.StrainObject;
import ehealth.data_objects.StrainDescription;
import ehealth.db.model.StrainsEntity;
import ehealth.db.repository.AllStrainsRepository;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import org.jboss.resteasy.client.jaxrs.ResteasyClient;
import org.jboss.resteasy.client.jaxrs.ResteasyClientBuilder;
import org.jboss.resteasy.client.jaxrs.ResteasyWebTarget;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.ws.rs.core.UriBuilder;
import java.io.IOException;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;
import java.util.UUID;

import static ehealth.client.ApiConstants.URL;

@Service
public class StrainsCollector {

    protected ResteasyClient client;
    protected ResteasyWebTarget target;
    protected StrainServicesInterface restClient;

    private Logger logger = LoggerFactory.getLogger(StrainsCollector.class);
    private GsonBuilder builder = new GsonBuilder();

    protected AllStrainsRepository allStrainsRepository;

    @Autowired
    public StrainsCollector(AllStrainsRepository allStrainsRepository) throws IOException {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
        this.allStrainsRepository = allStrainsRepository;
        getAllStrains();
    }


    public void getAllStrains() throws IOException {
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();
        if (strainsEntities == null || strainsEntities.size()==0) {
            Object o = builder.create().fromJson(restClient.getAllStrains(), Object.class);
            // Get strains information
            List<StrainObject> allStrains = getStrainsObjectList(o);
            // Save strains in DB
            saveStrainsInDb(allStrains);
        }
    }


    public List<StrainObject> getStrainsObjectList(Object o) {
        List<StrainObject> allStainsList = new ArrayList<>();
        List<String> keySet = new ArrayList<String>();
        keySet.addAll(((LinkedTreeMap) o).keySet());
        for (int i = 0; i < keySet.size(); i++) {
            StrainObject strainObject = new StrainObject();
            Object entry = ((LinkedTreeMap) o).get(keySet.get(i));
            strainObject.setName((keySet.get(i)));
            strainObject.setId(Float.valueOf(((LinkedTreeMap) entry).get("id").toString()).intValue());
            strainObject.setRace(((LinkedTreeMap) entry).get("race").toString());
            Object effects = ((LinkedTreeMap) entry).get("effects");
            BitSet bitset = new BitSet();
            for (String medical : (List<String>) ((LinkedTreeMap) effects).get("medical"))
                bitset.set((MedicalEffects.valueOf(medical.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setMedical(bitset.toLongArray()[0]);
            bitset.clear();
            for (String negative : (List<String>) ((LinkedTreeMap) effects).get("negative"))
                bitset.set((NegativeEffects.valueOf(negative.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setNegative(bitset.toLongArray()[0]);
            bitset.clear();
            for (String positive : (List<String>) ((LinkedTreeMap) effects).get("positive"))
                bitset.set((PositiveEffects.valueOf(positive.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setPositive(bitset.toLongArray()[0]);
            allStainsList.add(strainObject);
        }
        // Get strains description
        for (StrainObject strainObject : allStainsList) {
            StrainDescription description = restClient.strainDescById(strainObject.getId().toString());
            strainObject.setDescription(description.getDesc());
        }
        return allStainsList;
    }

//       Update\Save strains in DB
    private void saveStrainsInDb(List<StrainObject> allStainsList) {
        for (StrainObject strainObject : allStainsList) {
            StrainsEntity strainsEntity = allStrainsRepository.findByStrainId(strainObject.getId());
            if (strainsEntity != null) {
                continue;
            }
            strainsEntity = new StrainsEntity();
            strainsEntity.setId(UUID.randomUUID());
            strainsEntity.setStrainName(strainObject.getName());
            strainsEntity.setStrainId(strainObject.getId());
            strainsEntity.setRace(strainObject.getRace());
            strainsEntity.setDescription(strainObject.getDescription());
            strainsEntity.setMedical(strainObject.getMedical().intValue());
            strainsEntity.setPositive(strainObject.getPositive().intValue());
            strainsEntity.setNegative(strainObject.getNegative().intValue());
            // Init ranking
            strainsEntity.setNumberOfUsages(0);
            strainsEntity.setRank(0);
            allStrainsRepository.save(strainsEntity);
        }
    }

}