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

public enum SessionState {
    OK(0),
    BAD(1);

    private static Map<Integer, SessionState> intStatusToValue = new HashMap<>();
    private final int value;

    SessionState(int value) {
        this.value = value;
    }

    static {
        for (SessionState sessionState : SessionState.values()) {
            intStatusToValue.put(sessionState.value, sessionState);
        }
    }

    public static SessionState valueOf(int statusValue) {
        return intStatusToValue.get(statusValue);
    }

    @JsonValue
    public String getState() {
        return valueOf(this.value).name();
    }
}
