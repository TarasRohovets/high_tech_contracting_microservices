create table Users(
    Id int AUTO_INCREMENT PRIMARY KEY,
    Name NVARCHAR(50) NULL,
    Surname NVARCHAR(50) NULL,
    Email NVARCHAR(30) NULL,
    City NVARCHAR(30) NULL,
    Country NVARCHAR(30) NULL,
    Address NVARCHAR(300) NULL,
    TaxNumber NVARCHAR(30) NULL,
    Passwd NVARCHAR(30) NULL,
    ImageName NVARCHAR(100) NULL,
    ImagePath NVARCHAR(150) NULL,
    Type int NULL,
    Active bit Null,
    EmailConfirmed bit Null,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL
);
create table PropertyType(
    Id int AUTO_INCREMENT PRIMARY KEY,
    Type NVARCHAR(30) NOT NULL,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL
);
create table OperationType(
    Id int AUTO_INCREMENT PRIMARY KEY,
    Type NVARCHAR(30) NOT NULL,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL
);
create table Property(
    Id int AUTO_INCREMENT PRIMARY KEY,
    UserId int NOT NULL,
    PropertyTypeId int NOT NULL,
    OperationTypeId int NOT NULL,
    Price int NOT NULL,
    NetAream2 int,
    PriceNetAream2 int,
    GrossAream2 int,
    Typology NVARCHAR(30),
    Floor int,
    YearOfConstruction int,
    NumberOfBathrooms int,
    EnerergyCertificate NVARCHAR(30),
    Country NVARCHAR(30),
    City NVARCHAR(30),
    Address NVARCHAR(30),
    Description TEXT,
    Latitude DOUBLE,
    Longitude DOUBLE,
    ActiveForUser bit NULL,
    AdminApproved bit NULL,
    State int NULL,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL,
FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
FOREIGN KEY (PropertyTypeId) REFERENCES PropertyType(Id) ON DELETE CASCADE,
FOREIGN KEY (OperationTypeId) REFERENCES OperationType(Id) ON DELETE CASCADE
);

create table Images(
    Id int AUTO_INCREMENT PRIMARY KEY,
    PropertyId int NOT NULL, 
    ImageName NVARCHAR(100) NOT NULL,
    ImageUrl NVARCHAR(150) NOT NULL,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL,
FOREIGN KEY (PropertyId) REFERENCES Property(Id) ON DELETE CASCADE
);
create table Characteristics(
    Id int AUTO_INCREMENT PRIMARY KEY,
    PropertyId int, 
    Characteristic NVARCHAR(30),
    Active Bit,
    CreationDate datetime NULL,
    UpdateDate datetime NULL,
    Deleted bit NULL,
FOREIGN KEY (PropertyId) REFERENCES Property(Id) ON DELETE CASCADE
);
INSERT INTO `properties`.`PropertyType` (`Type`, `CreationDate`, `UpdateDate`) Values("House", NOW(), NOW());
INSERT INTO `properties`.`PropertyType` (`Type`, `CreationDate`, `UpdateDate`) Values("Apartment", NOW(), NOW());
INSERT INTO `properties`.`PropertyType` (`Type`, `CreationDate`, `UpdateDate`) Values("Room", NOW(), NOW());

INSERT INTO `properties`.`OperationType` (`Type`, `CreationDate`, `UpdateDate`) Values("Buy", NOW(), NOW());
INSERT INTO `properties`.`OperationType` (`Type`, `CreationDate`, `UpdateDate`) Values("Rent", NOW(), NOW());
