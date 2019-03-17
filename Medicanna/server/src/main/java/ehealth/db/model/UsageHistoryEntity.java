package ehealth.db.model;
/*
 *****************************************************************************
 *   The confidential and proprietary information contained in this file may
 *   only be used by a person authorised under and to the extent permitted
 *   by a subsisting licensing agreement from ARM Limited or its affiliates.
 *
 *          (C) COPYRIGHT 2013-2018 ARM Limited or its affiliates.
 *              ALL RIGHTS RESERVED
 *
 *   This entire notice must be reproduced on all copies of this file
 *   and copies of this file may only be made by a person if such person is
 *   permitted to do so under the terms of a subsisting license agreement
 *   from ARM Limited or its affiliates.
 *****************************************************************************/

import ehealth.db.converters.EpochTimeConverter;
import lombok.Getter;
import lombok.Setter;
import org.hibernate.annotations.Type;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "usage_history")
@Setter
@Getter
public class UsageHistoryEntity {

    @Id
    @Column(name = "id", updatable = false)
    @Type(type = "org.hibernate.type.PostgresUUIDType")
    private UUID id;

    @Column(name = "strain_name", updatable = false)
    private String strainName;

    @Column(name = "strain_id")
    private Integer strainId;

    @Convert(converter = EpochTimeConverter.class)
    @Column(name = "start_time", updatable = false)
    private Long startedAt;

    @Convert(converter = EpochTimeConverter.class)
    @Column(name = "end_time", updatable = false)
    private Long endedAt;

    @Column(name = "medical_rank")
    private Double medicalRank;

    @Column(name = "positive_rank")
    private Double positiveRank;

    @Column(name = "overall_rank")
    private Integer overallRank;

    @Column(name = "heartbeat_high")
    private Integer heartbeatHigh;

    @Column(name = "heartbeat_low")
    private Integer heartbeatLow;

    @Column(name = "heartbeat_avg")
    private Integer heartbeatAvg;

    @Column(name = "questions_answers_dictionary")
    private String questionsAnswersDictionary;


    @Column(name = "user_id", updatable = false)
    @Type(type = "org.hibernate.type.PostgresUUIDType")
    private UUID userId;

    @ManyToOne(cascade = CascadeType.ALL)
    @JoinColumn(name = "user_id", nullable = false, insertable = false, updatable = false)
    private RegisteredUsersEntity registeredUsersEntity;

}


