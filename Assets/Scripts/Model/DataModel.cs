using System;

namespace BattlefieldSimulator
{
    /// <summary>
    /// all models with corresponding table must inherit DataModel
    /// </summary>
    public class DataModel : BaseModel
    {
        public int _id { get; set; }
    }
}
