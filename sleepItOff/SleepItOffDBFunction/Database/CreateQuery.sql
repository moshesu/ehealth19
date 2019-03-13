/* INSERT INTO dbo.ManageUsers
	(userId, userName, password)
	VALUES
	('123456789', 'test', '123321'),
	('omerTest', 'test2', 'pass');
	*/


/*
drop table ManageUsers;
CREATE TABLE ManageUsers (
	userId VARCHAR(50) PRIMARY KEY,
	userName VARCHAR(50) NULL,
	password VARCHAR(50) NULL
);
*/

/*option 2*/
/*
drop table ManageUsers;
CREATE TABLE ManageUsers (
	userId VARCHAR(50) PRIMARY KEY,
	lastUpdated DATETIME
);
*/


/*
INSERT INTO dbo.UserDetails
	(userId, FirstName, LastName, Gender,Age,Height,Weight)
	VALUES
	('123456789', 'omer', 'g', 'Male' , '1993', '180', '65');
*/

/*
drop table UserDetails;
CREATE TABLE UserDetails (	
	userId VARCHAR(50) PRIMARY KEY,
	firstName VARCHAR(50) NULL,
	lastName VARCHAR(50) NULL,
	gender VARCHAR(6) NULL,
	age INT,
	height INT,
	weight INT
);
*/



/*  
	sleepType 1 = combat nap
	sleepType 2 = smart alarm
	sleepType 3 = creative sleep
*/

/*
drop table AwakeningAccuracy;
CREATE TABLE AwakeningAccuracy (
	userId VARCHAR(50) PRIMARY KEY,
	sleepType INT,
	definedAlarmTime DATETIME,
	actualAlarmTime DATETIME,
	timeDifference TIME
);
*/


/*
drop table SleepQuality;
CREATE TABLE SleepQuality (
	userId VARCHAR(50) PRIMARY KEY,
	averageWakeUps INT,
	averageSleepEfficiency INT
);
*/



/*
drop table SleepSegmentsStats;
CREATE TABLE SleepSegmentsStats (
	userId VARCHAR(50) PRIMARY KEY,
	lastUpdated DATETIME,

	awakeCountTimes INT,
	awakeTotalDuration INT,
	awakeToAwakeCount INT,
	awakeToSnoozeCount INT,
	awakeToDozeCount INT,
	awakeToRestlessSleepCount INT,
	awakeToRestfulSleepCount INT,
	awakeToREMCount INT,

	snoozeCountTimes INT,
	snoozeTotalDuration INT,
	snoozeToAwakeCount INT,
	snoozeToSnoozeCount INT,
	snoozeToDozeCount INT,
	snoozeToRestlessSleepCount INT,
	snoozeToRestfulSleepCount INT,
	snoozeToREMCount INT,

	dozeCountTimes INT,
	dozeTotalDuration INT,
	dozeToAwakeCount INT,
	dozeToSnoozeCount INT,
	dozeToDozeCount INT,
	dozeToRestlessSleepCount INT,
	dozeToRestfulSleepCount INT,
	dozeToREMCount INT,

	restlessSleepCountTimes INT,
	restlessSleepTotalDuration INT,
	restlessSleepToAwakeCount INT,
	restlessSleepToSnoozeCount INT,
	restlessSleepToDozeCount INT,
	restlessSleepToRestlessSleepCount INT,
	restlessSleepToRestfulSleepCount INT,
	restlessSleepToREMCount INT,

	restfulSleepCountTimes INT,
	restfulSleepTotalDuration INT,
	restfulSleepToAwakeCount INT,
	restfulSleepToSnoozeCount INT,
	restfulSleepToDozeCount INT,
	restfulSleepToRestlessSleepCount INT,
	restfulSleepToRestfulSleepCount INT,
	restfulSleepToREMCount INT,

	REMSleepCountTimes INT,
	REMSleepTotalDuration INT,
	REMSleepToAwakeCount INT,
	REMSleepToSnoozeCount INT,
	REMSleepToDozeCount INT,
	REMSleepToRestlessSleepCount INT,
	REMSleepToRestfulSleepCount INT,
	REMSleepToREMCount INT
	);

*/
