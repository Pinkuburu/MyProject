USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_UpdateMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_UpdateMessage]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* �����ܣ�	��Ƿ�����Ϣ						     */
/* �������ƣ�	Fx_UpdateMessage                       */
/* �����Ա��	HZW 	                             */
/* �汾ʱ�䣺	2009.7.23			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_UpdateMessage]

@ID int

AS
BEGIN
	SET NOCOUNT ON	/*�ر�ִ��ѶϢ���Լӿ��ٶ�*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_UpdateMessage	/* ��ʼ��� */

			DECLARE @Out int,@IDA int
				SET @IDA = -1
				SET @IDA = @ID
				IF(@IDA < 0)
					BEGIN
						SET @Out = 0  --ID����
					END
				ELSE
					BEGIN
						UPDATE Fx_OutBox SET [Status] = 1 WHERE ID = @IDA
						SET @Out = 1  --��Ϣ�ѱ�Ƿ���
					END

		COMMIT TRANSACTION EndFx_UpdateMessage	/* ��ӽ��� */
	SET NOCOUNT OFF	/* �ظ�ִ��ѶϢ */
	SELECT @Out
END
GO 