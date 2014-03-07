using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Wpf.Helper
{
    public class XmlHelper
    {
        XmlDocument xmldoc;

        public XmlHelper()
        {
            xmldoc = new XmlDocument();
        }
        public void LoadXML()
        {
            xmldoc.Load(Properties.Settings.Default.Path + "Data\\config.xml");
        }
        public void SaveXML()
        {
            xmldoc.Save(Properties.Settings.Default.Path + "Data\\config.xml");
        }

        public string ReadXML(string Element)
        {
            XmlNodeList rootList = xmldoc.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode xn1 in rootList)
            {
                XmlElement xe1 = (XmlElement)xn1;
                XmlNodeList xnl1 = xe1.ChildNodes;
                foreach (XmlNode xn2 in xnl1)
                {
                    XmlElement xe2 = (XmlElement)xn2;
                    if (xe2.Name == Element)
                    {
                        return xe2.InnerText;
                    }
                }
            }
            return "0";
        }

        public void WriteXML(string Element, string Text)
        {
            XmlNodeList rootList = xmldoc.SelectSingleNode("root").ChildNodes;
            foreach(XmlNode xn1 in rootList)
            {
                XmlElement xe1 = (XmlElement)xn1;
                XmlNodeList xnl1 = xe1.ChildNodes;
                foreach(XmlNode xn2 in xnl1)
                {
                    XmlElement xe2 = (XmlElement)xn2;
                    if(xe2.Name == Element)
                    {
                        xe2.InnerText = Text;
                    }
                }
            }
        }

        public void InitXML()
        {
            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldecl);
            XmlElement xmlelem = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("root");
            #region 初始金额
            XmlElement 初始金额 = xmldoc.CreateElement("初始金额");
            XmlElement 预算内户 = xmldoc.CreateElement("预算内户");
            预算内户.InnerText = "0.0";
            XmlElement 预算外户 = xmldoc.CreateElement("预算外户");
            预算外户.InnerText = "0.0";
            XmlElement 周转金户 = xmldoc.CreateElement("周转金户");
            周转金户.InnerText = "0.0";
            XmlElement 计生专户 = xmldoc.CreateElement("计生专户");
            计生专户.InnerText = "0.0";
            XmlElement 政粮补贴资金专户 = xmldoc.CreateElement("政粮补贴资金专户");
            政粮补贴资金专户.InnerText = "0.0";
            XmlElement 土地户 = xmldoc.CreateElement("土地户");
            土地户.InnerText = "0.0";

            初始金额.AppendChild(预算内户);
            初始金额.AppendChild(预算外户);
            初始金额.AppendChild(周转金户);
            初始金额.AppendChild(计生专户);
            初始金额.AppendChild(政粮补贴资金专户);
            初始金额.AppendChild(土地户);

            root.AppendChild(初始金额);
            #endregion

            #region 注册信息
            XmlElement 注册信息 = xmldoc.CreateElement("注册信息");
            XmlElement 时间 = xmldoc.CreateElement("时间");
            时间.InnerText = "时间";
            XmlElement 序列号 = xmldoc.CreateElement("注册码");
            序列号.InnerText = "注册码";

            注册信息.AppendChild(时间);
            注册信息.AppendChild(序列号);

            root.AppendChild(注册信息);
            #  endregion

            this.SaveXML();
        }
    }
}
