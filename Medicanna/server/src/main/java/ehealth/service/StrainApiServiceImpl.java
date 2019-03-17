package ehealth.service;

import com.google.gson.Gson;
import com.hubspot.jinjava.Jinjava;
import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.StrainObject;
import ehealth.client.data_objects.SuggestedStrains;
import ehealth.data_objects.*;
import ehealth.db.model.RegisteredUsersEntity;
import ehealth.db.model.StrainsEntity;
import ehealth.db.model.UsageHistoryEntity;
import ehealth.db.repository.AllStrainsRepository;
import ehealth.db.repository.RegisterUsersRepository;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import ehealth.exceptions.BadRegisterRequestException;
import ehealth.exceptions.BadRequestException;
import ehealth.service.api.StrainApiService;
import org.jboss.resteasy.client.jaxrs.ResteasyClient;
import org.jboss.resteasy.client.jaxrs.ResteasyClientBuilder;
import org.jboss.resteasy.client.jaxrs.ResteasyWebTarget;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.ws.rs.InternalServerErrorException;
import javax.ws.rs.core.UriBuilder;
import java.io.*;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.sql.Timestamp;
import java.text.DecimalFormat;
import java.util.*;

import static ehealth.client.ApiConstants.URL;
import static ehealth.messages.MessagesConstants.*;


@Service
public class StrainApiServiceImpl implements StrainApiService {

    protected ResteasyClient client;
    protected ResteasyWebTarget target;
    protected StrainServicesInterface restClient;
    private Logger logger = LoggerFactory.getLogger(StrainApiServiceImpl.class);

    @Autowired
    protected RegisterUsersRepository registerUsersRepository;

    @Autowired
    protected AllStrainsRepository allStrainsRepository;

    @Autowired
    StrainsCollector strainsCollector;

    @Autowired
    protected EmailService emailService;


    public StrainApiServiceImpl() {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
    }

    @Override
    public RegisteredUserData authenticate(LoginRequest loginRequest) {
        String user = loginRequest.getUsername().toLowerCase();
        String password = loginRequest.getPassword();
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findByUsername(loginRequest.getUsername());
        if (registeredUsersEntity == null) {
            logger.error("Username: " + user + " does not exist in DB");
            throw new BadRequestException(String.format(USER_NOT_FOUND, user));
        }
        if (!registeredUsersEntity.getPassword().equals(password)) {
            logger.error("Bad password for user: " + user);
            throw new BadRequestException(BAD_PASSWORD);
        }
        logger.info("User: " + user + " Authenticated successfully");
        // return registered user data
        return createUserDataResponseFromEntity(registeredUsersEntity);
    }

    @Override
    public RegisteredUserData register(RegisterRequest registerRequest) {
        if(registerUsersRepository.findByUsername(registerRequest.getUsername()) != null){
            throw new BadRequestException(String.format(DUPLICATE_USERNAME,registerRequest.getUsername()));
        }
        if (registerRequest != null) {
            logger.info("Register request: " + registerRequest.toString());
            RegisteredUsersEntity registeredUsersEntity = new RegisteredUsersEntity();
            // Generate Unique User Id
            registeredUsersEntity.setId(UUID.randomUUID());
            // Set createdAt as current time
            registeredUsersEntity.setCreatedAt(System.currentTimeMillis());
            // Set input user-data
            registeredUsersEntity.setUsername(registerRequest.getUsername().toLowerCase());
            registeredUsersEntity.setPassword(registerRequest.getPassword());
            registeredUsersEntity.setCity(registerRequest.getCity());
            registeredUsersEntity.setEmail(registerRequest.getEmail());
            registeredUsersEntity.setCountry(registerRequest.getCountry());
            registeredUsersEntity.setDob(registerRequest.getDOB());
            registeredUsersEntity.setGender(registerRequest.getGender());
            registeredUsersEntity.setMedical(registerRequest.getMedical());
            registeredUsersEntity.setPositive(registerRequest.getPositive());
            registeredUsersEntity.setNegative(registerRequest.getNegative());
            // Save to DB
            registerUsersRepository.save(registeredUsersEntity);
            return createUserDataResponseFromEntity(registeredUsersEntity);
        }
        throw new BadRegisterRequestException();
    }

