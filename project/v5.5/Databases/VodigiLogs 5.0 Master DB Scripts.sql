//******************************************************************************
    Vodigi - Open Source Interactive Digital Signage

    Copyright (C) 2005-2012  JMC Publications, LLC

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*******************************************************************************//

USE [master]
GO
/****** Object:  Database [VodigiLogs]    Script Date: 03/20/2012 09:00:08 ******/
CREATE DATABASE [VodigiLogs] ON  PRIMARY 
( NAME = N'VodigiLogs', FILENAME = N'C:\Vodigi\Databases\VodigiLogs.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'VodigiLogs_log', FILENAME = N'C:\Vodigi\Databases\VodigiLogs_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [VodigiLogs] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VodigiLogs].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VodigiLogs] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [VodigiLogs] SET ANSI_NULLS OFF
GO
ALTER DATABASE [VodigiLogs] SET ANSI_PADDING OFF
GO
ALTER DATABASE [VodigiLogs] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [VodigiLogs] SET ARITHABORT OFF
GO
ALTER DATABASE [VodigiLogs] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [VodigiLogs] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [VodigiLogs] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [VodigiLogs] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [VodigiLogs] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [VodigiLogs] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [VodigiLogs] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [VodigiLogs] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [VodigiLogs] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [VodigiLogs] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [VodigiLogs] SET  DISABLE_BROKER
GO
ALTER DATABASE [VodigiLogs] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [VodigiLogs] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [VodigiLogs] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [VodigiLogs] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [VodigiLogs] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [VodigiLogs] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [VodigiLogs] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [VodigiLogs] SET  READ_WRITE
GO
ALTER DATABASE [VodigiLogs] SET RECOVERY FULL
GO
ALTER DATABASE [VodigiLogs] SET  MULTI_USER
GO
ALTER DATABASE [VodigiLogs] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [VodigiLogs] SET DB_CHAINING OFF
GO
USE [VodigiLogs]
GO
/****** Object:  Table [dbo].[PlayerScreenLog]    Script Date: 03/20/2012 09:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerScreenLog](
	[PlayerScreenLogID] [int] IDENTITY(1000000,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[PlayerID] [int] NOT NULL,
	[PlayerName] [nvarchar](128) NOT NULL,
	[ScreenID] [int] NOT NULL,
	[ScreenName] [nvarchar](128) NOT NULL,
	[DisplayDateTime] [datetime] NOT NULL,
	[CloseDateTime] [datetime] NOT NULL,
	[ScreenDetails] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PlayerScreenLog] PRIMARY KEY CLUSTERED 
(
	[PlayerScreenLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenLog_AccountID] ON [dbo].[PlayerScreenLog] 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenLog_DisplayDateTime] ON [dbo].[PlayerScreenLog] 
(
	[DisplayDateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenLog_PlayerID] ON [dbo].[PlayerScreenLog] 
(
	[PlayerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenLog_ScreenID] ON [dbo].[PlayerScreenLog] 
(
	[ScreenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerScreenContentLog]    Script Date: 03/20/2012 09:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerScreenContentLog](
	[PlayerScreenContentLogID] [int] IDENTITY(1000000,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[PlayerID] [int] NOT NULL,
	[PlayerName] [nvarchar](128) NOT NULL,
	[ScreenID] [int] NOT NULL,
	[ScreenName] [nvarchar](128) NOT NULL,
	[ScreenContentID] [int] NOT NULL,
	[ScreenContentName] [nvarchar](64) NOT NULL,
	[ScreenContentTypeID] [int] NOT NULL,
	[ScreenContentTypeName] [nvarchar](64) NOT NULL,
	[DisplayDateTime] [datetime] NOT NULL,
	[CloseDateTime] [datetime] NOT NULL,
	[ContentDetails] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PlayerScreenContentLog] PRIMARY KEY CLUSTERED 
(
	[PlayerScreenContentLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_AccountID] ON [dbo].[PlayerScreenContentLog] 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_DisplayDateTime] ON [dbo].[PlayerScreenContentLog] 
(
	[DisplayDateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_PlayerID] ON [dbo].[PlayerScreenContentLog] 
(
	[PlayerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_ScreenContentID] ON [dbo].[PlayerScreenContentLog] 
(
	[ScreenContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_ScreenContentTypeID] ON [dbo].[PlayerScreenContentLog] 
(
	[ScreenContentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PlayerScreenContentLog_ScreenID] ON [dbo].[PlayerScreenContentLog] 
(
	[ScreenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginLog]    Script Date: 03/20/2012 09:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginLog](
	[LoginLogID] [int] IDENTITY(1000000,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[LoginDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED 
(
	[LoginLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LoginLog_AccountID] ON [dbo].[LoginLog] 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LoginLog_LoginDateTime] ON [dbo].[LoginLog] 
(
	[LoginDateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LoginLog_UserID] ON [dbo].[LoginLog] 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LoginLog_Username] ON [dbo].[LoginLog] 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 03/20/2012 09:00:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLog](
	[ActivityLogID] [int] IDENTITY(1000000,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[EntityType] [nvarchar](100) NOT NULL,
	[EntityAction] [nvarchar](100) NOT NULL,
	[ActivityDateTime] [datetime] NOT NULL,
	[ActivityDetails] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED 
(
	[ActivityLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_AccountID] ON [dbo].[ActivityLog] 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_ActivityDateTime] ON [dbo].[ActivityLog] 
(
	[ActivityDateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_EntityAction] ON [dbo].[ActivityLog] 
(
	[EntityAction] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_EntityType] ON [dbo].[ActivityLog] 
(
	[EntityType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_UserID] ON [dbo].[ActivityLog] 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ActivityLog_Username] ON [dbo].[ActivityLog] 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Default [DF_PlayerScreenLog_AccountID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_AccountID]  DEFAULT ((0)) FOR [AccountID]
GO
/****** Object:  Default [DF_PlayerScreenLog_PlayerID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_PlayerID]  DEFAULT ((0)) FOR [PlayerID]
GO
/****** Object:  Default [DF_PlayerScreenLog_PlayerName]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_PlayerName]  DEFAULT ('') FOR [PlayerName]
GO
/****** Object:  Default [DF_PlayerScreenLog_ScreenID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_ScreenID]  DEFAULT ((0)) FOR [ScreenID]
GO
/****** Object:  Default [DF_PlayerScreenLog_ScreenName]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_ScreenName]  DEFAULT ('') FOR [ScreenName]
GO
/****** Object:  Default [DF_PlayerScreenLog_DisplayDateTime]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_DisplayDateTime]  DEFAULT (getutcdate()) FOR [DisplayDateTime]
GO
/****** Object:  Default [DF_PlayerScreenLog_CloseDateTime]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_CloseDateTime]  DEFAULT ('1/1/1900') FOR [CloseDateTime]
GO
/****** Object:  Default [DF_PlayerScreenLog_ScreenDetails]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenLog] ADD  CONSTRAINT [DF_PlayerScreenLog_ScreenDetails]  DEFAULT ('') FOR [ScreenDetails]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_AccountID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_AccountID]  DEFAULT ((0)) FOR [AccountID]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_PlayerID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_PlayerID]  DEFAULT ((0)) FOR [PlayerID]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_PlayerName]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_PlayerName]  DEFAULT ('') FOR [PlayerName]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenID]  DEFAULT ((0)) FOR [ScreenID]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenName]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenName]  DEFAULT ('') FOR [ScreenName]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenContentID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenContentID]  DEFAULT ((0)) FOR [ScreenContentID]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenContent]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenContent]  DEFAULT ('') FOR [ScreenContentName]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenContentTypeID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenContentTypeID]  DEFAULT ((0)) FOR [ScreenContentTypeID]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ScreenContentTypeName]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ScreenContentTypeName]  DEFAULT ('') FOR [ScreenContentTypeName]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_DisplayDateTime]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_DisplayDateTime]  DEFAULT (getutcdate()) FOR [DisplayDateTime]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_CloseDateTime]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_CloseDateTime]  DEFAULT (((1)/(1))/(1900)) FOR [CloseDateTime]
GO
/****** Object:  Default [DF_PlayerScreenContentLog_ContentDetails]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[PlayerScreenContentLog] ADD  CONSTRAINT [DF_PlayerScreenContentLog_ContentDetails]  DEFAULT ('') FOR [ContentDetails]
GO
/****** Object:  Default [DF_ActivityLog_AccountID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_AccountID]  DEFAULT ((0)) FOR [AccountID]
GO
/****** Object:  Default [DF_ActivityLog_UserID]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_UserID]  DEFAULT ((0)) FOR [UserID]
GO
/****** Object:  Default [DF_ActivityLog_Username]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_Username]  DEFAULT ('') FOR [Username]
GO
/****** Object:  Default [DF_ActivityLog_EntityType]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_EntityType]  DEFAULT ('') FOR [EntityType]
GO
/****** Object:  Default [DF_ActivityLog_EntityAction]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_EntityAction]  DEFAULT ('') FOR [EntityAction]
GO
/****** Object:  Default [DF_ActivityLog_ActivityDateTime]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_ActivityDateTime]  DEFAULT (getutcdate()) FOR [ActivityDateTime]
GO
/****** Object:  Default [DF_ActivityLog_ActivityDetails]    Script Date: 03/20/2012 09:00:09 ******/
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_ActivityDetails]  DEFAULT ('') FOR [ActivityDetails]
GO
