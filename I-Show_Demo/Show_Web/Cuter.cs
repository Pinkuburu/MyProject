using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Show_Web
{
	public class Cuter
	{
		private string strCuter;
		private string strSplit;

		public Cuter(string cuter)
		{
			this.strCuter=cuter;
			this.strSplit=",";
		}

		public Cuter(string cuter,string split)
		{
			this.strCuter=cuter;
			this.strSplit=split;
		}

		/// <summary>
		/// ���ĳһ�ַ���
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(string item)
		{
			this.strCuter+=item+this.strSplit;
		}

		/// <summary>
		/// ɾ��ĳһ�ַ���
		/// </summary>
		/// <param name="item"></param>
		public void DelItem(string item)
		{
			string[] arrCuter=this.GetArrCuter();
			int len=arrCuter.Length;
			
			this.strCuter="";
			for(int i=0;i<len;i++)
			{
				if(item!=arrCuter[i])
				{
					this.strCuter+=arrCuter[i];

					if(i<(len-1))
					{
						this.strCuter+=this.strSplit;
					}
				}
			}

			string strLastString=this.strCuter.Substring(this.strCuter.Length-1,1);

			if(strLastString==this.strSplit)
				this.strCuter=this.strCuter.Substring(0,this.strCuter.Length-1);
//
//			string item1=item+this.strSplit;
//			string item2=this.strSplit+item;
//			try
//			{
//				int i=this.strCuter.IndexOf(item1);
//				if(i==-1)
//				{
//					this.strCuter=this.strCuter.Replace(item2,"");
//				}
//				else
//				{
//					this.strCuter=this.strCuter.Replace(item1,"");
//				}
////				int length=strCuter.Length;
////				int len=item.Length;
////				if(length>=(i+len))
////				{
////					this.strCuter=this.strCuter.Remove(i,len);
////				}
////				else
////					this.strCuter=this.strCuter.Remove(i,length-i);
//			}
//			catch(Exception e)
//			{
//				string s=e.ToString();
//			}
		}

		/// <summary>
		/// ����ĳһ�����ϵ��ַ���
		/// </summary>
		/// <param name="x"></param>
		/// <param name="s"></param>
		public void SetCuter(int x,string s)
		{
			string[] arrCuter=this.GetArrCuter();
			arrCuter[x]=s;
			string strS="";
			for(int i=0;i<this.GetSize();i++)
			{
				if(i!=this.GetSize()-1)
					strS+=arrCuter[i]+this.strSplit;
				else
					strS+=arrCuter[i];
				
			}
			this.strCuter=strS;
		}

		/// <summary>
		/// �õ��ַ�������
		/// </summary>
		/// <returns></returns>
		public string GetCuter()
		{
			return this.strCuter;
		}

		/// <summary>
		/// �õ��ַ�����
		/// </summary>
		/// <returns></returns>
		public string[] GetArrCuter()
		{
			string[] arrCuter=Regex.Split(this.strCuter,this.strSplit);
			return arrCuter;
		}

		/// <summary>
		/// ʹ�������õ�ĳһ�ַ�����������0��ʼ
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public string GetCuter(int x)
		{
			string[] arrCuter=this.GetArrCuter();
			return arrCuter[x];
		}

		/// <summary>
		/// ʹ���ַ����õ�������������0��ʼ
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int GetIndex(string item)
		{
			string[] arrCuter=this.GetArrCuter();
			int len=arrCuter.Length;
			for(int i=0;i<len;i++)
			{
				if(item==arrCuter[i])
					return i;
			}
			return -1;
		}

		/// <summary>
		/// �õ��ַ�����ĳ���
		/// </summary>
		/// <returns></returns>
		public int GetSize()
		{
			string[] arrCuter=this.GetArrCuter();
			return arrCuter.Length;
		}

		public override string ToString()
		{
			return this.strCuter;
		}
	}
}