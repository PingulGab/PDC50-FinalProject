-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 14, 2024 at 06:27 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `scheded`
--

-- --------------------------------------------------------

--
-- Table structure for table `attendance`
--

CREATE TABLE `attendance` (
  `ID` int(11) NOT NULL,
  `ClassID` int(11) DEFAULT NULL,
  `StudentID` int(11) DEFAULT NULL,
  `Date` date NOT NULL,
  `Status` enum('Present','Absent','Excused') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `attendance`
--

INSERT INTO `attendance` (`ID`, `ClassID`, `StudentID`, `Date`, `Status`) VALUES
(18, 32, 8, '2024-12-14', 'Present');

-- --------------------------------------------------------

--
-- Table structure for table `classes`
--

CREATE TABLE `classes` (
  `ID` int(11) NOT NULL,
  `Image` varchar(255) DEFAULT NULL,
  `Name` varchar(255) NOT NULL,
  `Acronym` varchar(50) DEFAULT NULL,
  `Days` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`Days`)),
  `StartTime` time DEFAULT NULL,
  `EndTime` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `classes`
--

INSERT INTO `classes` (`ID`, `Image`, `Name`, `Acronym`, `Days`, `StartTime`, `EndTime`) VALUES
(32, 'a_college_of_computer_studies_ccs_a.png?timestamp=638697770703048743', 'College of Computer Studies', 'CCS', '{\"Monday\":true,\"Tuesday\":true,\"Wednesday\":true,\"Thursday\":false,\"Friday\":false,\"Saturday\":false}', '10:00:00', '13:00:00'),
(33, 'a_tiktok_academy_foundation_taf_a.png?timestamp=638697770935465820', 'TikTok Academy Foundation', 'TAF', '{\"Monday\":true,\"Tuesday\":false,\"Wednesday\":true,\"Thursday\":false,\"Friday\":true,\"Saturday\":false}', '13:00:00', '15:00:00'),
(34, 'a_professional_domain_course_5_pdc50_a.png?timestamp=638697771271494778', 'Professional Domain Course 5', 'PDC50', '{\"Monday\":false,\"Tuesday\":true,\"Wednesday\":false,\"Thursday\":true,\"Friday\":false,\"Saturday\":true}', '16:00:00', '20:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `ID` int(11) NOT NULL,
  `Image` varchar(255) DEFAULT NULL,
  `Name` varchar(255) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `StudentID` varchar(255) DEFAULT NULL,
  `ContactNumber` varchar(20) DEFAULT NULL,
  `ClassID` int(11) DEFAULT NULL,
  `Birthdate` datetime DEFAULT NULL,
  `ElementaryEducation` varchar(255) DEFAULT NULL,
  `SecondaryEducation` varchar(255) DEFAULT NULL,
  `TertiaryEducation` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `students`
--

INSERT INTO `students` (`ID`, `Image`, `Name`, `Gender`, `StudentID`, `ContactNumber`, `ClassID`, `Birthdate`, `ElementaryEducation`, `SecondaryEducation`, `TertiaryEducation`) VALUES
(8, 'a_jan_pingul_nature_lover_21_0591_739_a.png?timestamp=638697792713974812', 'Jan Pingul Nature Lover', 'Male', '21 0591 739', '09123631', 32, '0000-00-00 00:00:00', '', '', ''),
(9, 'a_jai_tolentino_19_0231_444_a.png?timestamp=638697777240062458', 'Jai Tolentino', 'Male', '19 0231 444', '09123456789', 34, '2002-01-14 00:00:00', 'HFA', 'HFA', 'AUF'),
(10, 'a_edlan_perez_18_492_1246_a.png?timestamp=638697777818775061', 'Edlan Perez', 'Male', '18 492 1246', '09197488212', 33, '2002-12-13 00:00:00', 'Magalang Elementary School', 'Magalang High School', 'AUF'),
(11, 'a_anne_canlas_21_433_0245_a.png?timestamp=638697777711228643', 'Anne Canlas', 'Female', '21 433 0245', '09197488212', 33, '2003-05-02 00:00:00', 'Scholastica Elementary', 'Scholastica Highschool', 'AUF');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `attendance`
--
ALTER TABLE `attendance`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ClassID` (`ClassID`),
  ADD KEY `StudentID` (`StudentID`);

--
-- Indexes for table `classes`
--
ALTER TABLE `classes`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `students`
--
ALTER TABLE `students`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `ClassID` (`ClassID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `attendance`
--
ALTER TABLE `attendance`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `classes`
--
ALTER TABLE `classes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT for table `students`
--
ALTER TABLE `students`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `attendance`
--
ALTER TABLE `attendance`
  ADD CONSTRAINT `attendance_ibfk_1` FOREIGN KEY (`ClassID`) REFERENCES `classes` (`ID`),
  ADD CONSTRAINT `attendance_ibfk_2` FOREIGN KEY (`StudentID`) REFERENCES `students` (`ID`);

--
-- Constraints for table `students`
--
ALTER TABLE `students`
  ADD CONSTRAINT `students_ibfk_1` FOREIGN KEY (`ClassID`) REFERENCES `classes` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
