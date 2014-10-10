USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_SaveMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_SaveMessage]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	存储消息						     */
/* 程序名称：	Fx_SaveMessage                       */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2009.7.22			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_SaveMessage]

@SID int,
@SMSContent varchar(200)

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_SaveMessage	/* 开始添加 */

			DECLARE @Out int
				IF(@SID > 0)
					BEGIN
						INSERT INTO Fx_OutBox(SID,SMSContent) VALUES(@SID,@SMSContent)
						SET @Out = 0  --信息已插入
					END
				ELSE
					BEGIN
						SET @Out = 1  --SID出错
					END

		COMMIT TRANSACTION EndFx_SaveMessage	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
	SELECT @Out
END
GO