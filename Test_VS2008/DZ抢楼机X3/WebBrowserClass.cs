using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DZ抢楼机X3
{
    class WebBrowserClass
    {
        /// <summary>
        /// 设置HTML控件显示文本，根据控件NAME值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strname">控件NAME值</param>
        /// <param name="strvalue">控件显示文本</param>
        public void WebWsername(WebBrowser web, string strtype, string strname, string strvalue)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("name").ToString() == strname)
                {
                    elem.SetAttribute("value", strvalue);
                }
            }
        }
        /// <summary>
        /// 设置多个HTML控件显示文本，根据控件NAME值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="arrname">arrname[]控件name,arrname[][]控件显示文本</param>
        public void WebWsernameListName(WebBrowser web, string strtype, string[][] arrname)
        {

            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            for (int i = 0; i < arrname[0].Length; i++)
            {
                foreach (HtmlElement elem in elemlist)
                {
                    if (elem.GetAttribute("name").ToString() == arrname[i][0])
                    {
                        elem.SetAttribute("value", arrname[i][1]);
                    }
                }
            }

        }
        /// <summary>
        /// 设置多个HTML控件显示文本，根据控件ID值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="arrid">arrname[]控件id,arrname[][]控件显示文本</param>
        public void WebWsernameListID(WebBrowser web, string strtype, string[][] arrid)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            for (int i = 0; i < arrid.Length; i++)
            {
                foreach (HtmlElement elem in elemlist)
                {
                    if (elem.GetAttribute("id").ToString() == arrid[i][0])
                    {
                        elem.SetAttribute("value", arrid[i][1]);
                    }
                }
            }

        }
        /// <summary>
        /// 设置HTML控件显示文本，根据空间ID值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strid">控件ID值</param>
        /// <param name="strvalue">控件显示文本</param>
        public void WebWserid(WebBrowser web, string strtype, string strid, string strvalue)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("id").ToString() == strid)
                {
                    elem.SetAttribute("value", strvalue);
                }
            }
        }
        /// <summary>
        /// 激发HTML事件，根据空间ID值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strid">控件ID值</param>
        /// <param name="exname">控件事件名称</param>
        public void WebExid(WebBrowser web, string strtype, string strid, string exname)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {

                if (elem.GetAttribute("id").ToString() == strid)
                {
                    elem.InvokeMember(exname);
                }
            }
        }
        /// <summary>
        /// 激发HTML事件，根据空间NAME值
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strname">控件NAME值</param>
        /// <param name="exname">控件事件名称</param>
        public void WebExname(WebBrowser web, string strtype, string strid, string exname)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("name").ToString() == strid)
                {
                    elem.InvokeMember(exname);
                }
            }
        }
        /// <summary>
        /// 执行当前HTML页中用脚本语言定义的函数
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strSrc">控件NAME值</param>
        /// <param name="exname">控件事件名称</param>
        public void WebExSrc(WebBrowser web, string strtype, string strSrc, string exname)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                elem.InvokeMember(exname);
            }
        }
        /// <summary>
        /// 设置HTML控件的属性,针对radiobutton和checkbox
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strname">控件id</param>
        /// <param name="ischeck">true为选中,false为勾掉</param>
        public void WebExCheck(WebBrowser web, string strtype, string strname, bool ischeck)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("id").ToString() == strname)
                {
                    elem.SetAttribute("checked", ischeck.ToString());
                }
            }
        }
        /// <summary>
        /// 在当前HTML页中选中Select中与定义的索引值相同的项
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strid">控件NAME值</param>
        /// <param name="strvalue">要选中的索引值</param>
        public void WebSelect(WebBrowser web, string strtype, string strid, string strvalue)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("name").ToString() == strid)
                {
                    elem.SetAttribute("SelectedIndex", strvalue);
                }
            }
        }
        /// <summary>
        /// 在当前HTML页中选中Select中与定义的索引值相同的项
        /// </summary>
        /// <param name="web">WebBrowser对象</param>
        /// <param name="strtype">控件类型</param>
        /// <param name="strid">控件NAME值</param>
        /// <param name="strvalue">没用的</param>
        public void WebSelectheck(WebBrowser web, string strtype, string strid, string strvalue)
        {
            HtmlElementCollection elemlist = web.Document.GetElementsByTagName(strtype);
            foreach (HtmlElement elem in elemlist)
            {
                if (elem.GetAttribute("name").ToString() == strid)
                {
                    elem.SetAttribute("Options[22]", "selected");
                }
            }
        }
    }
}
