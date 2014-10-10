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
		// ���ý�����������ݵ���
		MemoryStream memStreamEncryptedData = new MemoryStream();

		transformer.IV = initVec;
		ICryptoTransform transform = transformer.GetCryptoServiceProvider(bytesKey);
		CryptoStream encStream = new CryptoStream(memStreamEncryptedData, transform, CryptoStreamMode.Write);
		try
		{
			// �������ݣ���������д���ڴ���
			encStream.Write(bytesData, 0, bytesData.Length);
		}
		catch(Exception ex)
		{
			throw new Exception("����������д����ʱ���� \n" + ex.Message);
		}
		// Ϊ�ͻ��˽��м������ó�ʼ����������Կ
		encKey = transformer.Key;
		initVec = transformer.IV;
		encStream.FlushFinalBlock();
		encStream.Close();

		// ���ͻ�����
		return memStreamEncryptedData.ToArray();
		// end Encrypt
	}
}
