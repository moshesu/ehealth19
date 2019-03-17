package ehealth.db.converters;
/*
*****************************************************************************
*   The confidential and proprietary information contained in this file may
*   only be used by a person authorised under and to the extent permitted
*   by a subsisting licensing agreement from ARM Limited or its affiliates.
*
*          (C) COPYRIGHT 2013-2016 ARM Limited or its affiliates.
*              ALL RIGHTS RESERVED
*
*   This entire notice must be reproduced on all copies of this file
*   and copies of this file may only be made by a person if such person is
*   permitted to do so under the terms of a subsisting license agreement
*   from ARM Limited or its affiliates.
*****************************************************************************/

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import javax.persistence.AttributeConverter;
import javax.ws.rs.InternalServerErrorException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class ListToJsonStringConverter implements AttributeConverter<List<Integer>, String> {

    private ObjectMapper objectMapper = new ObjectMapper();

    @Override
    public String convertToDatabaseColumn(List<Integer> attributes) {
        String jsonString = null;
        if (attributes != null) {
            try {
                jsonString = objectMapper.writeValueAsString(attributes);
            } catch (JsonProcessingException e) {
                throw new InternalServerErrorException("Can not convert attributes to json", e);
            }
        }
        return jsonString;
    }

    @Override
    public List<Integer> convertToEntityAttribute(String dbData) {
        List<Integer> attributes = new ArrayList<>();
        if (dbData != null) {
            try {
                attributes = objectMapper.readValue(dbData, new TypeReference<List<Integer>>(){});
            } catch (IOException e) {
                throw new InternalServerErrorException("Can not convert json to map", e);
            }
        }
        return attributes;
    }
}
