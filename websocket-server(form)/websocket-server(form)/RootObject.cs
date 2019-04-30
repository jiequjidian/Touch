using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace websocket_server_form_
{
    public class upDate
    {
        public string key { get; set; }
        public string Value { get; set; }
    }
    // class Employees
    //{
    //    public string ItemNumber { get; set; }
    //    public string ItemName { get; set; }
    //}
    public class downDate
    {
        public string ID0 { get; set; }
        public string ID1 { get; set; }
        public string ID2 { get; set; }
        public string Value { get; set; }
    }
    class RootObject
    {
        public string key;
        public List<upDate> value { get; set; }
        //public List<Employees> dataSome1 { get; set; }
        //public List<Employees> dataSome2 { get; set; }
        //public List<Manager> manager { get; set; }
    }
    
        public class normalData
        {
            public DateTime DateTime;
            public float Val;
            public string Ann;
            public float PH;
            public double 前液位;
            public float 后液位;
            public float 瞬时流量;
        }
       
    
}
