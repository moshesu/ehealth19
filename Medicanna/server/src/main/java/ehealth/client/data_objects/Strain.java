package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class Strain {
    @JsonProperty("id")
    Integer id;
    @JsonProperty("name")
    String name;
    @JsonProperty("race")
    String race;
    @JsonProperty("desc")
    String desc;
}
