package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.sql.Timestamp;
import java.util.Map;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class UsageHistoryResponse {
    @JsonProperty("id")
    private UUID id;

    @JsonProperty("user_id")
    private UUID userId;

    @JsonProperty("start_time")
    private Long startTime;

    @JsonProperty("end_time")
    private Long endTime;

    @JsonProperty("strain_name")
    private String strainName;

    @JsonProperty("strain_id")
    private Integer strainId;

    @JsonProperty("medical_rank")
    private Double medicalRank;

    @JsonProperty("positive_rank")
    private Double positiveRank;

    @JsonProperty("overall_rank")
    private Integer overallRank;

    @JsonProperty("heartbeat_high")
    private Integer heartbeatHigh;

    @JsonProperty("heartbeat_low")
    private Integer heartbeatLow;

    @JsonProperty("heartbeat_avg")
    private Integer heartbeatAvg;

    @JsonProperty("questions_answers_dictionary")
    private String questionsAnswersDictionary;

}
