using System;

namespace BattlefieldSimulator
{
    public class ArmType : DataModel
    {
        public string _name { get; set; }
        public string _auther { get; set; }
        public string _updateby { get; set; }
        public string _information { get; set; }
        public bool _isable { get; set; }
        public TimeSpan _createtime { get; set; }
        public TimeSpan _updatetime { get; set; }
        public int _speed { get; set; }
        public int _attack_power { get; set; }
        public int _attack_distance { get; set; }
        public int _defense { get; set; }
    }
}
