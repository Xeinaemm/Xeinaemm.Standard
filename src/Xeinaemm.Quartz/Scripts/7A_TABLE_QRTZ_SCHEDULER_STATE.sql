﻿IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_SCHEDULER_STATE]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
BEGIN
	CREATE TABLE [dbo].[QRTZ_SCHEDULER_STATE] (
	  [SCHED_NAME] [NVARCHAR] (120)  NOT NULL ,
	  [INSTANCE_NAME] [NVARCHAR] (200)  NOT NULL ,
	  [LAST_CHECKIN_TIME] [BIGINT] NOT NULL ,
	  [CHECKIN_INTERVAL] [BIGINT] NOT NULL
	)
END
GO