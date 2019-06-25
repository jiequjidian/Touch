using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Net;
using System.Net.Sockets;

namespace websocket_server_form_
{
    public delegate void deleTestlbl(string txt);
    public partial class Form1 : Form
    {
        public static Form1 mainForm = null;
        public static char flag = '0';
        public static WebSocketServer wssv;
        public Form1()
        {
            InitializeComponent();
            mainForm = this;
            //this.ControlBox = false;
        }

        /// <summary>
        /// 主窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            string ipStr = GetLocalIP();
            Console.WriteLine("本机IP：" + ipStr);
            //创建websocketserver实例

            var wssv = new WebSocketServer("ws://" + ipStr + ":6690");
            wssv.AddWebSocketService<WsServices>("/WsServices");
            ////开启websocket
            wssv.Start();
            if (wssv.Port != 0)
            {//打印当前ip和端口号
                Console.WriteLine("IP:" + wssv.Address);
                Console.WriteLine("端口号" + wssv.Port);
            }

            if (wssv.IsListening)
            {
                Console.WriteLine("@@");
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                foreach (var path in wssv.WebSocketServices.Paths)
                {
                    Console.WriteLine("- {0}", path);
                    Console.WriteLine("@");
                    Console.WriteLine(path);
                }

                Console.WriteLine("@@");
            }
        }

