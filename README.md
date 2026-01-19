# StudentGrievancePortal

#Connection with Code
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=PIYUSH\\SQLEXPRESS01;Database=GrievanceERP;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }
}

# SQL Queries to run for the DB
CREATE DATABASE GrievanceERP;
GO

USE GrievanceERP;
GO

-- 1. Departments (Academic, Admin, Hostel, etc.)
CREATE TABLE Departments (
    DeptId INT PRIMARY KEY IDENTITY(1,1),
    DeptName NVARCHAR(100) NOT NULL
);

-- 2. Roles
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL -- 'Student', 'Coordinator', 'Admin'
);

-- 3. Users (Integrated with ERP logic)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    ERP_Id NVARCHAR(50) UNIQUE NOT NULL, -- The Student's Roll No or Employee ID
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId INT FOREIGN KEY REFERENCES Roles(RoleId),
    DeptId INT FOREIGN KEY REFERENCES Departments(DeptId),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 4. Grievances (The Core Table)
CREATE TABLE Grievances (
    GrievanceId INT PRIMARY KEY IDENTITY(1,1),
    TicketNumber AS ('GRV-' + CAST(GrievanceId AS NVARCHAR(10))), -- Auto-generated Ticket ID
    Subject NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    
    -- Status Management
    Status NVARCHAR(50) DEFAULT 'Submitted', -- 'Submitted', 'Under Review', 'Resolved', 'Closed'
    Priority NVARCHAR(20) DEFAULT 'Medium', -- 'Low', 'Medium', 'High'
    
    -- Accountability
    StudentId INT FOREIGN KEY REFERENCES Users(UserId), -- System knows who, but CC won't see this
    AssignedDeptId INT FOREIGN KEY REFERENCES Departments(DeptId), -- Routes to correct CC
    
    -- Tracking
    ResolutionDetails NVARCHAR(MAX) NULL, -- Filled by CC when resolving
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- 5. Seed Initial Data
INSERT INTO Roles (RoleName) VALUES ('Student'), ('Coordinator'), ('Admin');
INSERT INTO Departments (DeptName) VALUES ('Computer Science'), ('Mechanical'), ('Civil'), ('Accounts');

-- Sample Users (Passwords are plain text for setup, but should be hashed in code)
INSERT INTO Users (ERP_Id, FullName, Email, PasswordHash, RoleId, DeptId) 
VALUES ('STU001', 'John Doe', 'student@college.edu', '123', 1, 1),
       ('CC001', 'Prof. Smith', 'coordinator@college.edu', '123', 2, 1);


Select * from Grievances;
Select * from Users;

INSERT INTO Users (ERP_Id, FullName, Email, PasswordHash, RoleId, DeptId) 
VALUES ('CC003', 'Dr.Saumya', 'coordinator3@college.edu', '123', 2, 3);




