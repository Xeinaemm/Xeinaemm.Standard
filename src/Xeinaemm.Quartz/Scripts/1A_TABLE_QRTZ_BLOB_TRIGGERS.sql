﻿IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_BLOB_TRIGGERS]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
BEGIN
	CREATE TABLE [dbo].[QRTZ_BLOB_TRIGGERS] (
	  [SCHED_NAME] [NVARCHAR] (120)  NOT NULL ,
	  [TRIGGER_NAME] [NVARCHAR] (150)  NOT NULL ,
	  [TRIGGER_GROUP] [NVARCHAR] (150)  NOT NULL ,
	  [BLOB_DATA] [VARBINARY](MAX) NULL
	)
END
GO