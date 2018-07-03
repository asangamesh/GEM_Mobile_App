-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 192.168.88.227    Database: gem
-- ------------------------------------------------------
-- Server version	5.6.17

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `fluency_level`
--

DROP TABLE IF EXISTS `fluency_level`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fluency_level` (
  `FluencyLevelId` int(11) NOT NULL,
  `Number` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `ShortName` varchar(30) NOT NULL,
  PRIMARY KEY (`FluencyLevelId`),
  UNIQUE KEY `FluencyLevelId_UNIQUE` (`FluencyLevelId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `journey`
--

DROP TABLE IF EXISTS `journey`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `journey` (
  `JourneyId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamFocusId` int(11) DEFAULT '1',
  `SelectJourneyId` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  PRIMARY KEY (`JourneyId`),
  KEY `CreatedBy_idx` (`CreatedBy`),
  KEY `TeamFocusId_idx` (`TeamFocusId`),
  CONSTRAINT `CreatedByJourney` FOREIGN KEY (`CreatedBy`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TeamFocusIdJourney` FOREIGN KEY (`TeamFocusId`) REFERENCES `team_focus` (`TeamFocusId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `journey_practice`
--

DROP TABLE IF EXISTS `journey_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `journey_practice` (
  `JourneyPracticeId` int(11) NOT NULL,
  `JourneyId` int(11) DEFAULT NULL,
  `PracticeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`JourneyPracticeId`),
  KEY `JourneyId_idx` (`JourneyId`),
  KEY `PracticeId_idx` (`PracticeId`),
  CONSTRAINT `JourneyIdJP` FOREIGN KEY (`JourneyId`) REFERENCES `journey` (`JourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PracticeIdJP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `measure`
--

DROP TABLE IF EXISTS `measure`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `measure` (
  `MeasureId` int(11) NOT NULL AUTO_INCREMENT,
  `PracticeId` int(11) DEFAULT NULL,
  `Measure` varchar(250) NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `Why` varchar(250) DEFAULT NULL,
  `When` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`MeasureId`),
  KEY `PracticeId_idx` (`PracticeId`),
  CONSTRAINT `PracticeIdMeasure` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=283 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `member`
--

DROP TABLE IF EXISTS `member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `member` (
  `MemberId` int(11) NOT NULL AUTO_INCREMENT,
  `EmailAddress` varchar(100) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  PRIMARY KEY (`MemberId`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mission`
--

DROP TABLE IF EXISTS `mission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission` (
  `MissionId` int(11) NOT NULL,
  `TeamJourneyId` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  PRIMARY KEY (`MissionId`),
  KEY `TeamJourneyIdMission_idx` (`TeamJourneyId`),
  CONSTRAINT `TeamJourneyIdMission` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mission_member_measure_assesment`
--

DROP TABLE IF EXISTS `mission_member_measure_assesment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission_member_measure_assesment` (
  `MissionAssesmentId` int(11) NOT NULL,
  `MissionId` int(11) DEFAULT NULL,
  `MemberId` int(11) DEFAULT NULL,
  `MeasureId` int(11) DEFAULT NULL,
  `Assesment` int(11) DEFAULT NULL,
  PRIMARY KEY (`MissionAssesmentId`),
  KEY `MissionId_idx` (`MissionId`),
  KEY `MemberId_idx` (`MemberId`),
  KEY `MeasureId_idx` (`MeasureId`),
  CONSTRAINT `MeasureIdMMM` FOREIGN KEY (`MeasureId`) REFERENCES `measure` (`MeasureId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MemberIdMMM` FOREIGN KEY (`MemberId`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MissionIdMMM` FOREIGN KEY (`MissionId`) REFERENCES `mission` (`MissionId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mission_practice`
--

DROP TABLE IF EXISTS `mission_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission_practice` (
  `MissionPracticeId` int(11) NOT NULL,
  `MissionId` int(11) NOT NULL,
  `PracticeId` int(11) NOT NULL,
  PRIMARY KEY (`MissionPracticeId`),
  KEY `MissionIdMP_idx` (`MissionId`),
  KEY `PracticeIdMP_idx` (`PracticeId`),
  CONSTRAINT `PracticeIdMP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MissionIdMP` FOREIGN KEY (`MissionId`) REFERENCES `mission` (`MissionId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `practice`
--

DROP TABLE IF EXISTS `practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `practice` (
  `PracticeId` int(11) NOT NULL AUTO_INCREMENT,
  `FluencyLevelId` int(11) DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `SequenceNum` int(11) DEFAULT NULL,
  `PrerequisiteNum` int(11) DEFAULT NULL,
  PRIMARY KEY (`PracticeId`),
  KEY `FluencyLevelId_idx` (`FluencyLevelId`),
  CONSTRAINT `FluencyLevelId` FOREIGN KEY (`FluencyLevelId`) REFERENCES `fluency_level` (`FluencyLevelId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team`
--

DROP TABLE IF EXISTS `team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team` (
  `TeamId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Description` varchar(250) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `UpdatedBy` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamId`),
  KEY `CreatedByTeam_idx` (`CreatedBy`),
  KEY `UpdatedByTeam_idx` (`UpdatedBy`),
  CONSTRAINT `UpdatedByTeam` FOREIGN KEY (`UpdatedBy`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `CreatedByTeam` FOREIGN KEY (`CreatedBy`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_focus`
--

DROP TABLE IF EXISTS `team_focus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_focus` (
  `TeamFocusId` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`TeamFocusId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_journey`
--

DROP TABLE IF EXISTS `team_journey`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey` (
  `TeamJourneyId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamId` int(11) DEFAULT NULL,
  `JourneyId` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyId`),
  KEY `TeamId_idx` (`TeamId`),
  KEY `JourneyId_idx` (`JourneyId`),
  CONSTRAINT `JourneyIdTJ` FOREIGN KEY (`JourneyId`) REFERENCES `journey` (`JourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TeamIdTJ` FOREIGN KEY (`TeamId`) REFERENCES `team` (`TeamId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_journey_member`
--

DROP TABLE IF EXISTS `team_journey_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey_member` (
  `TeamJourneyMemberId` int(11) NOT NULL,
  `TeamJourneyId` int(11) DEFAULT NULL,
  `MemberId` int(11) DEFAULT NULL,
  `TeamJourneyMemberRoleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyMemberId`),
  KEY `MemberId_idx` (`MemberId`),
  KEY `TeamJourneyMemberRoleId_idx` (`TeamJourneyMemberRoleId`),
  KEY `TeamJourneyIdTJM_idx` (`TeamJourneyId`),
  CONSTRAINT `TeamJourneyIdTJM` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MemberIdTJM` FOREIGN KEY (`MemberId`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TeamJourneyMemberRoleIdTJM` FOREIGN KEY (`TeamJourneyMemberRoleId`) REFERENCES `team_journey_member_role` (`TeamJourneyMemberRoleId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_journey_member_role`
--

DROP TABLE IF EXISTS `team_journey_member_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey_member_role` (
  `TeamJourneyMemberRoleId` int(11) NOT NULL,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyMemberRoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_journey_practice`
--

DROP TABLE IF EXISTS `team_journey_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey_practice` (
  `TeamJourneyPracticeId` int(11) NOT NULL,
  `TeamJourneyId` int(11) DEFAULT NULL,
  `PracticeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyPracticeId`),
  KEY `PracticeIdTJP_idx` (`PracticeId`),
  KEY `TeamJourneyIdTJP_idx` (`TeamJourneyId`),
  CONSTRAINT `TeamJourneyIdTJP` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PracticeIdTJP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_member`
--

DROP TABLE IF EXISTS `team_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_member` (
  `TeamMemberId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamId` int(11) NOT NULL,
  `MemberId` int(11) NOT NULL,
  `TeamMemberRoleId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  PRIMARY KEY (`TeamMemberId`),
  KEY `Role_idx` (`TeamMemberRoleId`),
  CONSTRAINT `UserRoleIdTeamUser` FOREIGN KEY (`TeamMemberRoleId`) REFERENCES `team_member_role` (`TeamMemberRoleId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_member_role`
--

DROP TABLE IF EXISTS `team_member_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_member_role` (
  `TeamMemberRoleId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`TeamMemberRoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'gem'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-07-03 11:51:55
