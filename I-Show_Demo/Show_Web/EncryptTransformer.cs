using System;
using System.Security.Cryptography;

/// <summary>
/// 算法枚举类型
/// </summary>
public enum EncryptionAlgorithm {Des = 1, Rc2, Rijndael, TripleDes};

/// <summary>
/// EncryptTransformer
/// </summary>
internal class EncryptTransformer
{
	#region -- 构造函数 --
	internal EncryptTransformer(EncryptionAlgorithm algId)
	{
		// 保存正在使用的算法
		algorithmID = algId;
	}
	#endregion

	#region -- 私有变量 --
	private EncryptionAlgorithm algorithmID;
	private byte[] initVec;
	private byte[] encKey;
	#endregion

	#region -- 公共属性 --
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

	#region -- 公共方法 --
	internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
	{
		// 选取提供程序。
		switch (algorithmID)
		{
			case EncryptionAlgorithm.Des:
			{
				DES des = new DESCryptoServiceProvider();
				des.Mode = CipherMode.CBC;

				// 查看是否提供了密钥
				if (null == bytesKey)
				{
					encKey = des.Key;
				}
				else
				{
					des.Key = bytesKey;
					encKey = des.Key;
				}
				// 查看客户端是否提供了初始化向量
				if (null == initVec)
				{
					// 让算法创建一个
					initVec = des.IV;
				}
				else
				{
					//不，将它提供给算法
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
				// 查看客户端是否提供了初始化向量
				if (null == initVec)
				{
					//是，让算法创建一个
					initVec = des3.IV;
				}
				else
				{
					//不，将它提供给算法。
					des3.IV = initVec;
				}
				return des3.CreateEncryptor();
			}
			case EncryptionAlgorithm.Rc2:
			{
				RC2 rc2 = new RC2CryptoServiceProvider();
				rc2.Mode = CipherMode.CBC;
				// 测试是否提供了密钥
				if (null == bytesKey)
				{
					encKey = rc2.Key;
				}
				else
				{
					rc2.Key = bytesKey;
					encKey = rc2.Key;
				}
				// 查看客户端是否提供了初始化向量
				if (null == initVec)
				{
					//是，让算法创建一个
					initVec = rc2.IV;
				}
				else
				{
					//不，将它提供给算法。
					rc2.IV = initVec;
				}
				return rc2.CreateEncryptor();
			}
			case EncryptionAlgorithm.Rijndael:
			{
				Rijndael rijndael = new RijndaelManaged();
				rijndael.Mode = CipherMode.CBC;
				// 测试是否提供了密钥
				if(null == bytesKey)
				{
					encKey = rijndael.Key;
				}
				else
				{
					rijndael.Key = bytesKey;
					encKey = rijndael.Key;
				}
				// 查看客户端是否提供了初始化向量
				if(null == initVec)
				{
					// 是，让算法创建一个
					initVec = rijndael.IV;
				}
				else
				{
					// 不，将它提供给算法。
					rijndael.IV = initVec;
				}
				return rijndael.CreateEncryptor();
			} 
			default:
			{
				throw new CryptographicException("算法 ID '" + algorithmID + "' 不受支持");
			}
		}
	}
	#endregion
}
