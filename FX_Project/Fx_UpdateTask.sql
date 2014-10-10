USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_UpdateTaskRuntime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_UpdateTaskRuntime]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* �����ܣ�	���¼ƻ�����ʱ��				     */
/* �������ƣ�	Fx_UpdateTaskRuntime                 */
/* �����Ա��	HZW 	                             */
/* �汾ʱ�䣺	2009.7.31			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_UpdateTaskRuntime]

@ID int

AS
BEGIN
	SET NOCOUNT ON	/*�ر�ִ��ѶϢ���Լӿ��ٶ�*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_UpdateTaskRuntime	/* ��ʼ��� */

			DECLARE @Out int,@IDA int
				SET @IDA = -1
				SET @IDA = @ID
				IF(@IDA < 0)
					BEGIN
						SET @Out = 0  --ID����
					END
				ELSE
					BEGIN
						UPDATE Fx_Task SET RunTime = DateAdd(Day,1,GetDate()) WHERE ID = @IDA
						SET @Out = 1  --��Ϣ�ѱ�Ƿ���
					END

		COMMIT TRANSACTION EndFx_UpdateTaskRuntime	/* ��ӽ��� */
	SET NOCOUNT OFF	/* �ظ�ִ��ѶϢ */
	SELECT @Out
END
GO 