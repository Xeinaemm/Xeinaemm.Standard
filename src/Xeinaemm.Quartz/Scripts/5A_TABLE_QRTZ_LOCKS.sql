﻿IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_LOCKS]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
BEGIN
	CREATE TABLE [dbo].[QRTZ_LOCKS] (
	  [SCHED_NAME] [NVARCHAR] (120)  NOT NULL ,
	  [LOCK_NAME] [NVARCHAR] (40)  NOT NULL 
	)
END
GO