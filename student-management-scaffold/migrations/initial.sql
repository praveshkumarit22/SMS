-- Basic schema for Student Management feature (for quick manual apply)
CREATE DATABASE SchoolDB;
GO
USE SchoolDB;
GO

CREATE TABLE Classes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Sections (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClassId INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Section_Class FOREIGN KEY (ClassId) REFERENCES Classes(Id)
);

CREATE TABLE Students (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AdmissionNo NVARCHAR(50) NOT NULL UNIQUE,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    DateOfBirth DATE,
    ClassId INT,
    SectionId INT,
    RollNumber NVARCHAR(50),
    AcademicYear INT,
    GuardianName NVARCHAR(200),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);

CREATE TABLE StudentDocuments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    FileName NVARCHAR(260),
    FilePath NVARCHAR(1024),
    ContentType NVARCHAR(100),
    UploadedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Doc_Student FOREIGN KEY (StudentId) REFERENCES Students(Id)
);

-- Seed Classes and Sections
INSERT INTO Classes (Name) VALUES ('Class 1'), ('Class 2'), ('Class 3'), ('Class 4');
INSERT INTO Sections (ClassId, Name) VALUES (1, 'A'), (1, 'B'), (2, 'A'), (2, 'B'), (3, 'A');
GO
