package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.ToString;

import java.sql.Timestamp;

@Data
@AllArgsConstructor
@NoArgsConstructor
@ToString
public class UsageHistory {

    @JsonProperty("user_id")
    private String userId;

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

    @JsonProperty("is_blacklist")
    private Integer isBlacklist;

}
