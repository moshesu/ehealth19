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
public class BaseResponse {
    @JsonProperty("request_id")
    UUID requestId;

    @JsonProperty("status")
    String status;

    @JsonProperty("body")
    String body;

    public BaseResponse(String status) {
        this.status = status;
    }
}
