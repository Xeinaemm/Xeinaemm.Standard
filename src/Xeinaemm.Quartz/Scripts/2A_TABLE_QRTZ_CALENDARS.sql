﻿IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_CALENDARS]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
BEGIN
	CREATE TABLE [dbo].[QRTZ_CALENDARS] (
	  [SCHED_NAME] [NVARCHAR] (120)  NOT NULL ,
	  [CALENDAR_NAME] [NVARCHAR] (200)  NOT NULL ,
	  [CALENDAR] [VARBINARY](MAX) NOT NULL
	)
END
GO