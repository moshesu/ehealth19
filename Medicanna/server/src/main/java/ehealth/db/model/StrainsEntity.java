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

import lombok.Getter;
import lombok.Setter;
import org.hibernate.annotations.Type;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import java.util.UUID;

@Entity
@Table(name = "all_strains")
@Setter
@Getter
public class StrainsEntity {

    @Id
    @Column(name = "id", updatable = false)
    @Type(type = "org.hibernate.type.PostgresUUIDType")
    private UUID id;

    @Column(name = "strain_name", updatable = false)
    private String strainName;

    @Column(name = "race")
    private String race;

    @Column(name = "strain_id")
    private Integer strainId;

    @Column(name = "medical")
    private int medical;

    @Column(name = "positive")
    private int positive;

    @Column(name = "negative")
    private int negative;

    @Column(name = "rank")
    private double rank;

    @Column(name = "number_of_usages")
    private Integer numberOfUsages;

    @Column(name = "description")
    private String description;

}


