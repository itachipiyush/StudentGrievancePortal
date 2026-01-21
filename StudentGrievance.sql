CREATE DATABASE GrievanceERP;

USE GrievanceERP;

CREATE TABLE Departments ( 
		DeptId INT PRIMARY KEY IDENTITY(1,1), 
		DeptName NVARCHAR(100) NOT NULL 
	);

CREATE TABLE Roles ( 
		RoleId INT PRIMARY KEY IDENTITY(1,1), 
		RoleName NVARCHAR(50) NOT NULL );

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    ERP_Id NVARCHAR(50) NOT NULL UNIQUE,       
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId INT,
    DeptId INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    CONSTRAINT FK_Users_Departments FOREIGN KEY (DeptId) REFERENCES Departments(DeptId)
);

CREATE TABLE Grievances (
    GrievanceId INT IDENTITY(1,1) PRIMARY KEY,

    TicketNumber AS ('GRV-' + CAST(GrievanceId AS NVARCHAR(10))), -- Computed column

    Subject NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,

    Status NVARCHAR(50) DEFAULT 'Submitted',   -- Submitted, Under Review, Resolved, Closed
    Priority NVARCHAR(20) DEFAULT 'Medium',    -- Low, Medium, High

    StudentId INT,
    AssignedDeptId INT,

    ResolutionDetails NVARCHAR(MAX) NULL,

    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Grievances_Users FOREIGN KEY (StudentId) REFERENCES Users(UserId),
    CONSTRAINT FK_Grievances_Departments FOREIGN KEY (AssignedDeptId) REFERENCES Departments(DeptId)
);



INSERT INTO Roles (RoleName)
VALUES ('Student'), ('Coordinator'), ('Admin');

INSERT INTO Departments (DeptName)
VALUES ('Computer Science'), ('Mechanical'), ('Civil'), ('Accounts');

INSERT INTO Users (ERP_Id, FullName, Email, PasswordHash, RoleId, DeptId)
VALUES
('STU001', 'John Doe', 'student@college.edu', '123', 1, 1),
('CC001', 'Prof. Smith', 'coordinator@college.edu', '123', 2, 1);


INSERT INTO Users (ERP_Id, FullName, Email, PasswordHash, RoleId, DeptId)
VALUES
('CC003', 'Dr.Saumya', 'coordinator3@college.edu', '123', 2, 3);


SELECT * FROM Grievances;
SELECT * FROM Departments;

USE GrievanceERP;
GO
CREATE USER [MAYANK\4703m] FOR LOGIN [MAYANK\4703m];
ALTER ROLE db_owner ADD MEMBER [MAYANK\4703m];

ALTER AUTHORIZATION ON DATABASE::GrievanceERP TO [MAYANK\4703m];

SELECT name, suser_sname(owner_sid) AS Owner
FROM sys.databases
WHERE name = 'GrievanceERP';

SELECT * FROM Users;

drop database GrievanceERP;

use master;