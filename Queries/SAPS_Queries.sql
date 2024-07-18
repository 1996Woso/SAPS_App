USE [EviweKhohlombe]
GO

/****** Object:  Table [dbo].[Suspects]    Script Date: 2024/03/15 10:15:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--Suspects
CREATE TABLE [dbo].[Suspects1](
	[SuspectNumber] [int] IDENTITY(1,1) NOT NULL,
	[SuspectId] [nvarchar](13) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Suspects1] PRIMARY KEY CLUSTERED 
(
	[SuspectNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Case Managers
CREATE TABLE [dbo].[Case_Managers1](
	[ManagerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Surname] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Case_Managers1] PRIMARY KEY CLUSTERED 
(
	[ManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--Criminal Records
CREATE TABLE [dbo].[CriminalRecords](
	[Id] [int] IDENTITY(1,1) Not Null,
	[OffenceCommited] [nvarchar](max) NOT NULL,
	[Sentence] [nvarchar](3) NOT NULL,
	[IssuedAt] [nvarchar](20) NOT NULL,
	[IssuedBy] [nvarchar](20) NOT NULL,
	[IssueDate] [datetime2](7) NOT NULL,
	[SuspectNumber] [int] NOT NULL,
	[CaseManagerNo] [int] Not Null,
	[CaseManagerId] NVARCHAR(50) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[CaseManagerName] [nvarchar](max) NOT NULL,
CONSTRAINT [PK_CriminalRecords] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET IDENTITY_INSERT CriminalRecords ON

ALTER TABLE [dbo].[CriminalRecords]
ALTER COLUMN [CaseManagerId] NVARCHAR(50) NOT NULL;

ALTER TABLE [dbo].[Case_Managers]
ALTER COLUMN [CaseManagerId] NVARCHAR(50) NOT NULL;

ALTER TABLE [dbo].[Case_Managers]
ADD CONSTRAINT [UQ_Case_Managers_CaseManagerId] UNIQUE ([CaseManagerId]);



---- Add foreign key constraint for CaseManagerId
--ALTER TABLE [dbo].[CriminalRecords]  WITH CHECK ADD  CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerId] FOREIGN KEY([CaseManagerId])
--REFERENCES [dbo].[Case_Managers] ([CaseManagerId])
--ON DELETE CASCADE;
--GO

--ALTER TABLE [dbo].[CriminalRecords] CHECK CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerId];
--GO

-- Add foreign key constraint for CaseManagerNo
ALTER TABLE [dbo].[CriminalRecords]  WITH CHECK ADD  CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerNo] FOREIGN KEY([CaseManagerNo])
REFERENCES [dbo].[Case_Managers] ([CaseManagerNo])
ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[CriminalRecords] CHECK CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerNo];
GO

-- Add foreign key constraint for SuspectNumber
ALTER TABLE [dbo].[CriminalRecords]  WITH CHECK ADD  CONSTRAINT [FK_CriminalRecords_Suspects_SuspectNumber] FOREIGN KEY([SuspectNumber])
REFERENCES [dbo].[Suspects] ([SuspectNumber])
ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[CriminalRecords] CHECK CONSTRAINT [FK_CriminalRecords_Suspects_SuspectNumber];
GO

-- Create the CriminalRecords table with all constraints
CREATE TABLE [dbo].[CriminalRecords] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OffenceCommited] NVARCHAR(MAX) NOT NULL,
    [Sentence] NVARCHAR(3) NOT NULL,
    [IssuedAt] NVARCHAR(20) NOT NULL,
    [IssuedBy] NVARCHAR(20) NOT NULL,
    [IssueDate] DATETIME2(7) NOT NULL,
    [SuspectNumber] INT NOT NULL,
    [CaseManagerNo] INT NOT NULL,
    [CaseManagerId] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(MAX) NOT NULL,
    [CaseManagerName] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_CriminalRecords] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerNo] FOREIGN KEY ([CaseManagerNo])
        REFERENCES [dbo].[Case_Managers] ([CaseManagerNo])
        ON DELETE CASCADE,
    CONSTRAINT [FK_CriminalRecords_Suspects_SuspectNumber] FOREIGN KEY ([SuspectNumber])
        REFERENCES [dbo].[Suspects] ([SuspectNumber])
        ON DELETE CASCADE
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

