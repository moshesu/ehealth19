package ehealth.api;

import ehealth.client.data_objects.StrainObject;
import ehealth.client.data_objects.SuggestedStrains;
import ehealth.data_objects.*;
import ehealth.exceptions.BadRegisterRequestException;
import ehealth.exceptions.BadRequestException;
import ehealth.service.StrainApiServiceImpl;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import static ehealth.messages.MessagesConstants.BAD_LOGIN_INPUT;

@Controller
@RestController
public class BaseController {
    // Strain collect at start - not as beanW

    private StrainApiServiceImpl mainServiceImpl;
    private Logger logger = LoggerFactory.getLogger(BaseController.class.getName());

    @Autowired
    public BaseController(StrainApiServiceImpl mainServiceImpl) {
        this.mainServiceImpl = mainServiceImpl;
    }

    /**
     * Register API
     *
     * @return RegisteredUserData
     */
    @RequestMapping(value = "/register", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<RegisteredUserData> register(@RequestBody RegisterRequest registerRequest) {
        if (registerRequest == null) {
            throw new BadRegisterRequestException();
        }
        logger.info("POST register request: " + registerRequest.toString());
        return new ResponseEntity<>(mainServiceImpl.register(registerRequest), HttpStatus.CREATED);
    }

    /**
     * Edit profile API
     *
     * @return RegisteredUserData
     */
    @RequestMapping(value = "/edit/{user-id}", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<RegisteredUserData> edit(@PathVariable("user-id") String userId, @RequestBody RegisterRequest updateRequest) {
        if (updateRequest == null) {
            throw new BadRegisterRequestException();
        }
        logger.info("POST edit register data request: " + updateRequest.toString());
        return new ResponseEntity<>(mainServiceImpl.edit(userId, updateRequest), HttpStatus.OK);
    }

    /**
     * Login API
     *
     * @return RegisteredUserData
     */
    @RequestMapping(value = "/login", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<RegisteredUserData> login(@RequestBody LoginRequest loginRequest) {
        if (loginRequest == null) {
            throw new BadRequestException(BAD_LOGIN_INPUT);
        }
        logger.info("POST login request: " + loginRequest.toString());
        return new ResponseEntity<>(mainServiceImpl.authenticate(loginRequest), HttpStatus.OK);
    }

    /**
     * GET Strain by name API from strains database
     *
     * @return String
     */
    @RequestMapping(value = "/strain/name/{strain-name}", method = RequestMethod.GET)
    public ResponseEntity<StrainObject> getStrainInfoByName(@PathVariable("strain-name") String strainName) {
        logger.info("GET strain information by name: " + strainName);
        return new ResponseEntity<>(mainServiceImpl.getStrainByName(strainName), HttpStatus.OK);
    }


    /**
     * GET Strain by strain Id API from strains database
     *
     * @return String
     */
    @RequestMapping(value = "/strain/id/{strain-id}", method = RequestMethod.GET)
    public ResponseEntity<StrainObject> getStrainInfoById(@PathVariable("strain-id") Integer strainId) {
        logger.info("GET strain information by id: " + strainId);
        return new ResponseEntity<>(mainServiceImpl.getStrainById(strainId), HttpStatus.OK);

    }

    /**
     * GET Strain by strain effects API from strains database
     *
     * @return String
     */
    @RequestMapping(value = "/strain/effects", method = RequestMethod.GET)
    public ResponseEntity<SuggestedStrains> getStrainInfoByEffects(@RequestParam int medical, @RequestParam int positive) {
        logger.info("GET String by effects");
        return new ResponseEntity<>(mainServiceImpl.getStrainByEffects(medical, positive),HttpStatus.OK);
    }

    /**
     * Get recommended API
     *
     * @return SuggestedStrains(List < Strain >, status)
     */
    @RequestMapping(value = "/strains/recommended/{user-id}", method = RequestMethod.GET, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<SuggestedStrains> getRecommended(@PathVariable("user-id") String userId) {
        logger.info("GET recommended strains for userId: " + userId);
        // get User Info by ID from database
        return new ResponseEntity<>(mainServiceImpl.getRecommendedStrain(userId),HttpStatus.OK);
    }

    /**
     * POST save user usage history API
     *
     * @return BaseResponse
     */
    @RequestMapping(value = "/usage", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<BaseResponse> saveUserUsageHistory(@RequestBody UsageHistory usageHistory) {
        logger.info("POST Usage history for user-id: " + usageHistory.getUserId());
        logger.info("Usage History Request: " + usageHistory.toString());
        return new ResponseEntity<>(mainServiceImpl.saveUsageHistoryForUser(usageHistory),HttpStatus.CREATED);
    }

    /**
     * GET user usages history API
     *
     * @return List<UsageHistoryResponse>
     */
    @RequestMapping(value = "/usage/{user-id}", method = RequestMethod.GET, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public List<UsageHistoryResponse> getUserUsageHistory(@PathVariable("user-id") String userId) {
        logger.info("GET Usage history for user-id: " + userId);
        return mainServiceImpl.getUsageHistoryForUser(userId);
    }

    /**
     * GET all strains
     *
     * @return Map<strain_name                                                               ,                                                                                                                               strain_id>
     */
    @RequestMapping(value = "/strains/all", method = RequestMethod.GET, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public Map<String, Integer> saveUserUsageHistory() throws IOException {
        logger.info("GET List of all strains");
        return mainServiceImpl.GetListOfStrains();
    }

    /**
     * GET Export usage history report to email
     *
     * @return List<UsageHistoryResponse>
     */
    @RequestMapping(value = "/usage/export/{user-id}", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<BaseResponse> exportUsage(@PathVariable("user-id") String userId, @RequestBody EmailRequest emailRequest) throws IOException {
        logger.info("GET export usage report for user: " + userId);
        return new ResponseEntity<>(mainServiceImpl.exportToEmail(userId, emailRequest.getTo(), emailRequest.getContent()),HttpStatus.OK);
    }
}

