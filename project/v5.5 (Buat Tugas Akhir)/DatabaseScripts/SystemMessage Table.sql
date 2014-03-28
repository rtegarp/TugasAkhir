USE [Vodigi]
GO

/****** Object:  Table [dbo].[SystemMessage]    Script Date: 9/27/2012 8:30:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemMessage](
	[SystemMessageID] [int] IDENTITY(1000000,1) NOT NULL,
	[SystemMessageTitle] [nvarchar](64) NOT NULL,
	[SystemMessageBody] [nvarchar](256) NOT NULL,
	[DisplayDateStart] [datetime] NOT NULL,
	[DisplayDateEnd] [datetime] NOT NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_SystemMessage] PRIMARY KEY CLUSTERED 
(
	[SystemMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SystemMessage] ADD  CONSTRAINT [DF_SystemMessage_SystemMessageTitle]  DEFAULT ('') FOR [SystemMessageTitle]
GO

ALTER TABLE [dbo].[SystemMessage] ADD  CONSTRAINT [DF_SystemMessage_SystemMessage]  DEFAULT ('') FOR [SystemMessageBody]
GO

ALTER TABLE [dbo].[SystemMessage] ADD  CONSTRAINT [DF_SystemMessage_DisplayDateStart]  DEFAULT (getutcdate()) FOR [DisplayDateStart]
GO

ALTER TABLE [dbo].[SystemMessage] ADD  CONSTRAINT [DF_SystemMessage_DisplayDateEnd]  DEFAULT (getutcdate()) FOR [DisplayDateEnd]
GO


