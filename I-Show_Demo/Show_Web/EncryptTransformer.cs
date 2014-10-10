using System;
using System.Security.Cryptography;

/// <summary>
/// �㷨ö������
/// </summary>
public enum EncryptionAlgorithm {Des = 1, Rc2, Rijndael, TripleDes};

/// <summary>
/// EncryptTransformer
/// </summary>
internal class EncryptTransformer
{
	#region -- ���캯�� --
	internal EncryptTransformer(EncryptionAlgorithm algId)
	{
		// ��������ʹ�õ��㷨
		algorithmID = algId;
	}
	#endregion

	#region -- ˽�б��� --
	private EncryptionAlgorithm algorithmID;
	private byte[] initVec;
	private byte[] encKey;
	#endregion

	#region -- �������� --
	internal byte[] IV
	{
		get	{	return initVec;	}
		set	{	initVec = value;}
	}
	internal byte[] Key
	{
		get	{	return encKey;	}
	}
	#endregion

	#region -- �������� --
	internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
	{
		// ѡȡ�ṩ����
		switch (algorithmID)
		{
			case EncryptionAlgorithm.Des:
			{
				DES des = new DESCryptoServiceProvider();
				des.Mode = CipherMode.CBC;

				// �鿴�Ƿ��ṩ����Կ
				if (null == bytesKey)
				{
					encKey = des.Key;
				}
				else
				{
					des.Key = bytesKey;
					encKey = des.Key;
				}
				// �鿴�ͻ����Ƿ��ṩ�˳�ʼ������
				if (null == initVec)
				{
					// ���㷨����һ��
					initVec = des.IV;
				}
				else
				{
					//���������ṩ���㷨
					des.IV = initVec;
				}
				return des.CreateEncryptor();
			}
			case EncryptionAlgorithm.TripleDes:
			{
				TripleDES des3 = new TripleDESCryptoServiceProvider();
				des3.Mode = CipherMode.CBC;
				// See if a key was provided
				if (null == bytesKey)
				{
					encKey = des3.Key;
				}
				else
				{
					des3.Key = bytesKey;
					encKey = des3.Key;
				}
				// �鿴�ͻ����Ƿ��ṩ�˳�ʼ������
				if (null == initVec)
				{
					//�ǣ����㷨����һ��
					initVec = des3.IV;
				}
				else
				{
					//���������ṩ���㷨��
					des3.IV = initVec;
				}
				return des3.CreateEncryptor();
			}
			case EncryptionAlgorithm.Rc2:
			{
				RC2 rc2 = new RC2CryptoServiceProvider();
				rc2.Mode = CipherMode.CBC;
				// �����Ƿ��ṩ����Կ
				if (null == bytesKey)
				{
					encKey = rc2.Key;
				}
				else
				{
					rc2.Key = bytesKey;
					encKey = rc2.Key;
				}
				// �鿴�ͻ����Ƿ��ṩ�˳�ʼ������
				if (null == initVec)
				{
					//�ǣ����㷨����һ��
					initVec = rc2.IV;
				}
				else
				{
					//���������ṩ���㷨��
					rc2.IV = initVec;
				}
				return rc2.CreateEncryptor();
			}
			case EncryptionAlgorithm.Rijndael:
			{
				Rijndael rijndael = new RijndaelManaged();
				rijndael.Mode = CipherMode.CBC;
				// �����Ƿ��ṩ����Կ
				if(null == bytesKey)
				{
					encKey = rijndael.Key;
				}
				else
				{
					rijndael.Key = bytesKey;
					encKey = rijndael.Key;
				}
				// �鿴�ͻ����Ƿ��ṩ�˳�ʼ������
				if(null == initVec)
				{
					// �ǣ����㷨����һ��
					initVec = rijndael.IV;
				}
				else
				{
					// ���������ṩ���㷨��
					rijndael.IV = initVec;
				}
				return rijndael.CreateEncryptor();
			} 
			default:
			{
				throw new CryptographicException("�㷨 ID '" + algorithmID + "' ����֧��");
			}
		}
	}
	#endregion
}
