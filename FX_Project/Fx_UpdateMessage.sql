USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_UpdateMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_UpdateMessage]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	标记发出消息						     */
/* 程序名称：	Fx_UpdateMessage                       */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2009.7.23			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_UpdateMessage]

@ID int

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_UpdateMessage	/* 开始添加 */

			DECLARE @Out int,@IDA int
				SET @IDA = -1
				SET @IDA = @ID
				IF(@IDA < 0)
					BEGIN
						SET @Out = 0  --ID出错
					END
				ELSE
					BEGIN
						UPDATE Fx_OutBox SET [Status] = 1 WHERE ID = @IDA
						SET @Out = 1  --消息已标记发出
					END

		COMMIT TRANSACTION EndFx_UpdateMessage	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
	SELECT @Out
END
GO 