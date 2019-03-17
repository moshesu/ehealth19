package ehealth.enums;

import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.Map;

public enum MedicalEffects implements BaseEnumEffect {
    SEIZURES(0),
    MUSCLE_SPASMS(1),
    SPASTICITY(2),
    INFLAMMATION(3),
    EYE_PRESSURE(4),
    HEADACHES(5),
    FATIGUE(6),
    NAUSEA(7),
    LACK_OF_APPETITE(8),
    CRAMPS(9),
    STRESS(10),
    PAIN(11),
    DEPRESSION(12),
    INSOMNIA(13),
    HEADACHE(14);

    private static Map<String, MedicalEffects> stringToValue = new HashMap<>();
    private static Map<Integer, MedicalEffects> intToValue = new HashMap<>();

    public final int value;

    MedicalEffects(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    static {
        for (MedicalEffects effectType : MedicalEffects.values()) {
            stringToValue.put(effectType.toString(), effectType);
            intToValue.put(new Integer(effectType.value), effectType);
        }
    }

    public static MedicalEffects valueOf(int effectValue) {
        return intToValue.get(effectValue);

    }

    @JsonValue
    @Override
    public String getEffect() {
        return valueOf(this.toString()).name();
    }

}