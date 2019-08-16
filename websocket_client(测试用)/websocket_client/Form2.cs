using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace websocket_client
{
    public partial class Form2 : Form
    {
        private Form1 frmMain;
        private websocketApp serveA=new websocketApp();

        public Form2()
        {
            InitializeComponent();
          
        }
        public Form2(Form1 frmMain) : this()
        {
            this.frmMain = frmMain;
        }

        public delegate void MyDelegate(string urlStr, string txt);      
        private void Form2_Load(object sender, EventArgs e)
        {
            ////获取主窗体中给出的服务器ip
            string urlStr = frmMain.serviceUrl;
            serveA.urlChange(urlStr);
        }       
        //"发送"按钮单击事件
        private void Bt1_Click(object sender, EventArgs e)
        {
            string beforeStr = tb1.Text.Trim();
            
            List<downData> comm = new List<downData>();
            downData One = new downData();
            One.id = 1;
            One.key = "Channel2.ZXB.1HBENG_K";
            One.Value = beforeStr;
            comm.Add(One);
            downData Two = new downData();
            Two.id = 2;
            Two.key = "Channel2.ZXB.1HBENG_T";
            Two.Value = tb2.Text.Trim();
            comm.Add(Two);
            string trueStr = JsonConvert.SerializeObject(comm);
            serveA.dateSend(trueStr);
        }
        /// <summary>
        /// 窗体关闭时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            serveA.wsClose();
        }

       
        }

    //websocket处理程序
    class websocketApp
    {   
        WebSocket ws;
       // WebSocket ws = new WebSocket("ws://" + urlString + "/WsServices");

        public void urlChange(string urlStr)
        {
            // ws.Close();
            string str = "ws://" + urlStr + "/WsServices";
            str = str.Trim();            
            ws = new WebSocket(str);
            ws.Connect();
            int i = 0;
            ws.OnOpen += (sender, e) =>
            {
                MessageBox.Show("与服务器建立连接成功");
                Console.WriteLine("与服务器建立连接成功");
            };
            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine("收到服务器数据 " + e.Data);
            };
            ws.OnClose += (sender, e) =>
            {
                MessageBox.Show("连接已关闭");
            };           
        }
        public void dateSend(string txt)
        {
            ws.Send(txt);
        }
        public void wsClose()
        {
            ws.Close();
        }
    }

}
