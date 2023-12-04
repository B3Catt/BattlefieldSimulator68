using System;

namespace BattlefieldSimulator
{
    /// <summary>
    /// all models with corresponding table must inherit DataModel
    /// </summary>
    public class DataModel : BaseModel
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public string _auther { get; set; }
        public string _updateby { get; set; }
        public string _information { get; set; }
        public bool _isable { get; set; }
        public TimeSpan _createtime { get; set; }
        public TimeSpan _updatetime { get; set; }
    }
}
