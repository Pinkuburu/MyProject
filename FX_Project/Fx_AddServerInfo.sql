USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_AddServerInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_AddServerInfo]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	添加服务器信息			     */
/* 程序名称：	Fx_AddServerInfo                     */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2010.1.20                	     */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_AddServerInfo]

@ServerName varchar(50),
@IP varchar(15),
@Area varchar(15),
@Content varchar(400),
@Category int

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_AddServerInfo	/* 开始添加 */

			DECLARE @IPA int,@Out int
				SELECT @IPA = COUNT(IP) FROM Fx_ServerInfo WHERE IP = @IP AND Category = @Category
				IF(@IPA > 0)
					BEGIN
						UPDATE Fx_ServerInfo SET ServerName = @ServerName,Area = @Area,Content = @Content,CreateTime = GETDATE() WHERE IP = @IP AND Category = @Category
						SET @Out = 0  --已有该用户
					END
				ELSE
					BEGIN
						INSERT INTO Fx_ServerInfo(ServerName,IP,Area,Content,Category) VALUES (@ServerName,@IP,@Area,@Content,@Category)
						SET @Out = 1  --添加成功
					END

		COMMIT TRANSACTION EndFx_AddServerInfo	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
	SELECT @Out
END
GO