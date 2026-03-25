namespace MyAPP.Driver;

public class DDL
{
    public static readonly string[] Defenition = {
    @"CREATE DATABASE `myapp` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;",
    @"use `myapp`;
    CREATE TABLE `Users` (
      `UserID` int(11) NOT NULL AUTO_INCREMENT,
      `Email` varchar(255) NOT NULL,
      `PasswordHash` varchar(255) NOT NULL,
      `Role` varchar(50) NOT NULL,
      `IsActive` tinyint(1) NOT NULL DEFAULT 1,
      `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
      `UpdatedAt` datetime DEFAULT NULL,
      PRIMARY KEY (`UserID`),
      UNIQUE KEY `UX_Users_Email` (`Email`)
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;",
    
    @"use `myapp`;
    CREATE TABLE IF NOT EXISTS `RefreshTokens` (
      `Id` int(11) NOT NULL AUTO_INCREMENT,
      `UserId` int(11) NOT NULL,
      `Token` varchar(255) NOT NULL,
      `CreatedAt` datetime NOT NULL,
      `ExpiresAt` datetime NOT NULL,
      `IsRevoked` tinyint(1) NOT NULL DEFAULT 0,
      PRIMARY KEY (`Id`),
      UNIQUE KEY `UX_RefreshTokens_Token` (`Token`),
      KEY `IX_RefreshTokens_UserId` (`UserId`),
      CONSTRAINT `refreshtokens_ibfk_1`
        FOREIGN KEY (`UserId`)
        REFERENCES `Users` (`UserID`)
        ON DELETE CASCADE
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;",

    @"use `myapp`;
    CREATE TABLE `Agents` (
      `AgentsID` int(11) NOT NULL AUTO_INCREMENT,
      `FirstName` varchar(100) NOT NULL,
      `LastName` varchar(100) NOT NULL,
      `Phone` varchar(30) DEFAULT NULL,
      `Email` varchar(100) DEFAULT NULL,
      `HireDate` date DEFAULT NULL,
      PRIMARY KEY (`AgentsID`)
    ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE `Clients` (
      `ClientsID` int(11) NOT NULL AUTO_INCREMENT,
      `FirstName` varchar(100) NOT NULL,
      `LastName` varchar(100) NOT NULL,
      `BirthDate` date DEFAULT NULL,
      `Passport` varchar(50) DEFAULT NULL,
      `Phone` varchar(30) DEFAULT NULL,
      `Email` varchar(100) DEFAULT NULL,
      PRIMARY KEY (`ClientsID`)
    ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE `InsuranceTypes` (
      `InsuranceTypesID` int(11) NOT NULL AUTO_INCREMENT,
      `Name` varchar(100) NOT NULL,
      PRIMARY KEY (`InsuranceTypesID`)
    ) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE IF NOT EXISTS `Insurance` (
      `InsuranceID` int(11) NOT NULL AUTO_INCREMENT,
      `Name` varchar(100) NOT NULL,
      `Description` varchar(255) NOT NULL,
      `Terms` varchar(255) NOT NULL,
      `Tariff` decimal(10,2) NOT NULL,
      `Insurance_typesID` int(11) NOT NULL,
      PRIMARY KEY (`InsuranceID`),
      KEY `insurance_insurance_types_FK` (`Insurance_typesID`),
      CONSTRAINT `insurance_insurance_types_FK` FOREIGN KEY (`Insurance_typesID`) 
      REFERENCES `InsuranceTypes` (`InsuranceTypesID`) -- Убрано подчеркивание в имени таблицы!
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;",

    @"use `myapp`;
    CREATE TABLE `Policies` (
      `PoliciesID` int(11) NOT NULL AUTO_INCREMENT,
      `PolicyNumber` varchar(50) NOT NULL,
      `ClientsID` int(11) DEFAULT NULL,
      `InsuranceID` int(11) DEFAULT NULL,
      `AgentsID` int(11) DEFAULT NULL,
      `StartDate` date NOT NULL,
      `EndDate` date DEFAULT NULL,
      `SumInsured` decimal(14,2) DEFAULT NULL,
      `Premium` decimal(12,2) DEFAULT NULL,
      `Status` varchar(50) DEFAULT 'active',
      PRIMARY KEY (`PoliciesID`),
      KEY `clients_id` (`ClientsID`),
      KEY `insurance_id` (`InsuranceID`),
      KEY `agents_id` (`AgentsID`),
      CONSTRAINT `policies_ibfk_1` FOREIGN KEY (`ClientsID`) REFERENCES `Clients` (`ClientsID`) ON DELETE CASCADE,
      CONSTRAINT `policies_ibfk_2` FOREIGN KEY (`InsuranceID`) REFERENCES `Insurance` (`InsuranceID`),
      CONSTRAINT `policies_ibfk_3` FOREIGN KEY (`AgentsID`) REFERENCES `Agents` (`AgentsID`)
    ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE `Vehicles` (
      `VehiclesID` int(11) NOT NULL AUTO_INCREMENT,
      `Vin` varchar(50) DEFAULT NULL,
      `Make` varchar(100) DEFAULT NULL,
      `Model` varchar(100) DEFAULT NULL,
      `Year` int(4) DEFAULT NULL,
      PRIMARY KEY (`VehiclesID`),
      UNIQUE KEY `vin` (`Vin`)
    ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE `Payments` (
      `PaymentsID` int(11) NOT NULL AUTO_INCREMENT,
      `PoliciesID` int(11) NOT NULL,
      `Payment_date` date NOT NULL,
      `Amount` decimal(12,2) NOT NULL,
      `Method` varchar(50) DEFAULT NULL,
      `Reference` varchar(100) DEFAULT NULL,
      PRIMARY KEY (`PaymentsID`),
      KEY `Policies_id` (`PoliciesID`),
      CONSTRAINT `Payments_ibfk_1` FOREIGN KEY (`PoliciesID`) REFERENCES `Policies` (`PoliciesID`)
    ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",

    @"use `myapp`;
    CREATE TABLE `AgreementsVehicles` (
      `AgreementsVehiclesID` int(11) NOT NULL,
      `VehiclesID` int(11) NOT NULL,
      `PoliciesID` int(11) NOT NULL,
      PRIMARY KEY (`AgreementsVehiclesID`),
      KEY `agrement_vehicles_policies_FK` (`PoliciesID`),
      KEY `agrement_vehicles_vehicles_FK` (`VehiclesID`),
      CONSTRAINT `agrement_vehicles_policies_FK` FOREIGN KEY (`PoliciesID`) REFERENCES `Policies` (`PoliciesID`),
      CONSTRAINT `agrement_vehicles_vehicles_FK` FOREIGN KEY (`VehiclesID`) REFERENCES `Vehicles` (`VehiclesID`)
    ) ENGINE=InnoDB DEFAULT CHARSET=utf16 COLLATE=utf16_general_ci;",
    };
}