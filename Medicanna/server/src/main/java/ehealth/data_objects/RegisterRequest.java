package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class RegisterRequest {

    @JsonProperty("username")
    String username;

    @JsonProperty("password")
    String password;

    @JsonProperty("dob")
    String DOB;

    @JsonProperty("gender")
    String gender;

    @JsonProperty("country")
    String country;

    @JsonProperty("city")
    String city;

    @JsonProperty("email")
    String email;

    @JsonProperty("medical")
    int medical;

    @JsonProperty("positive")
    int positive;

    @JsonProperty("negative")
    int negative;

}
