package ehealth.client;

import ehealth.data_objects.StrainDescription;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import java.util.List;

import static ehealth.client.ApiConstants.ALL_EFECTS_URI;
import static ehealth.client.ApiConstants.ALL_STRAINS_URI;

@Path("/")
public interface StrainServicesInterface {

    @GET
    @Path("/strains/data/desc/{strain-id}")
    @Produces({MediaType.APPLICATION_JSON})
    StrainDescription strainDescById(@PathParam(value = "strain-id") String strainName);

    @GET
    @Path(ALL_STRAINS_URI)
    @Produces({MediaType.APPLICATION_JSON})
    String getAllStrains();
}
