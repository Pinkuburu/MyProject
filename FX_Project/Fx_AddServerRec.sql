USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_AddServerRec]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_AddServerRec]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	添加服务器出错记录				     */
/* 程序名称：	Fx_AddServerRec                      */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2009.7.30			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_AddServerRec]

@ServerName varchar(50),
@ServerStatus varchar(50)

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_AddServerRec	/* 开始添加 */
			BEGIN
				INSERT INTO Fx_ServerEW(ServerName,ServerStatus) VALUES(@ServerName,@ServerStatus)
			END
		COMMIT TRANSACTION EndFx_AddServerRec	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
END
GO 