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

import com.fasterxml.jackson.databind.ObjectMapper;
import ehealth.enums.BaseEnumEffect;
import ehealth.enums.PositiveEffects;

import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;
import java.util.UUID;

public class StrainListToBitSet {
    ObjectMapper objectMapper = new ObjectMapper();

    public static long convertListToIntBitSet(List<BaseEnumEffect> effects) {
        BitSet bitset = new BitSet();
        for (BaseEnumEffect effect : effects) {
            bitset.set(effect.getValue());
        }
        return bitset.toLongArray()[0];
    }

    public static void main(String[] args){
        List<BaseEnumEffect> effects = new ArrayList<>();
        effects.add(PositiveEffects.SLEEPY);
        effects.add(PositiveEffects.RELAXED);
        System.out.println(convertListToIntBitSet(effects));
        String uuid = "844ead73-93a7-4547-ab4c-48eaf16b6fe5";
        UUID a = UUID.fromString(uuid);
    }
}
