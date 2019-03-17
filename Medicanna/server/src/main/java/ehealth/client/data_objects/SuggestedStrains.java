package ehealth.client.data_objects;

import lombok.*;

import java.util.ArrayList;
import java.util.List;

@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class SuggestedStrains {
    Integer status = 0;
    List<StrainObject> suggestedStrains = new ArrayList();

    public void addStrain(StrainObject strainObject){
        this.suggestedStrains.add(strainObject);
    }
}
