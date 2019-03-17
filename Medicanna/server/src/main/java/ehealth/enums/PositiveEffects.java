package ehealth.enums;
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


import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.Map;


public enum PositiveEffects implements BaseEnumEffect {
    AROUSED(0),
    GIGGLY(1),
    FOCUSED(2),
    SLEEPY(3),
    TINGLY(4),
    UPLIFTED(5),
    TALKATIVE(6),
    ENERGETIC(7),
    CREATIVE(8),
    HAPPY(9),
    EUPHORIC(10),
    HUNGRY(11),
    RELAXED(12);


    private static Map<String, PositiveEffects> stringToValue = new HashMap<>();
    private static Map<Integer, PositiveEffects> intToValue = new HashMap<>();

    public final int value;

    PositiveEffects(int value) {

        this.value = value;
    }

    public int getValue() {
        return value;
    }

    static {
        for (PositiveEffects effectType : PositiveEffects.values()) {
            stringToValue.put(effectType.toString(), effectType);
            intToValue.put(new Integer(effectType.value), effectType);

        }
    }

    public static PositiveEffects valueOf(int effectValue) {
        return intToValue.get(effectValue);

    }

    @JsonValue
    @Override
    public String getEffect() {
        return valueOf(this.toString()).name();
    }
}
