using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Clear_VS_Recent.Utils
{
    public class ReadXml
    {
        public static XmlDocument XmlDocument;
        public static XmlNode XmlNode;
        public static string Read(string path)
        {
            try
            {
                XmlDocument = new XmlDocument();
                XmlDocument.Load(path);
                XmlElement xmlElement = (XmlElement)XmlDocument.SelectSingleNode("//collection[@name='CodeContainers.Offline']");
                XmlNode = xmlElement.GetElementsByTagName("value")[0];
                string value = XmlNode.InnerText;
                return value;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
    }
}
