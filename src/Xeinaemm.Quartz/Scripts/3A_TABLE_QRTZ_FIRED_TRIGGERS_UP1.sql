﻿ALTER TABLE [dbo].[QRTZ_FIRED_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_QRTZ_FIRED_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [ENTRY_ID]
  ) 
GO