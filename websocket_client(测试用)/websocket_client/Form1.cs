using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace websocket_client
{
    public partial class Form1 : Form
    {
       //设置默认ip+port
        public string serviceUrl = GetLocalIP()+":6690";
        private Form2 form2 = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb2.Text = serviceUrl;
        }

        private void Bt2_Click(object sender, EventArgs e)
        {
            if (bt2.Text == "连接")
            {
                serviceUrl = tb2.Text;
                form2 = new Form2(this);//不加this，无法获取父窗体的值
                form2.TopLevel = false;//设置是否为顶层窗口
                form2.Dock = DockStyle.Fill;//设置停靠方式为Fill
                                            // form2.MdiParent = this;
                form2.Parent = this.panel1;
                form2.Show();
            }

        }

        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
        
}
