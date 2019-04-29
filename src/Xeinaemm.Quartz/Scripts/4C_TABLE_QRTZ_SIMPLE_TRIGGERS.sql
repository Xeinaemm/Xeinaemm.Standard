﻿IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_SIMPLE_TRIGGERS]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
	CREATE TABLE [dbo].[QRTZ_SIMPLE_TRIGGERS] (
	  [SCHED_NAME] [NVARCHAR] (120)  NOT NULL ,
	  [TRIGGER_NAME] [NVARCHAR] (150)  NOT NULL ,
	  [TRIGGER_GROUP] [NVARCHAR] (150)  NOT NULL ,
	  [REPEAT_COUNT] [INTEGER] NOT NULL ,
	  [REPEAT_INTERVAL] [BIGINT] NOT NULL ,
	  [TIMES_TRIGGERED] [INTEGER] NOT NULL
	)
GO