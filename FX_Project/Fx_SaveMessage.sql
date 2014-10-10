USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_SaveMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_SaveMessage]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* �����ܣ�	�洢��Ϣ						     */
/* �������ƣ�	Fx_SaveMessage                       */
/* �����Ա��	HZW 	                             */
/* �汾ʱ�䣺	2009.7.22			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_SaveMessage]

@SID int,
@SMSContent varchar(200)

AS
BEGIN
	SET NOCOUNT ON	/*�ر�ִ��ѶϢ���Լӿ��ٶ�*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginFx_SaveMessage	/* ��ʼ��� */

			DECLARE @Out int
				IF(@SID > 0)
					BEGIN
						INSERT INTO Fx_OutBox(SID,SMSContent) VALUES(@SID,@SMSContent)
						SET @Out = 0  --��Ϣ�Ѳ���
					END
				ELSE
					BEGIN
						SET @Out = 1  --SID����
					END

		COMMIT TRANSACTION EndFx_SaveMessage	/* ��ӽ��� */
	SET NOCOUNT OFF	/* �ظ�ִ��ѶϢ */
	SELECT @Out
END
GO