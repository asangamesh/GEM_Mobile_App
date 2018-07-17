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
  `FluencyLevelId` int(11) NOT NULL AUTO_INCREMENT,
  `Number` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `ShortName` varchar(30) NOT NULL,
  PRIMARY KEY (`FluencyLevelId`),
  UNIQUE KEY `FluencyLevelId_UNIQUE` (`FluencyLevelId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fluency_level`
--

LOCK TABLES `fluency_level` WRITE;
/*!40000 ALTER TABLE `fluency_level` DISABLE KEYS */;
INSERT INTO `fluency_level` VALUES (1,1,'Launch Delivery Team','Launch Team'),(2,1,'Launch Delivery Events','Launch Team'),(3,2,'Stabilize Team','Stabilize Team'),(4,2,'Cycle Metrics','Stabilize Team'),(5,3,'Time to Market Improvements','Mature Team'),(6,3,'Product Improvements','Mature Team'),(7,4,'Reconfigure Work Management','Support Team'),(8,4,'Support Working Agreement','Support Team'),(9,5,'Experimenting Team','Coach Team'),(10,5,'Coaching Working Agreement','Coach Team'),(11,1,'Launch Product Team','Launch Product Team'),(12,1,'Launch Product Events','Launch Product Team'),(13,2,'Product Vision','Product Vision and Roadmap'),(14,2,'Product Roadmap','Product Vision and Roadmap');
/*!40000 ALTER TABLE `fluency_level` ENABLE KEYS */;
UNLOCK TABLES;

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
  PRIMARY KEY (`JourneyId`),
  KEY `TeamFocusIdJourney_idx` (`TeamFocusId`),
  CONSTRAINT `TeamFocusIdJourney` FOREIGN KEY (`TeamFocusId`) REFERENCES `team_focus` (`TeamFocusId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journey`
--

LOCK TABLES `journey` WRITE;
/*!40000 ALTER TABLE `journey` DISABLE KEYS */;
INSERT INTO `journey` VALUES (1,1,1,1);
/*!40000 ALTER TABLE `journey` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `journey_practice`
--

DROP TABLE IF EXISTS `journey_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `journey_practice` (
  `JourneyPracticeId` int(11) NOT NULL AUTO_INCREMENT,
  `JourneyId` int(11) DEFAULT NULL,
  `PracticeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`JourneyPracticeId`),
  KEY `JourneyId_idx` (`JourneyId`),
  KEY `PracticeIdJP_idx` (`PracticeId`),
  CONSTRAINT `JourneyIdJP` FOREIGN KEY (`JourneyId`) REFERENCES `journey` (`JourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PracticeIdJP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journey_practice`
--

LOCK TABLES `journey_practice` WRITE;
/*!40000 ALTER TABLE `journey_practice` DISABLE KEYS */;
/*!40000 ALTER TABLE `journey_practice` ENABLE KEYS */;
UNLOCK TABLES;

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
  KEY `PracticeIdMeasure_idx` (`PracticeId`),
  CONSTRAINT `PracticeIdMeasure` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=251 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `measure`
--

LOCK TABLES `measure` WRITE;
/*!40000 ALTER TABLE `measure` DISABLE KEYS */;
INSERT INTO `measure` VALUES (1,1,'Team collaborates, is transparent and establishes norms.',NULL,NULL,NULL),(2,1,'Team establishes Purpose/Vision and Working Agreements.',NULL,NULL,NULL),(3,1,'Team agrees on how to keep each other accountable.',NULL,NULL,NULL),(4,1,'Team develops and maintain an On-boarding practice.',NULL,NULL,NULL),(5,1,'Team Leader resolves resistance to team forming activities.',NULL,NULL,NULL),(6,2,'Team creates and commits to use intake practice.',NULL,NULL,NULL),(7,2,'Team understands the value of the intake practice.',NULL,NULL,NULL),(8,2,'Team collaborates to develop intake practice.',NULL,NULL,NULL),(9,2,'Team Leader ensures intake process is documented and available.',NULL,NULL,NULL),(10,2,'Team Leader  resolves resistance to use of intake practice.',NULL,NULL,NULL),(11,3,'Team adopts work management framework.',NULL,NULL,NULL),(12,3,'Team understands best approach to manage their work.',NULL,NULL,NULL),(13,3,'Team understands framework and how it helps manage work.',NULL,NULL,NULL),(14,3,'Team actively participates in implementing their framework.',NULL,NULL,NULL),(15,3,'Team Leader resolves resistance to selecting and adopting a framework.',NULL,NULL,NULL),(16,4,'Team Leader  schedules all meetings/events in advance with clearly articulated value.',NULL,NULL,NULL),(17,4,'Team Leader  includes purpose and agenda for all meetings in invites, at the meeting, and meeting close to confirm achievement.',NULL,NULL,NULL),(18,4,'Team understands the value of all events and participates.',NULL,NULL,NULL),(19,4,'Team ensures agreed tasks are updated and accurate prior to events.',NULL,NULL,NULL),(20,4,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(21,5,'Team leader helps the team make their work visible.',NULL,NULL,NULL),(22,5,'Product Management team assesses current needs and backlog maturity.',NULL,NULL,NULL),(23,5,'Team Leader understands vision, roadmap and manages expectations.',NULL,NULL,NULL),(24,5,'Team understands the Vision, Roadmap, and how work is broken down.',NULL,NULL,NULL),(25,5,'Team Leader  resolves and escalates issues related to managing backlog.',NULL,NULL,NULL),(26,6,'Team Leader  facilitate Daily Planning meeting, respecting time box.',NULL,NULL,NULL),(27,6,'Focus of meeting on managing work, not people.',NULL,NULL,NULL),(28,6,'Team discuss highest business value work items in priority order.',NULL,NULL,NULL),(29,6,'Team collaborates and understands the value of meeting.',NULL,NULL,NULL),(30,6,'Team Leader  helps team meet face to face.',NULL,NULL,NULL),(31,7,'Team Leader facilitates event.',NULL,NULL,NULL),(32,7,'Team understand event purpose and goals.',NULL,NULL,NULL),(33,7,'Team understands the value of planning together.',NULL,NULL,NULL),(34,7,'Team collaborates and commits together.',NULL,NULL,NULL),(35,7,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(36,8,'Team Leader facilitates event.',NULL,NULL,NULL),(37,8,'Team Leader identifies and manages raised issues and risks.',NULL,NULL,NULL),(38,8,'Team understands the value of developing a shared vision.',NULL,NULL,NULL),(39,8,'Team participates in refining and understands work.',NULL,NULL,NULL),(40,8,'Team Leader  helps team meet face to face.',NULL,NULL,NULL),(41,9,'Team Leader identifies and manages issues and risks raised during the event.',NULL,NULL,NULL),(42,9,'Team shares accomplishments.',NULL,NULL,NULL),(43,9,'Team and stakeholders participate in event.',NULL,NULL,NULL),(44,9,'Team and stakeholders identify new backlog items.',NULL,NULL,NULL),(45,9,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(46,10,'Team Leader plans and facilitates event.',NULL,NULL,NULL),(47,10,'Improvements are identified and tracked.',NULL,NULL,NULL),(48,10,'Team understands value of improving.',NULL,NULL,NULL),(49,10,'Team actively participates.',NULL,NULL,NULL),(50,10,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(51,11,'Team reviews how work flows through the system to identify bottle necks.',NULL,NULL,NULL),(52,11,'Team can articulate framework roles, artifacts and events and value.',NULL,NULL,NULL),(53,11,'Team collaborates to configure work management framework.',NULL,NULL,NULL),(54,11,'Team has been trained in work management framework.',NULL,NULL,NULL),(55,11,'Team Leader ensures all people involved in flow are present and reviews bottlenecks.',NULL,NULL,NULL),(56,12,'Team Leader facilitates with help of the Product Owner.',NULL,NULL,NULL),(57,12,'Team uses Definition of Ready to determine if the story is ready for refinement.',NULL,NULL,NULL),(58,12,'Team collaborates to understand user stories and acceptance criteria.',NULL,NULL,NULL),(59,12,'Team conducts relative sizing to support Product Owner in prioritization.',NULL,NULL,NULL),(60,12,'Team Leader ensures and helps team use  Definition of Ready.',NULL,NULL,NULL),(61,13,'Team Leader facilitates the Daily Stand Up.',NULL,NULL,NULL),(62,13,'Team understands the value and focus is to discuss 24 hour planning.',NULL,NULL,NULL),(63,13,'Team ensures the state of all work is up to date so they can plan for the next 24 hours.',NULL,NULL,NULL),(64,13,'Team is focused on how to finish work in the next 24 hours.',NULL,NULL,NULL),(65,13,'Team Leader helps team focus on planning and commitments.',NULL,NULL,NULL),(66,14,'People do not report status to the Team Leader.',NULL,NULL,NULL),(67,14,'Team Leader tracks progress and helps the team see trends.',NULL,NULL,NULL),(68,14,'Team blocks work that cannot be progressed.',NULL,NULL,NULL),(69,14,'Information Radiators are reviewed during Daily Stand Up.',NULL,NULL,NULL),(70,14,'Team Leader  encourages the team to problem solve to remove impediments.',NULL,NULL,NULL),(71,15,'Team Leader tracks and removes impediments and dependencies.',NULL,NULL,NULL),(72,15,'Team Leader gets the right people together when impediments and dependencies are reported.',NULL,NULL,NULL),(73,15,'Team Leader asks questions to surface impediments and dependencies.',NULL,NULL,NULL),(74,15,'The team participates in meetings to remove impediments and dependencies.',NULL,NULL,NULL),(75,15,'Team Leader  addresses resistance to resolving impediments and dependencies and escalates as needed.',NULL,NULL,NULL),(76,16,'Sprint commitment is reviewed as part of the Sprint Review/Demo to identify actuals vs. plan according to DoD.',NULL,NULL,NULL),(77,16,'Team discusses incomplete work and how it impacts the Product Roadmap.',NULL,NULL,NULL),(78,16,'Stakeholders participate and give feedback.',NULL,NULL,NULL),(79,16,'Stakeholders are enthusiastic about progress and endorse product/features within circle of influence.',NULL,NULL,NULL),(80,16,'Team Leader identifies regular impediments so the team can address them.',NULL,NULL,NULL),(81,17,'Team Leader  is focused on bringing areas of improvement to light.',NULL,NULL,NULL),(82,17,'Team reviews the Team Continuous Improvement Backlog moving items from Doing to Done as agreed. ',NULL,NULL,NULL),(83,17,'Team improvements are reviewed and celebrated during the meeting.',NULL,NULL,NULL),(84,17,'Retrospective includes: (\r a) Set the stage - review iteration (\r b) Gather data – gather the how (\r c) Generate insights – determine the what (\r d) Decide',NULL,NULL,NULL),(85,17,'Team Leader  plans the Retrospective to bring value to the team.',NULL,NULL,NULL),(86,18,'Team Leader ensures team reviews proposed Release Backlog prior to planning.',NULL,NULL,NULL),(87,18,'Team understands business value and vision of each Feature.',NULL,NULL,NULL),(88,18,'Team conducts relative sizing for more clarity.',NULL,NULL,NULL),(89,18,'Team identifies Features not Ready according to Definition of Ready and provides information to the Product Owner.',NULL,NULL,NULL),(90,18,'Team Leader notes and tracks issues raised by the team to ensure all Features are Ready for Release Planning.',NULL,NULL,NULL),(91,19,'Team Leader facilitates discussions to help clarify requirements prior to Iteration Planning.',NULL,NULL,NULL),(92,19,'Team estimates tasks and reviews together to ensure diverse collaboration.',NULL,NULL,NULL),(93,19,'Team reviews planning trends as input.',NULL,NULL,NULL),(94,19,'Team reviews capacity before committing to work.',NULL,NULL,NULL),(95,19,'Everyone doing the work commits, as a team.',NULL,NULL,NULL),(96,20,'Team Leader tracks agreed measures throughout the Iteration.',NULL,NULL,NULL),(97,20,'Team agrees on process used to track progress.',NULL,NULL,NULL),(98,20,'Team agrees to update progress every day in order to track according to agreement.',NULL,NULL,NULL),(99,20,'Team discusses agreed progress every day (usually at the Daily Stand Up).',NULL,NULL,NULL),(100,20,'Team Leader  facilitates progress discussions as agreed.',NULL,NULL,NULL),(101,21,'Team Leader supports the team in developing a quality plan.',NULL,NULL,NULL),(102,21,'Team Leader ensures all aspects of incident management are included in the quality plan.',NULL,NULL,NULL),(103,21,'Team actively participates in developing the quality plan.',NULL,NULL,NULL),(104,21,'Team identifies cross-functional ways they can support quality.',NULL,NULL,NULL),(105,21,'Team Leader helps bring the right people together to create the quality plan.',NULL,NULL,NULL),(106,22,'Team Leader ensure number of defects found in accepted work is tracked.',NULL,NULL,NULL),(107,22,'Team Leader discusses defect data with team to identify opportunies for improvement.',NULL,NULL,NULL),(108,22,'Team reviews defects identified in accepted work to look for trends and patterns.',NULL,NULL,NULL),(109,22,'Team adopts approaches to reduce the number of defects discovered in accepted work.',NULL,NULL,NULL),(110,22,'Team Leader facilitates working agreements and helps the team build accountability measures.',NULL,NULL,NULL),(111,23,'Team Leader ensures aging defects and defect fidelity are being tracked.',NULL,NULL,NULL),(112,23,'Team reviews aging defects to identify trends for deprioritized defects.',NULL,NULL,NULL),(113,23,'Team reviews ratio between work completed and defects to identify trends.',NULL,NULL,NULL),(114,23,'The team discusses aging defects findings with Product Owner to agree better approaches.',NULL,NULL,NULL),(115,23,'The team adopts approaches to improve testing in the development environemnt.',NULL,NULL,NULL),(116,24,'Team Leader ensures team actively tracks bottlenecks.',NULL,NULL,NULL),(117,24,'Team reviews defect flow trend in all work states to uncover bottlenecks in flow.',NULL,NULL,NULL),(118,24,'Team reviews the ratio of work being completed and accepted to understand clarity bottlenecks.',NULL,NULL,NULL),(119,24,'Team determines expected defect completion based on priority to identify bottlenecks.',NULL,NULL,NULL),(120,24,'Team identifies ways to remove bottlenecks and improve quality processes.',NULL,NULL,NULL),(121,25,'Team Leader facilitates discussions to develop a DevOps plan with team and stakeholders as needed.',NULL,NULL,NULL),(122,25,'Team collaborates to define how Ops will be included in their development approach.',NULL,NULL,NULL),(123,25,'Team collaborates to define how to create CI/CD pipeline.',NULL,NULL,NULL),(124,25,'Team Leader identifies industry standards and other examples of good in the industry/company.',NULL,NULL,NULL),(125,25,'Team develops a DevOps plan/roadmap that incudes culture, process and tools.',NULL,NULL,NULL),(126,26,'Team Leader helps facilitate discussions between relevant people to disucss test scenarios as part of work acceptance.',NULL,NULL,NULL),(127,26,'Team identifies the types of scenarios needed.',NULL,NULL,NULL),(128,26,'Team collaborates with relevant people to agree to Scenario format.',NULL,NULL,NULL),(129,26,'Team commits to writing unit test prior to coding to validate understanding.',NULL,NULL,NULL),(130,26,'Team Leader removes impediments and facilitates difficult conversations.',NULL,NULL,NULL),(131,27,'Team Leader facilitates Automated Testing approach discussions.',NULL,NULL,NULL),(132,27,'Team collaborates on how to impliment automated testing within the team, addressing cross-functional approach.',NULL,NULL,NULL),(133,27,'Team identifies tools needed to implement Automated Testing Approach.',NULL,NULL,NULL),(134,27,'Team develops Automated Test plan with business case for funding and reduced capacity.',NULL,NULL,NULL),(135,27,'Team Leader supports team by helping with developing plan and escalating for approval.',NULL,NULL,NULL),(136,28,'Team Leader facilitates discussion regarding Quality Coding Standards with the team.',NULL,NULL,NULL),(137,28,'Team collaborates on Quality Coding Standards with the intent of including in their DevOps approach.',NULL,NULL,NULL),(138,28,'Team agrees to a cross-functional approach to be used as mentoring and learning opportunities.',NULL,NULL,NULL),(139,28,'Team Leader supports team as needed with documentation and other research items.',NULL,NULL,NULL),(140,28,'Team Leader facilitates difficult conversations within the team to ensure fluency across all members.',NULL,NULL,NULL),(141,29,'Team Leader faciliates discussion with team to develop a Paired Programming approach.',NULL,NULL,NULL),(142,29,'Team collaborates on variety of approaches to support cross-functional knowledge across team.',NULL,NULL,NULL),(143,29,'Team agrees to and impliments a Paired Programming approach.',NULL,NULL,NULL),(144,29,'Team Leader researches and provides ideas as input into team discussions.',NULL,NULL,NULL),(145,29,'Team Leader helps team come to consensus on approach.',NULL,NULL,NULL),(146,30,'Team Leader faciliates discussion with team to develop a CI/CD Pipeline Plan.',NULL,NULL,NULL),(147,30,'Team collaborates on CI/CD approach.',NULL,NULL,NULL),(148,30,'Team identifies pre-condions, post-conditions and stages (roadmap) to implimentation.',NULL,NULL,NULL),(149,30,'Team identifies tools needed to implement CI/CD Pipeline.',NULL,NULL,NULL),(150,30,'Team Leader supports team by helping with developing plan and escalating for approval.',NULL,NULL,NULL),(151,31,'Team collaborates, is transparent and establishes norms.',NULL,NULL,NULL),(152,31,'Team establishes Purpose/Vision and Working Agreements.',NULL,NULL,NULL),(153,31,'Team agrees on how to keep each other accountable.',NULL,NULL,NULL),(154,31,'Team develops and maintain an On-boarding practice.',NULL,NULL,NULL),(155,31,'Team Leader resolves resistance to team forming activities.',NULL,NULL,NULL),(156,32,'Team Leader establishes a team to ideate and refine requests.',NULL,NULL,NULL),(157,32,'Team collaborates while evaluating ideas and requests.',NULL,NULL,NULL),(158,32,'Team establishes early indicators and expected outcomes.',NULL,NULL,NULL),(159,32,'Team prioritizes work for intake into the product backlog',NULL,NULL,NULL),(160,32,'Team Leader resolves issues and impediments for the team.',NULL,NULL,NULL),(161,33,'Team adopts work management framework.',NULL,NULL,NULL),(162,33,'Team understands best approach to manage their work.',NULL,NULL,NULL),(163,33,'Team understands framework and how it helps manage work.',NULL,NULL,NULL),(164,33,'Team actively participates in implementing their framework.',NULL,NULL,NULL),(165,33,'Team Leader resolves resistance to selecting and adopting a framework.',NULL,NULL,NULL),(166,34,'Team Leader  schedules all meetings/events in advance with clearly articulated value.',NULL,NULL,NULL),(167,34,'Team Leader  includes purpose and agenda for all meetings in invites, at the meeting, and meeting close to confirm achievement.',NULL,NULL,NULL),(168,34,'Team understands the value of all events and participates.',NULL,NULL,NULL),(169,34,'Team ensures agreed tasks are updated and accurate prior to events.',NULL,NULL,NULL),(170,34,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(171,35,'Team leader helps the team make their work visible.',NULL,NULL,NULL),(172,35,'Product Management team assesses current needs and backlog maturity.',NULL,NULL,NULL),(173,35,'Team Leader understands vision, roadmap and manages expectations.',NULL,NULL,NULL),(174,35,'Team understands the Vision, Roadmap, and how work is broken down.',NULL,NULL,NULL),(175,35,'Team Leader  resolves and escalates issues related to managing backlog.',NULL,NULL,NULL),(176,36,'Team Leader  facilitate Daily Planning meeting, respecting time box.',NULL,NULL,NULL),(177,36,'Focus of meeting on managing work, not people.',NULL,NULL,NULL),(178,36,'Team discuss highest business value work items in priority order.',NULL,NULL,NULL),(179,36,'Team collaborates and understands the value of meeting.',NULL,NULL,NULL),(180,36,'Team Leader  helps team meet face to face.',NULL,NULL,NULL),(181,37,'Team Leader facilitates event.',NULL,NULL,NULL),(182,37,'Team understand event purpose and goals.',NULL,NULL,NULL),(183,37,'Team understands the value of planning together.',NULL,NULL,NULL),(184,37,'Team collaborates and commits together.',NULL,NULL,NULL),(185,37,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(186,38,'Team Leader identifies the people needed to support backlog development.',NULL,NULL,NULL),(187,38,'Team reviews and refines backlog items.',NULL,NULL,NULL),(188,38,'Team understands the vision and expected outcomes of each backlog item.',NULL,NULL,NULL),(189,38,'Team Leader is able to clearly articulate business value and vision.',NULL,NULL,NULL),(190,38,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(191,39,'Team Leader identifies and manages issues and risks raised during the event.',NULL,NULL,NULL),(192,39,'Team shares accomplishments.',NULL,NULL,NULL),(193,39,'Team and stakeholders participate in event.',NULL,NULL,NULL),(194,39,'Team and stakeholders identify new backlog items.',NULL,NULL,NULL),(195,39,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(196,40,'Team Leader plans and facilitates event.',NULL,NULL,NULL),(197,40,'Improvements are identified and tracked.',NULL,NULL,NULL),(198,40,'Team understands value of improving.',NULL,NULL,NULL),(199,40,'Team actively participates.',NULL,NULL,NULL),(200,40,'Team Leader helps team meet face to face.',NULL,NULL,NULL),(201,41,'Product team is responsible for a family of product and services',NULL,NULL,NULL),(202,41,'Business value stream includes strategic stakeholders from concept to cash.',NULL,NULL,NULL),(203,41,'Product team breaks work into units that can be taken into the value stream. ',NULL,NULL,NULL),(204,41,'Program team leads work together to map work units to teams in the value stream.',NULL,NULL,NULL),(205,41,'Product team lead removes impediments to value stream realization.',NULL,NULL,NULL),(206,42,'Product team has documented and can clearly articulate product vision and ROI.',NULL,NULL,NULL),(207,42,'Product team has documented and clearly articulate market differentiators and potential market share increases.',NULL,NULL,NULL),(208,42,'Product team has documented and clearly articulate potential new customer expectations.',NULL,NULL,NULL),(209,42,'Product Team has documented and clearly articulate expected increased revenue.',NULL,NULL,NULL),(210,42,'Product team lead is able to remove impediments to clarity.',NULL,NULL,NULL),(211,43,'Product team lead has documented required product increments to achieve vision.',NULL,NULL,NULL),(212,43,'Product team collaborates to map product increments according to business value.',NULL,NULL,NULL),(213,43,'Program Team Leaders agree to large product increments to be completed.',NULL,NULL,NULL),(214,43,'Product Team prioritizes product increments based on business value.',NULL,NULL,NULL),(215,43,'Product team leader clarifies any misunderstanding and implements.',NULL,NULL,NULL),(216,44,'Product team identifies Pre-conditions to move products to develop.',NULL,NULL,NULL),(217,44,'Product team identifies Post-conditions to product delivery and acceptance criteria.',NULL,NULL,NULL),(218,44,'Product team identifies Pre-conditions for each product increment.',NULL,NULL,NULL),(219,44,'Product team identifies Post-conditions of each product increment.',NULL,NULL,NULL),(220,44,'Product team agrees on product scope.',NULL,NULL,NULL),(221,45,'Product team identifies solution constraints.',NULL,NULL,NULL),(222,45,'Program Team discusses constraints for clarity.',NULL,NULL,NULL),(223,45,'Product team identifies dependencies across the value stream.',NULL,NULL,NULL),(224,45,'Product team collaborate to identify ways to remove and/or manage non-value add constraints.',NULL,NULL,NULL),(225,45,'Product team Leader ensures everyone who should be involved contribute to the conversation.',NULL,NULL,NULL),(226,46,'Product team works across value stream leads to identify dependencies.',NULL,NULL,NULL),(227,46,'Product team collaborates to identify product increments dependencies.',NULL,NULL,NULL),(228,46,'Product team collaborates to remove dependencies',NULL,NULL,NULL),(229,46,'Product team collaborates to sequence product increments based on dependencies.',NULL,NULL,NULL),(230,46,'Product team lead helps team track dependencies',NULL,NULL,NULL),(231,47,'Product team breaks Product increments into feature sets.',NULL,NULL,NULL),(232,47,'Product team determines business value for each feature set.',NULL,NULL,NULL),(233,47,'Technical difficulty is identified for each feature set.',NULL,NULL,NULL),(234,47,'Product team works with technical Leaders to breakdown feature sets.',NULL,NULL,NULL),(235,47,'Product team prioritizes feature sets according to business value and technical difficulty',NULL,NULL,NULL),(236,48,'Product team breaks Product increments into features',NULL,NULL,NULL),(237,48,'Product team determines business value for each feature.',NULL,NULL,NULL),(238,48,'Technical difficulty is identified for each feature.',NULL,NULL,NULL),(239,48,'Product team works with technical Leaders to breakdown features.',NULL,NULL,NULL),(240,48,'Product team prioritizes feature according to business value and technical difficulty',NULL,NULL,NULL),(241,49,'Product team lead determines release cadence.',NULL,NULL,NULL),(242,49,'Product team lead shares release cadence across value stream.',NULL,NULL,NULL),(243,49,'Product team lead collaborates with other Leaders to plan release cadence.',NULL,NULL,NULL),(244,49,'Program Team provides release cadence to product and delivery teams.',NULL,NULL,NULL),(245,49,'Product team lead removes impediments to release cadence implementation',NULL,NULL,NULL),(246,50,'Product team lead create product Roadmap planning cadence',NULL,NULL,NULL),(247,50,'Product team Maps product increment to product Roadmap.',NULL,NULL,NULL),(248,50,'Product team maps feature sets to product Roadmap.',NULL,NULL,NULL),(249,50,'Product team maps features to product Roadmap.',NULL,NULL,NULL),(250,50,'Product team collaborates with Stakeholders and others to validated release of Roadmap.',NULL,NULL,NULL);
/*!40000 ALTER TABLE `measure` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `member`
--

LOCK TABLES `member` WRITE;
/*!40000 ALTER TABLE `member` DISABLE KEYS */;
/*!40000 ALTER TABLE `member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `member_mission_practice`
--

DROP TABLE IF EXISTS `member_mission_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `member_mission_practice` (
  `MemberMissionPracticeId` int(11) NOT NULL AUTO_INCREMENT,
  `MemberId` int(11) DEFAULT NULL,
  `MissionPracticeId` int(11) DEFAULT NULL,
  `RejectForReason` varchar(1000) CHARACTER SET utf8 DEFAULT NULL,
  `Status` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`MemberMissionPracticeId`),
  KEY `MemberId_idx` (`MemberId`),
  KEY `MissionPracticeID_idx` (`MissionPracticeId`),
  CONSTRAINT `MemberIdMMP` FOREIGN KEY (`MemberId`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MissionPracticeIDMMP` FOREIGN KEY (`MissionPracticeId`) REFERENCES `mission_practice` (`MissionPracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `member_mission_practice`
--

LOCK TABLES `member_mission_practice` WRITE;
/*!40000 ALTER TABLE `member_mission_practice` DISABLE KEYS */;
/*!40000 ALTER TABLE `member_mission_practice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mission`
--

DROP TABLE IF EXISTS `mission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission` (
  `MissionId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamJourneyId` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  PRIMARY KEY (`MissionId`),
  KEY `TeamJourneyIdMission_idx` (`TeamJourneyId`),
  CONSTRAINT `TeamJourneyIdMission` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=139 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mission`
--

LOCK TABLES `mission` WRITE;
/*!40000 ALTER TABLE `mission` DISABLE KEYS */;
/*!40000 ALTER TABLE `mission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mission_member_measure_assesment`
--

DROP TABLE IF EXISTS `mission_member_measure_assesment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission_member_measure_assesment` (
  `MissionAssesmentId` int(11) NOT NULL AUTO_INCREMENT,
  `MissionId` int(11) DEFAULT NULL,
  `MemberId` int(11) DEFAULT NULL,
  `MeasureId` int(11) DEFAULT NULL,
  `Assesment` int(11) DEFAULT NULL,
  PRIMARY KEY (`MissionAssesmentId`),
  KEY `MemberId_idx` (`MemberId`),
  KEY `MissionIdMMM_idx` (`MissionId`),
  KEY `MeasureIdMMM_idx` (`MeasureId`),
  CONSTRAINT `MeasureIdMMM` FOREIGN KEY (`MeasureId`) REFERENCES `measure` (`MeasureId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MemberIdMMM` FOREIGN KEY (`MemberId`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MissionIdMMM` FOREIGN KEY (`MissionId`) REFERENCES `mission` (`MissionId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mission_member_measure_assesment`
--

LOCK TABLES `mission_member_measure_assesment` WRITE;
/*!40000 ALTER TABLE `mission_member_measure_assesment` DISABLE KEYS */;
/*!40000 ALTER TABLE `mission_member_measure_assesment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mission_practice`
--

DROP TABLE IF EXISTS `mission_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mission_practice` (
  `MissionPracticeId` int(11) NOT NULL AUTO_INCREMENT,
  `MissionId` int(11) NOT NULL,
  `PracticeId` int(11) NOT NULL,
  PRIMARY KEY (`MissionPracticeId`),
  KEY `MissionIdMP_idx` (`MissionId`),
  KEY `PracticeIdMP_idx` (`PracticeId`),
  CONSTRAINT `PracticeIdMP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MissionIdMP` FOREIGN KEY (`MissionId`) REFERENCES `mission` (`MissionId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=283 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mission_practice`
--

LOCK TABLES `mission_practice` WRITE;
/*!40000 ALTER TABLE `mission_practice` DISABLE KEYS */;
/*!40000 ALTER TABLE `mission_practice` ENABLE KEYS */;
UNLOCK TABLES;

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
  KEY `FluencyLevelIdPractice_idx` (`FluencyLevelId`),
  CONSTRAINT `FluencyLevelIdPractice` FOREIGN KEY (`FluencyLevelId`) REFERENCES `fluency_level` (`FluencyLevelId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `practice`
--

LOCK TABLES `practice` WRITE;
/*!40000 ALTER TABLE `practice` DISABLE KEYS */;
INSERT INTO `practice` VALUES (1,1,'Form Delivery Team',1,1),(2,1,'Intake Work',1,2),(3,1,'Manage Work',1,3),(4,1,'Event Schedule',1,4),(5,1,'Manage Backlog',1,5),(6,2,'Plan Daily',1,1),(7,2,'Plan Iteration',1,2),(8,2,'Refine Work',1,3),(9,2,'Iteration Review',1,4),(10,2,'Inspect & Adapt',1,5),(11,3,'Launch Configured Framework',1,1),(12,3,'Advanced Backlog Refinement',1,2),(13,3,'Execute Daily Planning',1,3),(14,3,'Daily Planning Predictability',1,4),(15,3,'Impediments & Dependencies',1,5),(16,4,'Advanced Iteration Review',1,1),(17,4,'Advanced Inspect & Adapt',1,2),(18,4,'Advanced Iteration Planning',1,3),(19,4,'Scope Accepted',1,4),(20,4,'Track Progress',1,5),(21,6,'Quality Plan',1,1),(22,6,'Defect Discovery',1,2),(23,6,'Defect Management',1,3),(24,6,'Bottleneck Tracking',1,4),(25,6,'DevOps Roadmap',1,5),(26,5,'Test Scenarios',1,1),(27,5,'Automated Testing Plan',1,2),(28,5,'Quality Coding Standards',1,3),(29,5,'Paired Programming Aproach',1,4),(30,5,'CI/CD Pipeline Plan',1,5),(31,11,'Form Product Team',1,1),(32,11,'Product Discovery',1,2),(33,11,'Manage Product Work',1,3),(34,11,'Event Cadence',1,4),(35,11,'Manage Product Backlog',1,5),(36,12,'Daily Planning',1,1),(37,12,'Plan Iteration',1,2),(38,12,'Backlog Collaboration',1,3),(39,12,'Iteration Review',1,4),(40,12,'Inspect & Adapt',1,5),(41,13,'Value Stream',1,1),(42,13,'Product Vision',1,2),(43,13,'Product Mission',1,3),(44,13,'Product Increment Scope',1,4),(45,13,'Product Constraints',1,5),(46,14,'Product Dependencies',1,1),(47,14,'Product Feature Sets',1,2),(48,14,'Product Features',1,3),(49,14,'Release Cadence',1,4),(50,14,'Release Roadmap',1,5);
/*!40000 ALTER TABLE `practice` ENABLE KEYS */;
UNLOCK TABLES;

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
  PRIMARY KEY (`TeamId`)
) ENGINE=InnoDB AUTO_INCREMENT=170 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team`
--

LOCK TABLES `team` WRITE;
/*!40000 ALTER TABLE `team` DISABLE KEYS */;
/*!40000 ALTER TABLE `team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `team_focus`
--

DROP TABLE IF EXISTS `team_focus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_focus` (
  `TeamFocusId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`TeamFocusId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team_focus`
--

LOCK TABLES `team_focus` WRITE;
/*!40000 ALTER TABLE `team_focus` DISABLE KEYS */;
INSERT INTO `team_focus` VALUES (1,'Delivery');
/*!40000 ALTER TABLE `team_focus` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=122 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team_journey`
--

LOCK TABLES `team_journey` WRITE;
/*!40000 ALTER TABLE `team_journey` DISABLE KEYS */;
/*!40000 ALTER TABLE `team_journey` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `team_journey_member`
--

DROP TABLE IF EXISTS `team_journey_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey_member` (
  `TeamJourneyMemberId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamJourneyId` int(11) DEFAULT NULL,
  `MemberId` int(11) DEFAULT NULL,
  `TeamJourneyMemberRoleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyMemberId`),
  UNIQUE KEY `TeamMemberUnique` (`TeamJourneyId`,`MemberId`),
  KEY `MemberId_idx` (`MemberId`),
  KEY `TeamJourneyIdTJM_idx` (`TeamJourneyId`),
  KEY `TeamJourneyMemberIdTJM_idx` (`TeamJourneyMemberRoleId`),
  CONSTRAINT `TeamJourneyMemberIdTJM` FOREIGN KEY (`TeamJourneyMemberRoleId`) REFERENCES `team_journey_member_role` (`TeamJourneyMemberRoleId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `MemberIdTJM` FOREIGN KEY (`MemberId`) REFERENCES `member` (`MemberId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TeamJourneyIdTJM` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=118 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team_journey_member`
--

LOCK TABLES `team_journey_member` WRITE;
/*!40000 ALTER TABLE `team_journey_member` DISABLE KEYS */;
/*!40000 ALTER TABLE `team_journey_member` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `team_journey_member_role`
--

LOCK TABLES `team_journey_member_role` WRITE;
/*!40000 ALTER TABLE `team_journey_member_role` DISABLE KEYS */;
INSERT INTO `team_journey_member_role` VALUES (1,'Team Leader'),(2,'Team Member');
/*!40000 ALTER TABLE `team_journey_member_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `team_journey_practice`
--

DROP TABLE IF EXISTS `team_journey_practice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_journey_practice` (
  `TeamJourneyPracticeId` int(11) NOT NULL AUTO_INCREMENT,
  `TeamJourneyId` int(11) DEFAULT NULL,
  `PracticeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`TeamJourneyPracticeId`),
  KEY `TeamJourneyIdTJP_idx` (`TeamJourneyId`),
  KEY `PracticeIdTJP_idx` (`PracticeId`),
  CONSTRAINT `PracticeIdTJP` FOREIGN KEY (`PracticeId`) REFERENCES `practice` (`PracticeId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TeamJourneyIdTJP` FOREIGN KEY (`TeamJourneyId`) REFERENCES `team_journey` (`TeamJourneyId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team_journey_practice`
--

LOCK TABLES `team_journey_practice` WRITE;
/*!40000 ALTER TABLE `team_journey_practice` DISABLE KEYS */;
/*!40000 ALTER TABLE `team_journey_practice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'gem'
--

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

-- Dump completed on 2018-07-17 15:03:17
