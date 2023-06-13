
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/13/2023 09:31:15
-- Generated from EDMX file: C:\Users\Angelo\Desktop\sofDES\PasswordRepositorySample-master\WebApplication1\JAPKREPOSITORY.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [JAPKDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TBL_USER_DETAILS_TBL_LOGIN]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TBL_USER_DETAILS] DROP CONSTRAINT [FK_TBL_USER_DETAILS_TBL_LOGIN];
GO
IF OBJECT_ID(N'[dbo].[FK_TBL_USER_PASSWORDS_TBL_USER_PASSWORDS]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TBL_USER_PASSWORDS] DROP CONSTRAINT [FK_TBL_USER_PASSWORDS_TBL_USER_PASSWORDS];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TBL_LOGIN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_LOGIN];
GO
IF OBJECT_ID(N'[dbo].[TBL_USER_DETAILS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_USER_DETAILS];
GO
IF OBJECT_ID(N'[dbo].[TBL_USER_PASSWORDS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_USER_PASSWORDS];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TBL_LOGIN'
CREATE TABLE [dbo].[TBL_LOGIN] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [USERNAME] varchar(12)  NULL,
    [PASSWORD] varchar(255)  NULL,
    [DATE_CREATED] datetime  NULL
);
GO

-- Creating table 'TBL_USER_DETAILS'
CREATE TABLE [dbo].[TBL_USER_DETAILS] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NAME] varchar(20)  NULL,
    [EMAIL] varchar(30)  NULL,
    [UID] int  NULL,
    [DATE_CREATED] datetime  NOT NULL,
    [CONTACT] int  NULL,
    [BIRTHDAY] varchar(50)  NULL
);
GO

-- Creating table 'TBL_USER_PASSWORDS'
CREATE TABLE [dbo].[TBL_USER_PASSWORDS] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SITE_NAME] varchar(50)  NOT NULL,
    [SITE_PASSWORD] varchar(50)  NOT NULL,
    [USER_ID] int  NOT NULL,
    [DATE_CREATED] datetime  NOT NULL,
    [DATE_MODIFIED] datetime  NOT NULL,
    [IS_DELETED] bit  NOT NULL,
    [USER_NAME] varchar(50)  NULL,
    [EMAIL] varchar(50)  NULL,
    [CONTACT_NUMBER] int  NULL,
    [DEC_PASSWORD] varchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'TBL_LOGIN'
ALTER TABLE [dbo].[TBL_LOGIN]
ADD CONSTRAINT [PK_TBL_LOGIN]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TBL_USER_DETAILS'
ALTER TABLE [dbo].[TBL_USER_DETAILS]
ADD CONSTRAINT [PK_TBL_USER_DETAILS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TBL_USER_PASSWORDS'
ALTER TABLE [dbo].[TBL_USER_PASSWORDS]
ADD CONSTRAINT [PK_TBL_USER_PASSWORDS]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UID] in table 'TBL_USER_DETAILS'
ALTER TABLE [dbo].[TBL_USER_DETAILS]
ADD CONSTRAINT [FK_TBL_USER_DETAILS_TBL_LOGIN]
    FOREIGN KEY ([UID])
    REFERENCES [dbo].[TBL_LOGIN]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TBL_USER_DETAILS_TBL_LOGIN'
CREATE INDEX [IX_FK_TBL_USER_DETAILS_TBL_LOGIN]
ON [dbo].[TBL_USER_DETAILS]
    ([UID]);
GO

-- Creating foreign key on [USER_ID] in table 'TBL_USER_PASSWORDS'
ALTER TABLE [dbo].[TBL_USER_PASSWORDS]
ADD CONSTRAINT [FK_TBL_USER_PASSWORDS_TBL_USER_PASSWORDS]
    FOREIGN KEY ([USER_ID])
    REFERENCES [dbo].[TBL_LOGIN]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TBL_USER_PASSWORDS_TBL_USER_PASSWORDS'
CREATE INDEX [IX_FK_TBL_USER_PASSWORDS_TBL_USER_PASSWORDS]
ON [dbo].[TBL_USER_PASSWORDS]
    ([USER_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------