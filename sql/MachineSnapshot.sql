USE [Faspac]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MachineSnapshot](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StopDateTime] [smalldatetime] NULL,
	[MachineIp] [nvarchar](15) NULL,
	[MachineName] [nvarchar](50) NULL,
	[PouchesPerMinute] [int] NULL,
	[CyclingTime] [nvarchar](15) NULL,
	[CycleCount] [int] NULL,
	[HourMeter] [int] NULL,
	[Comment] [nvarchar](50) NULL
) ON [PRIMARY]

GO