        /// <summary>
        /// 更新数据显示框
        /// </summary>
        string[] rbText = new string[2];
        int rbLineNum = 0;
        public void UpdateTextBox(string txt)
        {
            if (!richTextBox1.InvokeRequired)
            {
                rbLineNum++;
                //若更新次数达到1000，保留500次的数据
                if (rbLineNum <= 500)
                {
                    rbText[0] += txt;
                }
                else if (rbLineNum > 500 && rbLineNum <= 1000)
                {
                    rbText[1] += txt;
                }
                else
                {
                    richTextBox1.Text = rbText[1];
                }
                int pos = richTextBox1.SelectionStart;
                richTextBox1.AppendText(txt);
                richTextBox1.SelectionStart = pos;
                //richTextBox1.Focus();               
                //richTextBox1.Text += txt; // 主线程调用时，直接赋值
            }
            else
            {
                // 多线程调用时，通过主线程去访问
                deleTestlbl de = UpdateTextBox;
                this.Invoke(de, txt);
            }
        }
        //始终将光标定位到最新数据
        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;                    //rtbReceive为控件的名字（自己取）
            richTextBox1.ScrollToCaret();
        }
        //将窗体设置为不可关闭
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 取消关闭窗体
            e.Cancel = true;
            // 将窗体变为最小化
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false; //不显示在系统任务栏 
            notifyIcon1.Visible = true; //托盘图标可见 
        }

        //任务栏图标双击事件
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        //清空按钮点击事件
        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }
        //暂停按钮点击事件
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "暂停")
            {
                richTextBox1.ReadOnly = true;
                button2.Text = "继续";
            }
            else
            {
                richTextBox1.ReadOnly = false;
                button2.Text = "暂停";
            }
        }


        //任务栏菜单
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //退出窗体
            // System.Environment.Exit(0);
            this.Dispose();
            this.Close();
        }


        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
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



        //创建委托，用于像数据库中写入数据
        public delegate void SQLDelegate(List<upDate> rb);
        /// <summary>
        /// websocket事件处理
        /// </summary>
        public class WsServices : WebSocketBehavior
        {
            static int count = 0;
            static int flagFirst = 0;
            /// <summary>
            /// 打开websocket连接
            /// </summary>
            protected override void OnOpen()
            {
                //获取客户端IP和port
                string IPAddress = base.Sessions.Sessions.First().Context.UserEndPoint.ToString();
                string IPAddress2 = base.Context.UserEndPoint.ToString();
                Console.WriteLine("连接的客户端：" + IPAddress);
                Console.WriteLine("连接的客户端：" + IPAddress2);

                MessageBox.Show("Connection Open");
                base.OnOpen();
            }
            /// <summary>
            /// 信息到达处理
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMessage(MessageEventArgs e)
            {               

                //以list为容器存放opc传来的json array
                List<upDate> rb = JsonConvert.DeserializeObject<List<upDate>>(e.Data);

                //将task并行执行，避免主线程阻塞
                new Action(async () =>
                {
                    await task_updateShow();  //更新窗体显示
                    await task_formatAndBrodcast();//格式化数据并广播
                    await task_writeSQL();//写入数据库
                })();

                //异步调用更新窗体显示======================================================
                async Task task_updateShow()
                {
                    //更新窗体文本显示
                    string txt = "@==@" + e.Data + "\r\n";
                    deleTestlbl myDelegate = new deleTestlbl(Form1.mainForm.UpdateTextBox);
                    myDelegate(txt);
                }

                //格式化数据并广播（异步）====================================================
                async Task task_formatAndBrodcast()
                {
                    //发送给其他客户端
                    List<downDate> rc = new List<downDate>();
                    for (int i = 0; i < rb.Count; i++)
                    {
                        downDate rcChild = new downDate();
                        string[] str3 = rb[i].key.Split('.');
                        rcChild.ID0 = str3[0];
                        rcChild.ID1 = str3[1];
                        rcChild.ID2 = str3[2];
                        rcChild.Value = rb[i].Value;
                        rc.Add(rcChild);
                    }

                    Sessions.Broadcast(JsonConvert.SerializeObject(rc));
                }

                //异步操作数据库===================================================================
                async Task task_writeSQL()
                {
                    //如果是第一条数据，立刻更新到数据库
                    if (flagFirst == 0)
                    {
                        appendDataToSQL(rb);
                        flagFirst = 1;
                        MessageBox.Show("更新了@@第一条数据");
                    }
                    else
                    {
                        //每五分钟更新1条数据到数据库
                        string nowTime = DateTime.Now.Minute.ToString();
                        int anum = nowTime[nowTime.Length - 1] - '0';
                        if (anum == 0 || anum == 5)
                        {
                            if (count == 0)
                            {
                                SQLDelegate mySqlDele = new SQLDelegate(appendDataToSQL);
                                mySqlDele(rb);
                                count = 1;
                                MessageBox.Show("更新了一条数据" + nowTime);
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                }
            }





            //向数据库中追加一table的数据
            public void appendDataToSQL(List<upDate> rb)
            {
                int bengCount = 14;
                count++;
                Console.WriteLine("@@@" + DateTime.Now.ToString());
                DataTable sqlTable = new DataTable();

                //从数据库中读取bengCount条数据
                string strSQL = "select top 14 * from DataInfo order by id desc ";
                SQLdispose myDispose = new SQLdispose();
                sqlTable = myDispose.ExecuteWithReturn(strSQL);

                //读取字段
                foreach (DataRow nowRow in sqlTable.Rows)
                {
                    int num222 = Convert.ToInt32(nowRow["ID"]) + bengCount;
                    nowRow["ID"] = num222;
                    nowRow["DateTime"] = DateTime.Now;
                }
                for (int i = 0; i < rb.Count; i++)
                {
                    string[] str3 = rb[i].key.Split('.');
                    if (sqlTable.Columns.Contains(str3[2]) == false)
                    {//如果数据表有没有该tag列，在表中新增一列
                        sqlTable.Columns.Add(str3[2], typeof(string));
                    }
                    // string coulumName = str3[2];
                    foreach (DataRow nowRow in sqlTable.Rows)
                    {
                        if (str3[1] == nowRow["bengName"].ToString().Trim())
                        {//如果泵站名在该row（行），则将数据更新到表中对应位置
                         //nowRow["PH"] = 3.1516;  
                            nowRow[str3[2]] = rb[i].Value;
                            break;
                        }
                    }
                }
                DataView dv = new DataView(sqlTable);
                dv.Sort = "id asc";
                sqlTable = dv.ToTable();
                //DataTable ceshiTable = sqlTable;
                myDispose.appendToSQL(sqlTable);
                // return true;
            }




        }



    }


}
