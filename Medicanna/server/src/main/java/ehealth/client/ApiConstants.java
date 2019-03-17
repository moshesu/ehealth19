package ehealth.client;


public class ApiConstants {

    public static final String API_KEY = "/M076LdW";
    public static final String URL = "http://strainapi.evanbusse.com" + API_KEY;


    //List all Effects
    public static final String ALL_EFECTS_URI = "/searchdata/effects";
    //List all Strains
    public static final String ALL_STRAINS_URI = "/strains/search/all";
    //Strain By Name
    public static final String STRAIN_BY_NAME_URI = "/strains/search/name/{strain-name}";
    //Strain By Race
    public static final String STRAIN_BY_RACE_URI = "/strains/search/race/{race-name}";
    //Strain By effect
    public static final String STRAIN_BY_EFFECT = "strains/search/effect/{effect}";

    //Strains-APIS - by strain ID
    //Strain description By ID

    public static final String GET_STRAIN_DESC_URI = "/strains/data/desc/{strain-id}";
    //Strain description By ID
    public static final String GET_STRAIN_EFFECT_URI = "strains/data/effects/{strain-id}";


}
