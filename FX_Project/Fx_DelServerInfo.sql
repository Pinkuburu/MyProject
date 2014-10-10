USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_DelServerInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_DelServerInfo]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* 程序功能：	删除服务器信息					     */
/* 程序名称：	Fx_DelServerInfo                     */
/* 设计人员：	HZW 	                             */
/* 版本时间：	2010.1.20			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_DelServerInfo]

@IP varchar(15)

AS
BEGIN
	SET NOCOUNT ON	/*关闭执行讯息，以加快速度*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_DelServerInfo	/* 开始添加 */

			DECLARE @IPA int,@Out int
				SELECT @IPA = COUNT(IP) FROM Fx_ServerInfo WHERE IP = @IP
				IF(@IPA > 0)
					BEGIN
						DELETE FROM Fx_ServerInfo WHERE IP = @IP
						SET @Out = 1  --删除成功
					END
				ELSE
					BEGIN
						SET @Out = 0  --未找到要删除的信息
					END

		COMMIT TRANSACTION EndFx_DelServerInfo	/* 添加结束 */
	SET NOCOUNT OFF	/* 回复执行讯息 */
	SELECT @Out
END
GO