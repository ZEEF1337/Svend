-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               5.7.24 - MySQL Community Server (GPL)
-- Server OS:                    Win64
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for svend
DROP DATABASE IF EXISTS `svend`;
CREATE DATABASE IF NOT EXISTS `svend` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin */;
USE `svend`;

-- Dumping structure for table svend.tickets
DROP TABLE IF EXISTS `tickets`;
CREATE TABLE IF NOT EXISTS `tickets` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Kategori` int(11) NOT NULL,
  `Title` varchar(500) COLLATE utf8_bin NOT NULL,
  `Body` varchar(7500) COLLATE utf8_bin NOT NULL,
  `CreationDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Status` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`ID`),
  KEY `TicketUserIDFK` (`UserID`),
  KEY `TicketKategoriFK` (`Kategori`),
  KEY `TicketStatusFK` (`Status`),
  CONSTRAINT `TicketKategoriFK` FOREIGN KEY (`Kategori`) REFERENCES `ticket_categories` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TicketStatusFK` FOREIGN KEY (`Status`) REFERENCES `ticket_status` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TicketUserIDFK` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.tickets: ~4 rows (approximately)
DELETE FROM `tickets`;
/*!40000 ALTER TABLE `tickets` DISABLE KEYS */;
INSERT INTO `tickets` (`ID`, `UserID`, `Kategori`, `Title`, `Body`, `CreationDate`, `Status`) VALUES
	(1, 3, 1, 'Jeg har nogle spørgsmål inden jeg køber', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book', '2020-06-14 11:07:27', 1),
	(2, 3, 2, 'Jeg kunne bare gerne tænke mig den her funktion!', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book', '2020-06-14 11:08:48', 1),
	(3, 3, 3, 'Jeg har fundet en fejl', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book', '2020-06-14 11:09:07', 1),
	(4, 3, 4, 'Jeg er altså løbet ind i bette problem', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book', '2020-06-14 11:09:36', 2);
/*!40000 ALTER TABLE `tickets` ENABLE KEYS */;

-- Dumping structure for table svend.ticket_assigned
DROP TABLE IF EXISTS `ticket_assigned`;
CREATE TABLE IF NOT EXISTS `ticket_assigned` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `TicketID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `TicketAssignedTicketIDFK` (`TicketID`),
  KEY `TicketAssignedUserIDFK` (`UserID`),
  CONSTRAINT `TicketAssignedTicketIDFK` FOREIGN KEY (`TicketID`) REFERENCES `tickets` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `TicketAssignedUserIDFK` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.ticket_assigned: ~1 rows (approximately)
DELETE FROM `ticket_assigned`;
/*!40000 ALTER TABLE `ticket_assigned` DISABLE KEYS */;
INSERT INTO `ticket_assigned` (`ID`, `TicketID`, `UserID`) VALUES
	(1, 4, 2);
/*!40000 ALTER TABLE `ticket_assigned` ENABLE KEYS */;

-- Dumping structure for table svend.ticket_categories
DROP TABLE IF EXISTS `ticket_categories`;
CREATE TABLE IF NOT EXISTS `ticket_categories` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Navn` varchar(500) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.ticket_categories: ~4 rows (approximately)
DELETE FROM `ticket_categories`;
/*!40000 ALTER TABLE `ticket_categories` DISABLE KEYS */;
INSERT INTO `ticket_categories` (`ID`, `Navn`) VALUES
	(1, 'Salgs spørgsmål'),
	(2, 'Feature request'),
	(3, 'Bug report'),
	(4, 'Teknisk problem');
/*!40000 ALTER TABLE `ticket_categories` ENABLE KEYS */;

-- Dumping structure for table svend.ticket_replies
DROP TABLE IF EXISTS `ticket_replies`;
CREATE TABLE IF NOT EXISTS `ticket_replies` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `TicketID` int(11) NOT NULL,
  `Body` varchar(7500) COLLATE utf8_bin NOT NULL,
  `CreationDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `TickerReplyTicketFK` (`TicketID`),
  KEY `TicketReplyUserFK` (`UserID`),
  CONSTRAINT `TickerReplyTicketFK` FOREIGN KEY (`TicketID`) REFERENCES `tickets` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `TicketReplyUserFK` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.ticket_replies: ~0 rows (approximately)
