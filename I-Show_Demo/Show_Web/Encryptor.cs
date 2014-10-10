using System;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Encryptor
/// </summary>
public class Encryptor
{
	public Encryptor(EncryptionAlgorithm algId)
	{
		transformer = new EncryptTransformer(algId);
	}

	private EncryptTransformer transformer;
	private byte[] initVec;
	private byte[] encKey;

	public byte[] IV
	{
		get	{	return initVec;	}
		set	{	initVec = value;}
	}

	public byte[] Key
	{
		get	{	return encKey;	}
	}

	public byte[] Encrypt(byte[] bytesData, byte[] bytesKey)
	{
		// 设置将保存加密数据的流
		MemoryStream memStreamEncryptedData = new MemoryStream();

		transformer.IV = initVec;
		ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
		CryptoStream encStream = new CryptoStream(memStreamEncryptedData, transform, CryptoStreamMode.Write);
		try
		{
			// 加密数据，并将它们写入内存流
			encStream.Write(bytesData, 0, bytesData.Length);
		}
		catch(Exception ex)
		{
			throw new Exception("将加密数据写入流时出错： \n" + ex.Message);
		}
		// 为客户端进行检索设置初始化向量和密钥
		encKey = transformer.Key;
		initVec = transformer.IV;
		encStream.FlushFinalBlock();
		encStream.Close();

		// 发送回数据
		return memStreamEncryptedData.ToArray();
		// end Encrypt
	}
}
