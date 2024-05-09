using Clear_VS_Recent.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clear_VS_Recent
{
    public partial class Form1 : Form
    {
        private static readonly string BeginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\VisualStudio");
        private JArray jsonArray;
        public string ConfigPath { get; set; }

        public Form1()
        {
            InitializeComponent();
            SetConfigPath();
            Text = Assembly.GetCallingAssembly().GetName().Name;
        }
        /// <summary>
        /// 设置ConfigPath
        /// </summary>
        private void SetConfigPath()
        {
            var folderList = Directory.GetDirectories(BeginPath);
            foreach (var folder in folderList)
            {
                var Regex = System.Text.RegularExpressions.Regex.Match(folder, @"\d{2}\..*");
                if (Regex.Success)
                    ConfigPath = Path.Combine(BeginPath, Regex.Value, "ApplicationPrivateSettings.xml");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strJson = ReadXml.Read(ConfigPath);
            if (strJson == null) return;
            jsonArray = Json.Deserialize(strJson);
            if (jsonArray == null) return;
            Action action = () =>
            {
                foreach (var item in jsonArray)
                {
                    string name = item["Key"].ToString();
                    string time = item["Value"]["LastAccessed"].ToString();
                    var listItem = new ListViewItem(name);
                    listItem.SubItems.Add(time);
                    listView1.Items.Add(listItem);
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            };

            Thread thread = new Thread(() =>
            {
                this.Invoke(action);
            });
            thread.Start();

        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            btnSelectAll.Enabled = false;
            btnCancelSelectAll.Enabled = true;
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSelectAll_Click(object sender, EventArgs e)
        {
            btnSelectAll.Enabled = true;
            btnCancelSelectAll.Enabled = false;
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            btnSelectAll.Enabled = true;
            btnCancelSelectAll.Enabled = true;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    item.Remove();
                    if (listView1.Items.Count == 0)
                    {
                        btnSelectAll.Enabled = false;
                        btnCancelSelectAll.Enabled = false;
                    }

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        if (jsonArray[i]["Key"].ToString() == item.Text)
                        {
                            jsonArray[i].Remove();
                        }
                    }

                }

            }

            ReadXml.XmlNode.InnerText = jsonArray.ToString();
            ReadXml.XmlDocument.Save(ConfigPath);

        }
    }
}
