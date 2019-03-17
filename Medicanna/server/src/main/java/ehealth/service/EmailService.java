package ehealth.service;

import com.hubspot.jinjava.Jinjava;
import com.sendgrid.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

@Service
public class EmailService {
    private Logger logger = LoggerFactory.getLogger(EmailService.class);
    public static final String contentTypeHtml = "text/html";

    public SendGrid sg;

    public EmailService() {
        String sendgridApikey = System.getenv("SENDGRID_API_KEY");
        if (sendgridApikey != null) {
            logger.info("Sendgrid APIKEY loaded successfully");
        } else {
            logger.info("Sendgrid APIKEY is missing - Email service may not work");
        }
        this.sg = new SendGrid(sendgridApikey);
    }

    public int sendEmail(String username, String userEmailAddress, String toAddress, String subject, String emailContent) throws IOException {
        Email from = new Email("medicanna-usage-service@medicannaApp.com");
        Email to = new Email(toAddress);
        Email replyTo = new Email(userEmailAddress);
        Content content = new Content(contentTypeHtml, emailContent);
        Mail mail = new Mail(from, subject, to, content);
        mail.setReplyTo(replyTo);
        return sendEmail(mail);
    }


    public int sendEmail(Mail mail) throws IOException {
        Response response = null;
        Request request = new Request();
        try {
            request.setMethod(Method.POST);
            request.setEndpoint("mail/send");
            request.setBody(mail.build());
            response = sg.api(request);
            logger.info("Email response status code: " + response.getStatusCode());
            logger.info(response.getBody());
            logger.info(response.getHeaders().toString());
        } catch (IOException ex) {
            throw ex;
        }
        return response.getStatusCode();
    }

}