    @Override
    public RegisteredUserData edit(String userId, RegisterRequest registerRequest) {
        if (registerRequest != null) {

            logger.info("Edit request: " + registerRequest.toString());
            RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));            // Generate Unique User Id
            // Set input user-data
            if (registerRequest.getMedical() != registeredUsersEntity.getMedical()) {
                registeredUsersEntity.setMedical(registerRequest.getMedical());
            }
            if (registerRequest.getPositive() != registeredUsersEntity.getPositive()) {
                registeredUsersEntity.setPositive(registerRequest.getPositive());
            }
            if (registerRequest.getNegative() != registeredUsersEntity.getNegative()) {
                registeredUsersEntity.setNegative(registerRequest.getNegative());
            }
            // Save to DB
            registerUsersRepository.save(registeredUsersEntity);
            return createUserDataResponseFromEntity(registeredUsersEntity);
        }
        throw new BadRegisterRequestException();
    }

    @Override
    public StrainObject getStrainByName(String strainName) {
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainName(strainName);
        if (strainsEntity == null) {
            throw new BadRequestException(String.format(STRAIN_NOT_FOUND,strainName));
        }
        return strainEntityToStrainObject(strainsEntity);
    }

    @Override
    public StrainObject getStrainById(Integer strainId) {
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainId(strainId);
        if (strainsEntity == null) {
            throw new BadRequestException(String.format(STRAIN_NOT_FOUND,strainId.toString()));
        }
        return strainEntityToStrainObject(strainsEntity);
    }

    @Override
    public List<StrainObject> getAllStrains() {
        List<StrainObject> allStrains = new ArrayList<>();
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();
        for (StrainsEntity strain : strainsEntities) {
            allStrains.add(strainEntityToStrainObject(strain));
        }
        return allStrains;
    }

    @Override
    public Map<String, Integer> GetListOfStrains() {
        Map<String, Integer> listOfStrains = new HashMap<>();
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();
        for (StrainsEntity strain : strainsEntities) {
            listOfStrains.put(strain.getStrainName(), strain.getStrainId());
        }
        return listOfStrains;
    }

    @Override
    public SuggestedStrains getRecommendedStrain(String userId) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));
        int medical = registeredUsersEntity.getMedical();
        int positive = registeredUsersEntity.getPositive();
        SuggestedStrains suggestedStrains = getStrainByEffects(medical, positive);
        List<StrainObject> listOfSuggestedStrains = suggestedStrains.getSuggestedStrains();

        // Filter blacklisted strains
        int numberOfSuggested = listOfSuggestedStrains.size();
        for (int i = 0; i < numberOfSuggested; i++)
            if (registeredUsersEntity.getBlacklist().contains(listOfSuggestedStrains.get(i).getId())) {
                listOfSuggestedStrains.remove(i);
            }
        return suggestedStrains;
    }

    @Override
    public SuggestedStrains getStrainByEffects(int medical, int positive) {
        SuggestedStrains recommendedStrains = new SuggestedStrains();

        // Check for strains that fit exact medical and positive effects
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();

        for (StrainsEntity strain : strainsEntities) {
            int medicalCand = strain.getMedical();
            int positiveCand = strain.getPositive();
            if ((medicalCand & medical) == medical && (positiveCand & positive) == positive) {
                recommendedStrains.addStrain(strainEntityToStrainObject(strain));
            }
        }
        // Check for strains that fit exact medical effects
        // Check for strains that similar to positive by number of effects
        if (recommendedStrains.getSuggestedStrains().size() == 0) {
            logger.info("Did not fine any strain that fits preferences");
            for (int i = 1; i < CountBits(positive); i++) {
                logger.info("Search for similar positive strains that differ in : " + i + " number of effects");
                for (StrainsEntity strain : strainsEntities) {
                    int medicalCand = strain.getMedical();
                    int positiveCand = strain.getPositive();
                    if ((medicalCand & medical) == medical &&
                            (CountBits(positiveCand ^ positive)) == i) {
                        recommendedStrains.addStrain(strainEntityToStrainObject(strain));
                    }
                }
                if (recommendedStrains.getSuggestedStrains().size() > 0) {
                    recommendedStrains.setStatus(1);
                    break;
                }
            }
        }
        // Check for strains that fit only medical effects - similar
        if (recommendedStrains.getSuggestedStrains().size() == 0) {
            logger.info("Did not fine any strain that fits preferences with similar positive");
            for (int i = 0; i < CountBits(medical); i++) {
                logger.info("Search for similar medical strains that differ in: " + i + " number of effects");
                for (StrainsEntity strain : strainsEntities) {
                    int medicalCand = strain.getMedical();
                    if ((CountBits(medicalCand ^ medical)) == i) {
                        recommendedStrains.addStrain(strainEntityToStrainObject(strain));
                    }
                }
                if (recommendedStrains.getSuggestedStrains().size() > 0) {
                    recommendedStrains.setStatus(2);
                    break;
                }
            }
        }
        return recommendedStrains;

    }

    @Override
    public BaseResponse saveUsageHistoryForUser(UsageHistory usageHistory) {
        // Fetch register user entity from DB if exist
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(usageHistory.getUserId()));
        if (registeredUsersEntity == null) {
            logger.error(String.format(USER_NOT_FOUND, usageHistory.getUserId()));
            throw new BadRequestException(String.format(USER_NOT_FOUND, usageHistory.getUserId()));
        }
        // Convert Usage History request to database entity and add to list
        UsageHistoryEntity usageHistoryEntity = buildUsageHistoryEntity(usageHistory);
        List<UsageHistoryEntity> usageHistoryEntityList;
        if (registeredUsersEntity.getUsageHistoryEntity() == null) {
            usageHistoryEntityList = Arrays.asList(usageHistoryEntity);
        } else {
            usageHistoryEntityList = registeredUsersEntity.getUsageHistoryEntity();
            usageHistoryEntityList.add(usageHistoryEntity);
        }
        registeredUsersEntity.setUsageHistoryEntity(usageHistoryEntityList);
        // Update blackList
        if (usageHistory.getIsBlacklist() == 1) {
            List<Integer> blacklist = registeredUsersEntity.getBlacklist();
            blacklist.add(usageHistoryEntity.getStrainId());
            registeredUsersEntity.setBlacklist(blacklist);
            logger.info("String Id: " + usageHistoryEntity.getStrainId().toString() + " added to blacklist");
        }

        registerUsersRepository.save(registeredUsersEntity);

        // Update strain rank in strains db table
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainId(usageHistory.getStrainId());
        Integer numOfUsages = strainsEntity.getNumberOfUsages();
        if (numOfUsages == null) {
            numOfUsages = 0;
        }
        double rank = strainsEntity.getRank();
        strainsEntity.setRank(prepareRankValue((numOfUsages * rank + usageHistory.getOverallRank()) / (numOfUsages + 1)));
        strainsEntity.setNumberOfUsages(numOfUsages + 1);
        allStrainsRepository.save(strainsEntity);

        return new BaseResponse(
                UUID.randomUUID(),
                "201",
                usageHistoryEntity.getId().toString());
    }

    @Override
    public List<UsageHistoryResponse> getUsageHistoryForUser(String userId) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));
        List<UsageHistoryResponse> usageHistoryResponseList = new ArrayList<>();
        if (registeredUsersEntity.getUsageHistoryEntity() == null ||
                registeredUsersEntity.getUsageHistoryEntity().size() == 0) {
            return usageHistoryResponseList;
        }
        for (UsageHistoryEntity usageHistoryEntity : registeredUsersEntity.getUsageHistoryEntity()) {
            usageHistoryResponseList.add(new UsageHistoryResponse(
                    usageHistoryEntity.getId(),
                    usageHistoryEntity.getUserId(),
                    usageHistoryEntity.getStartedAt(),
                    usageHistoryEntity.getEndedAt(),
                    usageHistoryEntity.getStrainName(),
                    usageHistoryEntity.getStrainId(),
                    usageHistoryEntity.getMedicalRank(),
                    usageHistoryEntity.getPositiveRank(),
                    usageHistoryEntity.getOverallRank(),
                    usageHistoryEntity.getHeartbeatHigh(),
                    usageHistoryEntity.getHeartbeatLow(),
                    usageHistoryEntity.getHeartbeatAvg(),
                    usageHistoryEntity.getQuestionsAnswersDictionary()));
        }
        return usageHistoryResponseList;
    }

    @Override
    public BaseResponse exportToEmail(String userId, String to, String userContent) throws IOException {
        BaseResponse resp = new BaseResponse();
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));            // Generate Unique User Id
        if (registeredUsersEntity == null) {
            logger.error(String.format(USER_NOT_FOUND,userId));
            throw new BadRequestException(String.format(USER_NOT_FOUND, userId));
        }
        List<UsageHistoryResponse> usageHistoryEntityList = getUsageHistoryForUser(userId);
        String subject = "Usage History for:  " + registeredUsersEntity.getUsername();
        StringBuilder usageHistoryContent = new StringBuilder();
        for (UsageHistoryResponse usageHistoryResponse : usageHistoryEntityList) {
            usageHistoryContent.append(printUsageAsHtml(usageHistoryResponse));
        }
        String emailContent = renderContent(registeredUsersEntity, to, usageHistoryContent.toString(), userContent);
        int emailResp = emailService.sendEmail(
                registeredUsersEntity.getUsername(),
                registeredUsersEntity.getEmail(),
                to, subject,
                emailContent);
        resp.setBody("Usage history exported to: " + to + " successfully");
        resp.setStatus(String.valueOf(emailResp));
        return new BaseResponse(UUID.randomUUID(),
                String.valueOf(emailResp),
                String.format(EMAIL_MESSAGE,to));
    }

    private UsageHistoryEntity buildUsageHistoryEntity(UsageHistory usageHistory) {
        UsageHistoryEntity usageHistoryEntity = new UsageHistoryEntity();
        usageHistoryEntity.setId(UUID.randomUUID());
        usageHistoryEntity.setUserId(UUID.fromString(usageHistory.getUserId()));
        usageHistoryEntity.setStartedAt(usageHistory.getStartTime());
        usageHistoryEntity.setEndedAt(usageHistory.getEndTime());
        usageHistoryEntity.setStrainId(usageHistory.getStrainId());
        usageHistoryEntity.setStrainName(usageHistory.getStrainName());
        usageHistoryEntity.setHeartbeatHigh(usageHistory.getHeartbeatHigh());
        usageHistoryEntity.setHeartbeatAvg(usageHistory.getHeartbeatAvg());
        usageHistoryEntity.setHeartbeatLow(usageHistory.getHeartbeatLow());
        usageHistoryEntity.setMedicalRank(usageHistory.getMedicalRank());
        usageHistoryEntity.setPositiveRank(usageHistory.getPositiveRank());
        usageHistoryEntity.setOverallRank(usageHistory.getOverallRank());
        usageHistoryEntity.setQuestionsAnswersDictionary(usageHistory.getQuestionsAnswersDictionary());
        return usageHistoryEntity;
    }

    private RegisteredUserData createUserDataResponseFromEntity(RegisteredUsersEntity registeredUsersEntity) {
        return new RegisteredUserData(
                registeredUsersEntity.getId(),
                registeredUsersEntity.getUsername(),
                registeredUsersEntity.getDob(),
                registeredUsersEntity.getGender(),
                registeredUsersEntity.getCountry(),
                registeredUsersEntity.getCity(),
                registeredUsersEntity.getEmail(),
                registeredUsersEntity.getMedical(),
                registeredUsersEntity.getPositive(),
                registeredUsersEntity.getNegative(),
                registeredUsersEntity.getCreatedAt()
        );
    }

    private StrainObject strainEntityToStrainObject(StrainsEntity strainsEntity) {
        StrainObject strainObject = new StrainObject();
        strainObject.setDescription(strainsEntity.getDescription());
        strainObject.setName(strainsEntity.getStrainName());
        strainObject.setId(strainsEntity.getStrainId());
        strainObject.setMedical(new Long(strainsEntity.getMedical()));
        strainObject.setPositive(new Long(strainsEntity.getPositive()));
        strainObject.setNegative(new Long(strainsEntity.getNegative()));
        strainObject.setRace(strainsEntity.getRace());
        strainObject.setRank(strainsEntity.getRank());
        strainObject.setNumberOfUsages((strainsEntity.getNumberOfUsages() == null) ? 0 : strainsEntity.getNumberOfUsages());
        return strainObject;
    }

    private static int CountBits(int num) {
        int tmpNum = num, count = 0;
        while (tmpNum > 0) {
            count += tmpNum & 1;
            tmpNum >>= 1;
        }
        return count;
    }

    private double prepareRankValue(double value) {
        // Format value to three decimal places
        DecimalFormat df = new DecimalFormat("#.#");
        // Return formatted value multiplied vy 100 to get percentage format value
        return Float.valueOf(df.format(Double.valueOf(value))).doubleValue();
    }

    private String printJsonAsHtml(String data) {
        String questionsHtml = "";
        Map<String, String> result = new Gson().fromJson(data, Map.class);
        for (Map.Entry<String, String> entry : result.entrySet()) {
            questionsHtml += "<li>  " + entry.getKey() + "     " + entry.getValue() + "</li>";
        }
        return questionsHtml;
    }

    private String printUsageAsHtml(UsageHistoryResponse usageHistoryResponse) {
        String usageResponseMessage = "";
        usageResponseMessage +=
                "<br> Usage History For Strain: " + usageHistoryResponse.getStrainName() + "" +
                        "<br> Started At: " + new Timestamp(usageHistoryResponse.getStartTime()).toString() +
                        "<br> Ended At: " + new Timestamp(usageHistoryResponse.getEndTime()).toString() +
                        "<li> Medical user rank: " + usageHistoryResponse.getMedicalRank() + "</li>" +
                        "<li> Positive user rank: " + usageHistoryResponse.getPositiveRank() + "</li>" +
                        "<li> Overall user rank: " + usageHistoryResponse.getOverallRank() + "</li>";
        if (usageHistoryResponse.getHeartbeatAvg() > 0) {
            usageResponseMessage +=
                    "<li> Was band use ?: " + "Yes" + "" +
                            "<li> Heartbeat highest value: " + usageHistoryResponse.getHeartbeatHigh() + "</li>" +
                            "<li> Heartbeat lowest value: " + usageHistoryResponse.getHeartbeatLow() + "</li>" +
                            "<li> Heartbeat average value: " + usageHistoryResponse.getHeartbeatAvg() + "</li>";
        } else {
            usageResponseMessage += "<li> Was band used ? " + "No" + "</li>";
        }
        usageResponseMessage += "<br>User Feedback: ";
        usageResponseMessage += printJsonAsHtml(usageHistoryResponse.getQuestionsAnswersDictionary());
        usageResponseMessage += "<br>______________________________________________________________________________";
        return usageResponseMessage;
    }

    private String printProfilerAsHtml(RegisteredUsersEntity registeredUsersEntity) {
        String usageProfile = "";
        usageProfile +=
                "<h3> {{username}} Profile: </h3> " +
                        "<li> Date of birth: " + registeredUsersEntity.getDob() +
                        "<li> Gender: " + registeredUsersEntity.getGender() +
                        "<li> Location: " + registeredUsersEntity.getCountry() + ", " + registeredUsersEntity.getCity() +
                        "<li> Medical preferences: " + getMedicalEffects(registeredUsersEntity.getMedical()) + "</li>" +
                        "<li> Positive preferences: " + getPositiveEffects(registeredUsersEntity.getPositive()) + "</li>" +
                        "<li> Negative preferences: " + getNegativeEffects(registeredUsersEntity.getNegative()) + "</li>";

        usageProfile += "<br>______________________________________________________________________________";
        return usageProfile;
    }

    private String getNegativeEffects(int negative) {
        List<String> effects = new ArrayList<>();
        int numOfEffects = NegativeEffects.values().length;
        for (int i = 0, tmpNum = 1; i < numOfEffects; i++, tmpNum <<= 1) {
            if ((tmpNum & negative) != 0) {
                effects.add(NegativeEffects.valueOf(i).getEffect().toLowerCase().replace('_', ' '));
            }
        }
        return effects.toString().replace('[', ' ').replace(']', ' ');
    }

    private String getPositiveEffects(int positive) {
        List<String> effects = new ArrayList<>();
        int numOfEffects = PositiveEffects.values().length;
        for (int i = 0, tmpNum = 1; i < numOfEffects; i++, tmpNum <<= 1) {
            if ((tmpNum & positive) != 0) {
                effects.add(PositiveEffects.valueOf(i).getEffect().toLowerCase().replace('_', ' '));
            }
        }
        return effects.toString().replace('[', ' ').replace(']', ' ');
    }

    private String getMedicalEffects(int medical) {
        List<String> effects = new ArrayList<>();
        int numOfEffects = MedicalEffects.values().length;
        for (int i = 0, tmpNum = 1; i < numOfEffects; i++, tmpNum <<= 1) {
            if ((tmpNum & medical) != 0) {
                effects.add(MedicalEffects.valueOf(i).getEffect().toLowerCase().replace('_', ' '));
            }
        }
        return effects.toString().replace('[', ' ').replace(']', ' ');
    }

    private String getHtmlTemplateFromFile(String filename) {
        byte[] encoded;
        try{
            encoded = Files.readAllBytes(Paths.get("src/main/resources/html_templates/" + filename));
        }
        catch (IOException e ){
            throw new InternalServerErrorException("Bad html template file input ");
        }
        return new String(encoded);
    }

    private String renderContent(RegisteredUsersEntity registeredUsersEntity, String toAddress, String usageHistory, String userContent) {
        Jinjava jinjava = new Jinjava();
        Map<String, Object> context = new HashMap<>();
        context.put("to", toAddress);
        context.put("userContent", userContent);
        context.put("usageData", usageHistory);
        context.put("username", registeredUsersEntity.getUsername());
        context.put("profile", printProfilerAsHtml(registeredUsersEntity));
        String emailTemplate = getHtmlTemplateFromFile("email_usage_template.html");
        return jinjava.render(emailTemplate, context);
    }

}
