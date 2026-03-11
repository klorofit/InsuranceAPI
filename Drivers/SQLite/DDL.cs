namespace MyAPP.Driver;

public class DDL
{
    public static readonly string[] Defenition = {
        @"PRAGMA foreign_keys = ON;",

        @"CREATE TABLE IF NOT EXISTS Users (
            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
            Email TEXT NOT NULL,
            PasswordHash TEXT NOT NULL,
            Role TEXT NOT NULL,
            IsActive INTEGER NOT NULL DEFAULT 1,
            CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            UpdatedAt DATETIME
        );",
        @"CREATE UNIQUE INDEX IF NOT EXISTS UX_Users_Email
         ON Users (Email);",

        @"CREATE TABLE IF NOT EXISTS Agents (
            AgentsID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Phone TEXT,
            Email TEXT,
            HireDate TEXT
        );",

        @"CREATE TABLE IF NOT EXISTS Clients (
            ClientsID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            BirthDate TEXT,
            Passport TEXT,
            Phone TEXT,
            Email TEXT,
            Address TEXT
        );",

        @"CREATE TABLE IF NOT EXISTS InsuranceTypes (
            InsuranceTypesID INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL
        );",

        @"CREATE TABLE IF NOT EXISTS Vehicles (
            VehiclesID INTEGER PRIMARY KEY AUTOINCREMENT,
            Vin TEXT,
            Make TEXT,
            Model TEXT,
            Year INTEGER
        );",
        @"CREATE UNIQUE INDEX IF NOT EXISTS UX_Vehicles_Vin
         ON Vehicles (Vin);",

        @"CREATE TABLE IF NOT EXISTS RefreshTokens (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            Token TEXT NOT NULL,
            CreatedAt DATETIME NOT NULL,
            ExpiresAt DATETIME NOT NULL,
            IsRevoked INTEGER NOT NULL DEFAULT 0,
            FOREIGN KEY (UserId) REFERENCES Users(UserID) ON DELETE CASCADE
        );",


        @"CREATE TABLE IF NOT EXISTS Insurance (
            InsuranceID INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Description TEXT NOT NULL,
            Terms TEXT NOT NULL,
            Tariff REAL NOT NULL,
            InsuranceTypesID INTEGER NOT NULL,
            FOREIGN KEY (InsuranceTypesID) REFERENCES InsuranceTypes(InsuranceTypesID)
        );",
        @"CREATE INDEX IF NOT EXISTS idx_insurance_type
         ON Insurance (InsuranceTypesID);",

        @"CREATE TABLE IF NOT EXISTS Policies (
            PoliciesID INTEGER PRIMARY KEY AUTOINCREMENT,
            PolicyNumber TEXT NOT NULL,
            ClientsID INTEGER,
            InsuranceID INTEGER,
            AgentsID INTEGER,
            StartDate TEXT NOT NULL,
            EndDate TEXT,
            SumInsured REAL,
            Premium REAL,
            Status TEXT DEFAULT 'active',
            FOREIGN KEY (ClientsID) REFERENCES Clients(ClientsID) ON DELETE CASCADE,
            FOREIGN KEY (InsuranceID) REFERENCES Insurance(InsuranceID),
            FOREIGN KEY (AgentsID) REFERENCES Agents(AgentsID)
        );",
        @"CREATE UNIQUE INDEX IF NOT EXISTS UX_Policies_Number
         ON Policies (Policy_number);",
        @"CREATE INDEX IF NOT EXISTS idx_policies_clients
         ON Policies (ClientsID);",
        @"CREATE INDEX IF NOT EXISTS idx_policies_insurance
         ON Policies (InsuranceID);",
        @"CREATE INDEX IF NOT EXISTS idx_policies_agents
         ON Policies (AgentsID);",

        @"CREATE TABLE IF NOT EXISTS Payments (
            PaymentsID INTEGER PRIMARY KEY AUTOINCREMENT,
            PoliciesID INTEGER NOT NULL,
            PaymentDate TEXT NOT NULL,
            Amount REAL NOT NULL,
            Method TEXT,
            Reference TEXT,
            FOREIGN KEY (PoliciesID) REFERENCES Policies(PoliciesID)
        );",
        @"CREATE INDEX IF NOT EXISTS idx_payments_policy
         ON Payments (PoliciesID);",


        @"CREATE TABLE IF NOT EXISTS AgreementsVehicles (
            AgreementsVehiclesID INTEGER PRIMARY KEY AUTOINCREMENT,
            VehiclesID INTEGER NOT NULL,
            PoliciesID INTEGER NOT NULL,
            FOREIGN KEY (PoliciesID) REFERENCES Policies(PoliciesID),
            FOREIGN KEY (VehiclesID) REFERENCES Vehicles(VehiclesID)
        );",
        @"CREATE INDEX IF NOT EXISTS idx_agreements_vehicles_policies
         ON AgreementsVehicles (PoliciesID);",
        @"CREATE INDEX IF NOT EXISTS idx_agreements_vehicles_vehicles
         ON AgreementsVehicles (VehiclesID);",
    };
}