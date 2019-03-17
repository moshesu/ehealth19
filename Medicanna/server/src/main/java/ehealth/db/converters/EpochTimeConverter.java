package ehealth.db.converters;
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

import javax.persistence.AttributeConverter;
import java.sql.Timestamp;

public class EpochTimeConverter implements AttributeConverter<Long, Timestamp> {

    @Override
    public Timestamp convertToDatabaseColumn(Long attribute) {
        return attribute == null? null: new Timestamp(attribute);
    }

    @Override
    public Long convertToEntityAttribute(Timestamp dbData) {
        return dbData == null? null: dbData.getTime();
    }
}
