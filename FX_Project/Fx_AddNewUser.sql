USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_AddNewUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_AddNewUser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	添加新用户						     */
/* 程序名称：	Fx_AddNewUser                        */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2009.7.21			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_AddNewUser]

@SID int

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginSetToolAccount	/* 开始添加 */

			DECLARE @SIDA int,@Out int
				SELECT @SIDA = [SID] FROM Fx_User WHERE [SID] = @SID
				IF(@SIDA > 0)
					BEGIN
						SET @Out = 0  --已有该用户
					END
				ELSE
					BEGIN
						INSERT INTO Fx_User(SID) VALUES (@SID)
						SET @Out = 1  --添加成功
					END

		COMMIT TRANSACTION EndSetToolAccount	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
	SELECT @Out
END
GO