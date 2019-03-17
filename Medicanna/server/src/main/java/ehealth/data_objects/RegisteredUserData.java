package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;

import java.util.UUID;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class RegisteredUserData {
    @JsonProperty("user_id")
    UUID userId;

    @JsonProperty("username")
    String username;

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

    @JsonProperty("created_at")
    Long createdAt;


}
