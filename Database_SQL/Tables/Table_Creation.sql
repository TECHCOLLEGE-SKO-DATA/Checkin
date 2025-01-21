CREATE DATABASE CheckInSystem
GO

USE CheckInSystem
GO
CREATE TABLE employee(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    cardID char(11) UNIQUE,
    firstName varchar(50),
    middleName varchar(100),
    lastName varchar(50),
    isOffSite bit NOT NULL DEFAULT 0,
    offSiteUntil DATE,
);

USE CheckInSystem
GO
CREATE TABLE [group]
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    name varchar(50) NOT NULL,
    isvisible bit NOT NULL DEFAULT 0
);

USE CheckInSystem
GO
CREATE TABLE employeeGroup(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    employeeID INT NOT NULL,
    groupID INT NOT NULL,
    FOREIGN KEY (employeeID) REFERENCES employee(ID) ON DELETE CASCADE,
    FOREIGN KEY (groupID) REFERENCES [Group](ID) ON DELETE CASCADE,
);

USE CheckInSystem
GO
CREATE TABLE onSiteTime(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    employeeID INT NOT NULL,
    arrivalTime DATETIME NOT NULL,
    departureTime DATETIME,
    FOREIGN KEY (employeeID) REFERENCES employee(ID) ON DELETE CASCADE
);

USE CheckInSystem
GO
CREATE TABLE adminUser(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    username VARCHAR(20) NOT NULL UNIQUE,
    hashedPassword VARCHAR(60) NOT NULL,
);