package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import ehealth.client.data_objects.StrainObject;
import lombok.*;

import java.util.List;



@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class RecommendedStrainList {

    @JsonProperty("count")
    int count;

    @JsonProperty("body")
    List<StrainObject> body;
}
