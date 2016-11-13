-- CREATE DATABASE

USE master;
IF EXISTS(SELECT * FROM sys.databases WHERE name='RWP')
  DROP DATABASE [RWP];
GO

CREATE DATABASE [RWP];
GO

-- CREATE TABLES

USE [RWP];
GO

CREATE TABLE [dbo].[Doctor]
(
  [Id] int identity(1,1) not null,
  [FirstName] nvarchar(128) not null,
  [MiddleName] nvarchar(128) null,
  [LastName] nvarchar(128) not null,
  [Position] nvarchar(128) not null,
  [Print] varbinary(max) null,
  [Note] nvarchar(2048) null

  constraint [PK_Doctor] primary key ([Id]),

  constraint [UQ_Doctor_FullName] unique ([FirstName], [MiddleName], [LastName])
);
GO

CREATE TABLE [dbo].[Customer]
(
  [Id] int identity(1,1) not null,
  [Name] nvarchar(450) not null,
  [Address] nvarchar(1024) not null,
  [ResearchPlace] nvarchar(450) null,
  [ContactName] nvarchar(1024) null,
  [ContactEMail] nvarchar(1024) null,
  [Note] nvarchar(256) null

  constraint [PK_Customer] primary key ([Id]),

  constraint [UQ_Customer_Name] unique ([Name])
);

CREATE TABLE [dbo].[ResearchScope]
(
  [Id] int identity(1,1) not null,
  [Name] nvarchar(128) not null,
  [Order] int null,
  [IsSystem] bit not null default 0

  constraint [PK_ResearchScope] primary key ([Id]),

  constraint [UQ_ResearchScope_Name] unique ([Name])
);
GO

CREATE TABLE [dbo].[ScanRegime]
(
  [Id] int identity(1,1) not null,
  [Name] nvarchar(128) not null,
  [Order] int null,
  [IsSystem] bit not null default 0

  constraint [PK_ScanRegime] primary key ([Id]),

  constraint [UQ_ScanRegime_Name] unique ([Name])
);
GO

CREATE TABLE [dbo].[Attachment]
(
  [Id] int identity(1,1) not null,
  [Name] nvarchar(256) not null,
  [Type] nvarchar(256) not null,
  [Note] nvarchar(256) not null

  constraint [PK_Attachment] primary key ([Id])
);
GO

CREATE TABLE [dbo].[ResearchTemplate]
(
  [Id] int identity(1,1) not null,
  [Name] nvarchar(450) not null,
  [Content] nvarchar(max) null,
  [Order] int null,
  [IsSystem] bit not null default 0

  constraint [PK_ResearchTemplate] primary key ([Id]),

  constraint [UQ_ResearchTemplate_Name] unique ([Name])
);
GO

CREATE TABLE [dbo].[Patient]
(
  [Id] int identity(1,1) not null,
  [FirstName] nvarchar(128) not null,
  [MiddleName] nvarchar(128) null,
  [LastName] nvarchar(128) not null,
  [DOB] datetime not null,
  [Sex] bit not null,
  [Note] nvarchar(2048) null

  constraint [PK_Patient] primary key ([Id]),

  constraint [UQ_Patient_Personality] unique ([FirstName], [MiddleName], [LastName], [DOB], [Sex])
);

CREATE TABLE [dbo].[MedicalResearch]
(
  [Id] int identity(1,1) not null,
  [IdCustomer] int not null,
  [IdDoctor] int not null,
  [IdPatient] int not null,
  [IdResearchTemplate] int null,
  [ExaminationDate] datetime not null,
  [ResearchDate] datetime not null,
  [Number] nvarchar(128) not null,
  [SliceThickness] nvarchar(128) not null,
  [UseContrast] bit not null,
  [Dose] float not null,
  [Content] nvarchar(max) not null,
  [Conclusion] nvarchar(2048) not null

  constraint [PK_MedicalResearch] primary key ([Id]),
  constraint [FK_MedicalResearch_Customer] foreign key ([IdCustomer]) references [dbo].[Customer] ([Id]),
  constraint [FK_MedicalResearch_Patient] foreign key ([IdPatient]) references [dbo].[Patient] ([Id]),
  constraint [FK_MedicalResearch_Doctor] foreign key ([IdDoctor]) references [dbo].[Doctor] ([Id]),
  constraint [FK_MedicalResearch_ResearchTemplate] foreign key ([IdResearchTemplate]) references [dbo].[ResearchTemplate] ([Id]) on delete set null,

  constraint [UQ_MedicalResearch_Number] unique ([IdCustomer], [IdPatient], [IdDoctor], [Number])
);
GO

CREATE TABLE [dbo].[MedicalResearchScope]
(
  [IdMedicalResearch] int not null,
  [IdResearchScope] int not null

  constraint [PK_MedicalResearchScope] primary key ([IdMedicalResearch], [IdResearchScope]),
  constraint [FK_MedicalResearchScope_MedicalResearch] foreign key ([IdMedicalResearch]) references [dbo].[MedicalResearch] ([Id]),
  constraint [FK_MedicalResearchScope_ResearchScope] foreign key ([IdResearchScope]) references [dbo].[ResearchScope] ([Id])
);
GO

CREATE TABLE [dbo].[MedicalScanRegime]
(
  [IdMedicalResearch] int not null,
  [IdScanRegime] int not null

  constraint [PK_MedicalScanRegime] primary key ([IdMedicalResearch], [IdScanRegime]),
  constraint [FK_MedicalScanRegime_MedicalResearch] foreign key ([IdMedicalResearch]) references [dbo].[MedicalResearch] ([Id]),
  constraint [FK_MedicalScanRegime_ScanRegime] foreign key ([IdScanRegime]) references [dbo].[ScanRegime] ([Id])
);
GO

CREATE TABLE [dbo].[MedicalResearchAttachment]
(
  [IdMedicalResearch] int not null,
  [IdAttachment] int not null

  constraint [PK_MedicalResearchAttachment] primary key ([IdMedicalResearch], [IdAttachment]),
  constraint [FK_MedicalResearchAttachment_MedicalResearch] foreign key ([IdMedicalResearch]) references [dbo].[MedicalResearch] ([Id]),
  constraint [FK_MedicalResearchAttachment_Attachment] foreign key ([IdAttachment]) references [dbo].[Attachment] ([Id])
);
GO

CREATE TABLE [dbo].[ReportSettings]
(
  [Id] int identity(1,1) not null,
  [IdMedicalResearch] int not null,
  [Name] nvarchar(128) not null,
  [Type] nvarchar(128) not null,
  [Settings] nvarchar(max) null

  constraint [PK_ReportSettings] primary key ([Id]),
  constraint [FK_MedicalResearch_ReportSettings] foreign key ([IdMedicalResearch]) references [dbo].[MedicalResearch] ([Id]),

  constraint [UQ_ReportSettings] unique ([Name], [Type], [IdMedicalResearch])
);
GO

-- FILL DEFAULT DATA

INSERT INTO [dbo].[ResearchTemplate] ([Name], [Content], [Order], [IsSystem]) VALUES
('пустой', '', 1000000, 1);
GO

INSERT INTO [dbo].[ResearchScope] ([Name], [Order], [IsSystem]) VALUES
('Голова', 10, 1),
('Грудь', 20, 1),
('Живот', 30, 1),
('Таз', 40, 1),
('Околоносовые пазухи', 50, 1),
('Шея', 60, 1),
('Надпочечники', 70, 1);
GO

INSERT INTO [dbo].[ScanRegime] ([Name], [Order], [IsSystem]) VALUES
('Спираль', 10, 1),
('Последовательно', 20, 1);
GO
