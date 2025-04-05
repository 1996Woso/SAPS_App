IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE TABLE [Case_Managers] (
        [CaseManagerNo] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Surname] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [CaseManagerId] nvarchar(max) NULL,
        CONSTRAINT [PK_Case_Managers] PRIMARY KEY ([CaseManagerNo])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE TABLE [Offences] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Offences] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE TABLE [PoliceStations] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_PoliceStations] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE TABLE [Suspects] (
        [SuspectNumber] int NOT NULL IDENTITY,
        [SuspectId] nvarchar(13) NOT NULL,
        [FirstName] nvarchar(20) NOT NULL,
        [LastName] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Suspects] PRIMARY KEY ([SuspectNumber])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE TABLE [CriminalRecords] (
        [Id] int NOT NULL IDENTITY,
        [OffenceCommited] nvarchar(max) NOT NULL,
        [Sentence] nvarchar(3) NOT NULL,
        [IssuedAt] nvarchar(max) NOT NULL,
        [IssuedBy] nvarchar(20) NOT NULL,
        [IssueDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NULL,
        [SuspectNumber] int NOT NULL,
        [IssuerId] nvarchar(max) NULL,
        [CaseManagerNo] int NOT NULL,
        [CaseManagerId] nvarchar(max) NULL,
        [CaseManagerName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_CriminalRecords] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CriminalRecords_Case_Managers_CaseManagerNo] FOREIGN KEY ([CaseManagerNo]) REFERENCES [Case_Managers] ([CaseManagerNo]) ON DELETE CASCADE,
        CONSTRAINT [FK_CriminalRecords_Suspects_SuspectNumber] FOREIGN KEY ([SuspectNumber]) REFERENCES [Suspects] ([SuspectNumber]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE INDEX [IX_CriminalRecords_CaseManagerNo] ON [CriminalRecords] ([CaseManagerNo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    CREATE INDEX [IX_CriminalRecords_SuspectNumber] ON [CriminalRecords] ([SuspectNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250215133012_CreateTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250215133012_CreateTables', N'8.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250217075303_ExtendCaseManager'
)
BEGIN
    ALTER TABLE [Case_Managers] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250217075303_ExtendCaseManager'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250217075303_ExtendCaseManager', N'8.0.1');
END;
GO

COMMIT;
GO

