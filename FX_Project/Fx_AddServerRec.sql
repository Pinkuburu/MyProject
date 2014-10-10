USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_AddServerRec]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_AddServerRec]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* �����ܣ�	��ӷ����������¼				     */
/* �������ƣ�	Fx_AddServerRec                      */
/* �����Ա��	HZW 	                             */
/* �汾ʱ�䣺	2009.7.30			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_AddServerRec]

@ServerName varchar(50),
@ServerStatus varchar(50)

AS
BEGIN
	SET NOCOUNT ON	/*�ر�ִ��ѶϢ���Լӿ��ٶ�*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_AddServerRec	/* ��ʼ��� */
			BEGIN
				INSERT INTO Fx_ServerEW(ServerName,ServerStatus) VALUES(@ServerName,@ServerStatus)
			END
		COMMIT TRANSACTION EndFx_AddServerRec	/* ��ӽ��� */
	SET NOCOUNT OFF	/* �ظ�ִ��ѶϢ */
END
GO 