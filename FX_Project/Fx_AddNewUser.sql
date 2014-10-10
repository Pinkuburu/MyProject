USE [Fx_Main]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Fx_AddNewUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Fx_AddNewUser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*****************************************************/
/* �����ܣ�	������û�						     */
/* �������ƣ�	Fx_AddNewUser                        */
/* �����Ա��	HZW 	                             */
/* �汾ʱ�䣺	2009.7.21			 				 */
/*****************************************************/

CREATE PROCEDURE [dbo].[Fx_AddNewUser]

@SID int

AS
BEGIN
	SET NOCOUNT ON	/*�ر�ִ��ѶϢ���Լӿ��ٶ�*/
	SET XACT_ABORT ON
		BEGIN TRANSACTION BeginSetToolAccount	/* ��ʼ��� */

			DECLARE @SIDA int,@Out int
				SELECT @SIDA = [SID] FROM Fx_User WHERE [SID] = @SID
				IF(@SIDA > 0)
					BEGIN
						SET @Out = 0  --���и��û�
					END
				ELSE
					BEGIN
						INSERT INTO Fx_User(SID) VALUES (@SID)
						SET @Out = 1  --��ӳɹ�
					END

		COMMIT TRANSACTION EndSetToolAccount	/* ��ӽ��� */
	SET NOCOUNT OFF	/* �ظ�ִ��ѶϢ */
	SELECT @Out
END
GO