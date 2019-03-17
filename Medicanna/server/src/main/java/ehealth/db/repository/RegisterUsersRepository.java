package ehealth.db.repository;
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

import ehealth.db.model.RegisteredUsersEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface RegisterUsersRepository extends JpaRepository<RegisteredUsersEntity, UUID> {

    public RegisteredUsersEntity findByUsername(String username);

    public RegisteredUsersEntity findById(UUID userId);

}
