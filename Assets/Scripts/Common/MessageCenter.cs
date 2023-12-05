using System.Collections.Generic;

namespace BattlefieldSimulator
{
    /// <summary>
    /// the message here is different from messageView
    /// </summary>
    public class MessageCenter
    {
        /// <summary>
        /// storage of normal message
        /// </summary>
        private Dictionary<string, System.Action<object>> msgDic;

        /// <summary>
        /// storage of temp message, make deletion after invoke
        /// </summary>
        private Dictionary<string, System.Action<object>> tempMsgDic;

        /// <summary>
        /// storage of special message, in case of certain object
        /// </summary>
        private Dictionary<System.Object, Dictionary<string, System.Action<object>>> objMsgDic;

        /// <summary>
        /// 
        /// </summary>
        public MessageCenter()
        {
            msgDic = new Dictionary<string, System.Action<object>>();
            tempMsgDic = new Dictionary<string, System.Action<object>>();
            objMsgDic = new Dictionary<object, Dictionary<string, System.Action<object>>>();
        }

        /// <summary>
        /// add event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void AddEvent(string eventName, System.Action<object> callback)
        {
            if (msgDic.ContainsKey(eventName))
            {
                msgDic[eventName] += callback;
            }
            else
            {
                msgDic.Add(eventName, callback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        public void RemoveEvent(string eventName, System.Action<object> callback)
        {
            if (msgDic.ContainsKey(eventName))
            {
                msgDic[eventName] -= callback;
                if (msgDic[eventName] == null)
                {
                    msgDic.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// excute the event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="arg"></param>
        public void PostEvent(string eventName, object arg = null)
        {
            if (msgDic.ContainsKey(eventName))
            {
                msgDic[eventName].Invoke(arg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenerObj"></param>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void AddEvent(object listenerObj, string eventName, System.Action<object> callback)
        {
            if (objMsgDic.ContainsKey(listenerObj))
            {
                if (objMsgDic[listenerObj].ContainsKey(eventName))
                {
                    objMsgDic[listenerObj][eventName] += callback;
                }
                else
                {
                    objMsgDic[listenerObj].Add(eventName, callback);
                }
            }
            else
            {
                Dictionary<string, System.Action<object>> _tempDic = new Dictionary<string, System.Action<object>>
                {
                    { eventName, callback }
                };
                objMsgDic.Add(listenerObj, _tempDic);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenerObj"></param>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void RemoveEvent(object listenerObj, string eventName, System.Action<object> callback)
        {
            if (objMsgDic.ContainsKey(listenerObj) && objMsgDic[listenerObj].ContainsKey(eventName))
            {
                objMsgDic[listenerObj][eventName] -= callback;
                if (objMsgDic[listenerObj][eventName] == null)
                {
                    objMsgDic[listenerObj].Remove(eventName);
                    if (objMsgDic[listenerObj].Count == 0)
                    {
                        objMsgDic.Remove(listenerObj);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenerObj"></param>
        public void RemoveObjAllEvent(System.Object listenerObj)
        {
            if (objMsgDic.ContainsKey(listenerObj))
            {
                objMsgDic.Remove(listenerObj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenerObj"></param>
        /// <param name="eventName"></param>
        /// <param name="arg"></param>
        public void PostEvent(object listenerObj, string eventName, object arg = null)
        {
            if (objMsgDic.ContainsKey(listenerObj) && objMsgDic[listenerObj].ContainsKey(eventName))
            {
                objMsgDic[listenerObj][eventName].Invoke(arg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void AddTempEvent(string eventName, System.Action<object> callback)
        {
            if (tempMsgDic.ContainsKey(eventName))
            {
                tempMsgDic[eventName] += callback;
            }
            else
            {
                tempMsgDic.Add(eventName, callback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="arg"></param>
        public void PostTempEvent(string eventName, object arg = null)
        {
            if (tempMsgDic.ContainsKey(eventName))
            {
                tempMsgDic[eventName].Invoke(arg);
                tempMsgDic[eventName] = null;
                tempMsgDic.Remove(eventName);
            }
        }
    }
}
