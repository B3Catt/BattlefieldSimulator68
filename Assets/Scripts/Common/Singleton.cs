using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Singleton<T>
    {
        private static readonly T instance = Activator.CreateInstance<T>();
    }
}
