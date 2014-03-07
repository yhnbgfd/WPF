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
        XmlNode xmlnode;
        XmlElement xmlelem;
        XmlTextReader reader;

        public XmlHelper()
        {
            xmldoc = new XmlDocument();
            //xmldoc.Load(Properties.Settings.Default.Path+"Data\\config.xml");
            //reader = new XmlTextReader(Properties.Settings.Default.Path + "Data\\config.xml");
        }

        public void ReadXML()
        {

        }

        public void WriteXML()
        {

        }

        public void InitXML()
        {
            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldecl);
            xmlelem = xmldoc.CreateElement("","root","");
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("root");





            xmldoc.Save(Properties.Settings.Default.Path + "Data\\config.xml");
        }
    }
}
