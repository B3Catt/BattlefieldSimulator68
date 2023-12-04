using System;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class DataModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string auther { get; set; }
        public string updateby { get; set; }
        public string information { get; set; }
        public bool isable { get; set; }
        public TimeSpan createtime { get; set; }
        public TimeSpan updatetime { get; set; }
    }
}