DELETE FROM `ticket_replies`;
/*!40000 ALTER TABLE `ticket_replies` DISABLE KEYS */;
INSERT INTO `ticket_replies` (`ID`, `UserID`, `TicketID`, `Body`, `CreationDate`) VALUES
	(1, 1, 4, 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s', '2020-06-14 16:05:57');
/*!40000 ALTER TABLE `ticket_replies` ENABLE KEYS */;

-- Dumping structure for table svend.ticket_status
DROP TABLE IF EXISTS `ticket_status`;
CREATE TABLE IF NOT EXISTS `ticket_status` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Navn` varchar(500) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.ticket_status: ~2 rows (approximately)
DELETE FROM `ticket_status`;
/*!40000 ALTER TABLE `ticket_status` DISABLE KEYS */;
INSERT INTO `ticket_status` (`ID`, `Navn`) VALUES
	(1, 'Åben'),
	(2, 'Lukket');
/*!40000 ALTER TABLE `ticket_status` ENABLE KEYS */;

-- Dumping structure for table svend.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Fornavn` varchar(255) COLLATE utf8_bin NOT NULL,
  `Efternavn` varchar(255) COLLATE utf8_bin NOT NULL,
  `Email` varchar(255) COLLATE utf8_bin NOT NULL,
  `Brugernavn` varchar(255) COLLATE utf8_bin NOT NULL,
  `Password` varchar(255) COLLATE utf8_bin NOT NULL,
  `Salt` varchar(255) COLLATE utf8_bin NOT NULL,
  `Rolle` int(11) NOT NULL DEFAULT '3',
  `Specialitet` int(11) NOT NULL DEFAULT '3',
  `AuthToken` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Brugernavn` (`Brugernavn`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Password` (`Password`),
  UNIQUE KEY `Salt` (`Salt`),
  KEY `UsersRoleFK` (`Rolle`),
  KEY `UsersSpecFK` (`Specialitet`),
  CONSTRAINT `UsersRoleFK` FOREIGN KEY (`Rolle`) REFERENCES `user_roles` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `UsersSpecFK` FOREIGN KEY (`Specialitet`) REFERENCES `user_specialties` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.users: ~3 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `Fornavn`, `Efternavn`, `Email`, `Brugernavn`, `Password`, `Salt`, `Rolle`, `Specialitet`, `AuthToken`) VALUES
	(1, 'Anton', 'Frederiksen', 'anton@test.com', 'anton', 'f65ec5f2cab18c84b87d3a7f23ea04bcffb246318078a7c41a98807572e15fe7b03fa86761e780fc34fee16f3b0464d443c2e062d13b6648e44a9553e7223c5b', 'WZ081RxDaediGSBLHD05BS/3&poCm4os', 1, 1, 'fbaefcfaa84ba3f0e6677c5f34b8ca54fea2f126'),
	(2, 'Svend', 'Thomassen', 'svend@test.com', 'svend', '885881f78ffd1ecc09e3ad5834dafc4b3671be8430d57d618e068218679875e18247c8ec352269b406d6fce905b216daa94d41755e952d88307096be37739976', 'YkbsOCblDnI#tuG6@q8iKl9V3nLXv9HO', 2, 2, 'befca74fe034b6548f553e2e35b2844325f93d28'),
	(3, 'Problembarn', '#1', 'problembarn@whoknows.com', 'problembarn', '0824b289b91b4faaec803746daefbeff0748b7efdad276442aea6220f38e2ccb8d6050ae055c5f4347378e6688f0cac1c2e304e65539cefe6ce37b0f3a06f5a1', '&QzGUO7GSqxw4Ho8@8mzLdUvtPDZzdFs', 3, 3, '98ccaa4cc698f6f0e6ac0282df1db0f902b33ab2');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

-- Dumping structure for table svend.user_roles
DROP TABLE IF EXISTS `user_roles`;
CREATE TABLE IF NOT EXISTS `user_roles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Navn` varchar(500) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.user_roles: ~2 rows (approximately)
DELETE FROM `user_roles`;
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
INSERT INTO `user_roles` (`ID`, `Navn`) VALUES
	(1, 'Administrator'),
	(2, 'Supporter'),
	(3, 'Kunde');
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;

-- Dumping structure for table svend.user_specialties
DROP TABLE IF EXISTS `user_specialties`;
CREATE TABLE IF NOT EXISTS `user_specialties` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Navn` varchar(500) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Dumping data for table svend.user_specialties: ~2 rows (approximately)
DELETE FROM `user_specialties`;
/*!40000 ALTER TABLE `user_specialties` DISABLE KEYS */;
INSERT INTO `user_specialties` (`ID`, `Navn`) VALUES
	(1, 'Tekniker'),
	(2, 'Salg'),
	(3, 'Ingen');
/*!40000 ALTER TABLE `user_specialties` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
