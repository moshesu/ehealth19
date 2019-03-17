package ehealth.app;
/*
*****************************************************************************
*   The confidential and proprietary information contained in this file may
*   only be used by a person authorised under and to the extent permitted
*   by a subsisting licensing agreement from ARM Limited or its affiliates.
*
*          (C) COPYRIGHT 2013-2018 ARM Limited or its affiliates.
*              ALL RIGHTS RESERVED
*
*   This entire notice must be reproduced on all copies of this file
*   and copies of this file may only be made by a person if such person is
*   permitted to do so under the terms of a subsisting license agreement
*   from ARM Limited or its affiliates.
*****************************************************************************/

import org.apache.catalina.valves.AccessLogValve;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty;
import org.springframework.boot.context.embedded.ConfigurableEmbeddedServletContainer;
import org.springframework.boot.context.embedded.EmbeddedServletContainerCustomizer;
import org.springframework.boot.context.embedded.tomcat.TomcatEmbeddedServletContainerFactory;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.EnableAspectJAutoProxy;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurerAdapter;

import java.io.CharArrayWriter;

@Configuration
@EnableAutoConfiguration
@EnableAspectJAutoProxy
@EnableWebMvc
@ComponentScan
@EnableAsync
public class ApplicationConfig extends WebMvcConfigurerAdapter implements EmbeddedServletContainerCustomizer {

    @Value("#{environment.ACCESS_LOG_ENABLED ?: false}")
    private boolean accessLogEnabled;

    @ConditionalOnProperty("accessLogEnabled")
    @Override
    public void customize(ConfigurableEmbeddedServletContainer configurableEmbeddedServletContainer) {
        if (accessLogEnabled == true) {
            if (configurableEmbeddedServletContainer instanceof TomcatEmbeddedServletContainerFactory) {

                final TomcatEmbeddedServletContainerFactory tcContainer = (TomcatEmbeddedServletContainerFactory) configurableEmbeddedServletContainer;
                AccessLogValve accessLogValve = new AccessLogValve() {
                    @Override
                    public void log(CharArrayWriter message) {
                        System.out.println(message.toCharArray());
                    }
                };
                accessLogValve.setPattern("%{yyyy-MM-dd HH:mm:ss.SSS}t [access log] ***REQUEST:%r ***RESPONSE status code:%s ***Time to process request in mils:%D ");
                tcContainer.addContextValves(accessLogValve);
            }
        }
    }

}
