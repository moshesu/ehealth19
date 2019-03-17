package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;



@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class StrainObject {
    @JsonProperty("name")
    String name;

    @JsonProperty("id")
    Integer id;

    @JsonProperty("race")
    String race;

    @JsonProperty("description")
    String description;

    @JsonProperty("positive")
    Long positive = new Long(0);

    @JsonProperty("negative")
    Long negative = new Long(0);

    @JsonProperty("medical")
    Long medical = new Long(0);

    @JsonProperty("rank")
    double rank;

    @JsonProperty("number_of_usages")
    int numberOfUsages;
}
