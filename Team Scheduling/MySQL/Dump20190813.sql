-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: teamscheduling
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `locations`
--

DROP TABLE IF EXISTS `locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `locations` (
  `LocationId` int(11) NOT NULL AUTO_INCREMENT,
  `LocationName` varchar(45) NOT NULL,
  `StreetAddress` varchar(200) NOT NULL,
  `City` varchar(50) DEFAULT NULL,
  `State` char(2) DEFAULT NULL,
  `Zip` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`LocationId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `locations`
--

LOCK TABLES `locations` WRITE;
/*!40000 ALTER TABLE `locations` DISABLE KEYS */;
INSERT INTO `locations` VALUES (1,'Scottsdale District Park','9340 E Redfield Rd','Scottsdale','AZ','85260'),(2,'Gilbert District Park','90 E Civic Center Dr','Gilbert','AZ','85296'),(7,'Mesa District Park','15757 N 90TH PL','Scottsdale','AZ','85260');
/*!40000 ALTER TABLE `locations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schedulelocations`
--

DROP TABLE IF EXISTS `schedulelocations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `schedulelocations` (
  `ScheduleLocationsId` int(11) NOT NULL AUTO_INCREMENT,
  `ScheduleId` int(11) NOT NULL,
  `LocationId` int(11) NOT NULL,
  PRIMARY KEY (`ScheduleLocationsId`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schedulelocations`
--

LOCK TABLES `schedulelocations` WRITE;
/*!40000 ALTER TABLE `schedulelocations` DISABLE KEYS */;
INSERT INTO `schedulelocations` VALUES (3,7,1),(4,7,2),(5,1,2),(6,2,2),(12,3,2),(13,3,7),(24,5,1),(25,5,7),(28,4,1),(29,4,7),(32,6,1),(33,6,7),(34,7,1);
/*!40000 ALTER TABLE `schedulelocations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scheduler`
--

DROP TABLE IF EXISTS `scheduler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `scheduler` (
  `ScheduleId` int(11) NOT NULL AUTO_INCREMENT,
  `NumberOfGames` int(11) NOT NULL,
  `ScheduleType` varchar(45) NOT NULL,
  `Age` varchar(45) DEFAULT NULL,
  `StartDate` date NOT NULL,
  `EndDate` date NOT NULL,
  `BracketRule` varchar(45) NOT NULL,
  `StartTime` time DEFAULT NULL,
  `EndTime` time DEFAULT NULL,
  `TimeBetweenGames` int(11) NOT NULL,
  `Monday` tinyint(1) DEFAULT NULL,
  `Tuesday` tinyint(1) DEFAULT NULL,
  `Wednasday` tinyint(1) DEFAULT NULL,
  `Thursday` tinyint(1) DEFAULT NULL,
  `Friday` tinyint(1) DEFAULT NULL,
  `Saturday` tinyint(1) DEFAULT NULL,
  `Sunday` tinyint(1) DEFAULT NULL,
  `GameDuration` int(11) NOT NULL,
  PRIMARY KEY (`ScheduleId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduler`
--

LOCK TABLES `scheduler` WRITE;
/*!40000 ALTER TABLE `scheduler` DISABLE KEYS */;
INSERT INTO `scheduler` VALUES (3,1,'Playoffs','9U Minor','2019-08-01','2019-08-15','Single Elimination','13:00:00','18:00:00',20,1,1,0,0,1,1,1,0),(4,2,'Off Season','9U Major','2019-07-24','2019-08-15','Manual','11:00:00','17:00:00',20,0,0,0,0,1,1,1,0),(6,10,'Regular Season','10U Minor','2019-08-08','2019-08-30','Single Elimination','14:00:00','20:00:00',20,0,0,0,0,1,1,1,60),(7,20,'Regular Season','10U Major','2019-08-16','2019-08-25','Single Elimination','14:00:00','20:00:00',30,0,0,0,1,1,1,1,45);
/*!40000 ALTER TABLE `scheduler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teams`
--

DROP TABLE IF EXISTS `teams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `teams` (
  `teamid` int(11) NOT NULL AUTO_INCREMENT,
  `teamname` varchar(45) NOT NULL,
  PRIMARY KEY (`teamid`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teams`
--

LOCK TABLES `teams` WRITE;
/*!40000 ALTER TABLE `teams` DISABLE KEYS */;
INSERT INTO `teams` VALUES (1,'Team1'),(2,'Team2'),(3,'Team3'),(4,'Team4'),(5,'Team5'),(6,'Team6'),(7,'Team7');
/*!40000 ALTER TABLE `teams` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournamentschedule`
--

DROP TABLE IF EXISTS `tournamentschedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tournamentschedule` (
  `tournamentscheduleid` int(11) NOT NULL AUTO_INCREMENT,
  `teamone` int(11) NOT NULL,
  `teamtwo` int(11) NOT NULL,
  `locationid` int(11) NOT NULL,
  `gamedate` date NOT NULL,
  `starttime` time NOT NULL,
  `ScheduleId` int(11) NOT NULL,
  PRIMARY KEY (`tournamentscheduleid`)
) ENGINE=InnoDB AUTO_INCREMENT=135 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournamentschedule`
--

LOCK TABLES `tournamentschedule` WRITE;
/*!40000 ALTER TABLE `tournamentschedule` DISABLE KEYS */;
INSERT INTO `tournamentschedule` VALUES (75,1,6,2,'2019-08-02','13:00:00',3),(76,2,5,2,'2019-08-02','13:50:00',3),(77,3,4,2,'2019-08-02','14:40:00',3),(78,1,5,2,'2019-08-02','15:30:00',3),(79,6,4,2,'2019-08-02','16:20:00',3),(80,2,3,2,'2019-08-02','17:10:00',3),(81,1,4,7,'2019-08-02','13:00:00',3),(82,5,3,7,'2019-08-02','13:50:00',3),(83,6,2,7,'2019-08-02','14:40:00',3),(84,1,3,7,'2019-08-02','15:30:00',3),(85,4,2,7,'2019-08-02','16:20:00',3),(86,5,6,7,'2019-08-02','17:10:00',3),(87,1,2,2,'2019-08-03','13:00:00',3),(88,3,6,2,'2019-08-03','13:50:00',3),(89,4,5,2,'2019-08-03','14:40:00',3),(120,1,6,1,'2019-08-09','14:00:00',6),(121,2,5,1,'2019-08-09','15:20:00',6),(122,3,4,1,'2019-08-09','16:40:00',6),(123,1,5,1,'2019-08-09','18:00:00',6),(124,6,4,7,'2019-08-09','14:00:00',6),(125,2,3,7,'2019-08-09','15:20:00',6),(126,1,4,7,'2019-08-09','16:40:00',6),(127,5,3,7,'2019-08-09','18:00:00',6),(128,6,2,1,'2019-08-10','14:00:00',6),(129,1,3,1,'2019-08-10','15:20:00',6),(130,4,2,1,'2019-08-10','16:40:00',6),(131,5,6,1,'2019-08-10','18:00:00',6),(132,1,2,7,'2019-08-10','14:00:00',6),(133,3,6,7,'2019-08-10','15:20:00',6),(134,4,5,7,'2019-08-10','16:40:00',6);
/*!40000 ALTER TABLE `tournamentschedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournamentsteams`
--

DROP TABLE IF EXISTS `tournamentsteams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `tournamentsteams` (
  `tournamentsteamsid` int(11) NOT NULL AUTO_INCREMENT,
  `ScheduleId` int(11) NOT NULL,
  `TeamId` int(11) NOT NULL,
  PRIMARY KEY (`tournamentsteamsid`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournamentsteams`
--

LOCK TABLES `tournamentsteams` WRITE;
/*!40000 ALTER TABLE `tournamentsteams` DISABLE KEYS */;
INSERT INTO `tournamentsteams` VALUES (1,3,1),(2,3,2),(3,3,3),(4,3,4),(5,3,5),(6,3,6),(7,4,1),(8,4,2),(9,4,3),(10,4,4),(11,4,5),(12,4,6),(13,6,1),(14,6,2),(15,6,3),(16,6,4),(17,6,5),(18,6,6);
/*!40000 ALTER TABLE `tournamentsteams` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-08-13 23:15:16
