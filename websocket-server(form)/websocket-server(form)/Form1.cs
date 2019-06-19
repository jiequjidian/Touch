using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Data.SqlClient;
using System.Threading;
using System.Reflection;

namespace websocket_server_form_
{
    public delegate void deleTestlbl( string txt);
    public  partial class Form1 : Form
    {
        public static Form1 mainForm = null;
        public static char flag ='0';
        public static WebSocketServer wssv;
        public Form1()
        {
            InitializeComponent();
            mainForm = this;
            //this.ControlBox = false;
        }
        
       
        private void Form1_Load(object sender, EventArgs e)
        {
            //创建websocketserver实例
            var wssv = new WebSocketServer("ws://0.0.0.0:6690");

            wssv.AddWebSocketService<WsServices>("/WsServices");           
            ////开启websocket
            wssv.Start();
            if(wssv.Port !=0)
            {
                Console.WriteLine(wssv.Port);
            }
          
        }
       
        
        //更改显示框函数
        public void UpdateTextBox( string txt)
        {
            if (!richTextBox1.InvokeRequired)
            {
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
        //无用，不可删除
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
            if(button2.Text== "暂停")
            {
                richTextBox1.ReadOnly = true;
                button2.Text ="继续";
            }
            else
            {
                richTextBox1.ReadOnly = false;
                button2.Text = "暂停";
            }
            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string[] nameBeng = { "赵重路", "通波塘", "华青路", "新业路", "新区路", "赵巷A", "赵巷B", "赵巷C", "金星路", "外青松", "新金路", "汇金路", "民乐路", "新城一站" };
            string[] nameSpell = { "ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "ML", "XCYZ" };
            for (int i = 0; i < nameSpell.Length; i++)
            {//遍历所有泵站
               //1.读取该泵站在数据库中的表格数据，返回datatable本地表

            //2.遍历数据采集端上传的数据rb，将rb中该泵站的各值更新到本地表中

            //3.将本地表更新到数据库
              
            }
        }

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


        //读取数据库中上一次存入的数据（末尾13行）
        //public DataTable readSQL()
        //{
        //    DataTable mytable = new DataTable();
        //    SQLdispose myDispose = new SQLdispose();
        //    string strSQL = "select top 13 * from DataInfo order by id desc ";
        //    mytable = myDispose.ExecuteWithReturn(strSQL);
        //    int ccc = 0;
        //    return mytable;

        //}
    }






    public delegate void MyDelegate(string Item1);//委托实质上是一个类
    public delegate void SQLDelegate(List<upDate> rb);
    //websocket事项
    public class WsServices : WebSocketBehavior
    {
        int count = 0;
        int flagFirst = 0;
        protected override void OnOpen()
        {
            Console.WriteLine("Connection Open");
            base.OnOpen();
        }
        //信息来往事件
        protected override void OnMessage(MessageEventArgs e)
        {
            //以list为容器存放opc传来的json array
            List<upDate> rb = JsonConvert.DeserializeObject<List<upDate>>(e.Data);            
            //如果是第一条数据，立刻更新到数据库
            if (flagFirst == 0)
            {
                appendDataToSQL(rb);
                flagFirst = 1;
                Console.WriteLine("更新了@@第一条数据");
            }
            else
            {
                //每五分钟更新1条数据到数据库
                string aaa = DateTime.Now.Minute.ToString();
                int anum = aaa[aaa.Length - 1] - '0';
                if (anum == 0 || anum == 5)
                {
                    if (count == 0)
                    {
                        SQLDelegate mySqlDele = new SQLDelegate(appendDataToSQL);
                        mySqlDele(rb);
                        count = 1;
                        Console.WriteLine("更新了一条数据" + aaa);
                    }
                }
                else
                {
                    count = 0;
                }
            }

            string message = "∆p0 = " + e.Data + "\r\n";
            //更新窗体文本显示
            deleTestlbl myDelegate = new deleTestlbl(Form1.mainForm.UpdateTextBox);
            myDelegate(message);

           
            
                

            //如果有新连接，则立刻从数据库更新一条数据
            //if (Sessions.Count != flagSessions)
            //{
            //    List<downDate> rd = new List<downDate>();
            //    DataTable downTable = readSQL();
                
            //    //Console.WriteLine("key:" + rb[0].key);          
            //    foreach (DataRow row in downTable.Rows)
            //    {//第0列是id，第1列是时间，第2列是泵名，第3列开始是各值
            //        for(int j=3;j< downTable.Columns.Count;j++)
            //        {
            //            downDate rcChild = new downDate();                       
            //            rcChild.ID0 = "";
            //            rcChild.ID1 = row["bengName"].ToString().Trim();
            //            rcChild.ID2 = downTable.Columns[j].ColumnName.Trim();
            //            rcChild.Value = row[j].ToString().Trim();
            //            rd.Add(rcChild);
            //        }
            //    }
            //    List<downDate> kkkk = rd;
            //    Sessions.Broadcast(JsonConvert.SerializeObject(rd));
            //    flagSessions = Sessions.Count;
            //    flagSend = 1;
            //}
            //else
            {
                //if(flagSend==0)
                {
                    //发送给其他客户端
                    List<downDate> rc = new List<downDate>();
                    //Console.WriteLine("key:" + rb[0].key);          
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
                //else
                //{
                //    if(flagSend>=1&&flagSend<20)
                //    {
                //        flagSend++;
                //    }
                //    else
                //    {
                //        flagSend = 0;
                //    }
                //}
            }
           
        }
        //关闭服务事件
        protected override void OnClose(CloseEventArgs e)
        {
            
           // base.OnClose(e);
        }

        public void appendData(string column, string values)
        {
            //string column = "DataTime,PH,B_ID";
            //string values = "2018/04/12,3.65,30";

            SQLdispose myDispose = new SQLdispose();
            myDispose.appendData(column, values);

        }
        //读取数据库中上一次存入的数据（末尾13行）
        //public DataTable readSQL()
        //{
        //    DataTable mytable = new DataTable();
        //    SQLdispose myDispose = new SQLdispose();
        //    string strSQL = "select top 14 * from DataInfo order by id desc ";
        //    mytable = myDispose.ExecuteWithReturn(strSQL);
        //    int ccc = 0;
        //    return mytable;
        //}
        public string[] readSQLHead()
        {
            SQLdispose myDispose = new SQLdispose();
            string[] myStr= myDispose.readeColumn();
           
            return myStr;
        }

        //向数据库中追加一table的数据
        public  void appendDataToSQL(List<upDate> rb)
        {
            int bengCount = 14;
            count++;
            Console.WriteLine("@@@"+DateTime.Now.ToString());
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
        //string[] nameBeng = { "赵重路", "通波塘", "华青路", "新业路", "新区路", "赵巷A", "赵巷B", "赵巷C", "金星路", "外青松", "新金路", "汇金路", "民乐路", "新城一站" };
        //string[] nameSpell = { "ZCL", "TBT", "HQL", "XYL", "XQL", "ZXA", "ZXB", "ZXC", "JXL", "WQS", "XJL", "HJL", "ML", "XCYZ" };
        


       

    }

   
}
