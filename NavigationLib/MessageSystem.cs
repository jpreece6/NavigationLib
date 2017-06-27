using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationLib
{
    public class MessageSystem
    {


        private Dictionary<Type, List<WeakAction>> _recipients;

        public void Register<TMessage>(Page pg, Action<TMessage> action)
        {
            
        }
        /*
        public void BroadcastMessage(string key, object data)
        {
            foreach(var itmPage in _pages)
            {
                itmPage.Value.ReceiveMessage(key, data);
            }
        }

        public void SendMessage(string pageKey, string key, object data)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist");

            var page = _pages[pageKey];
            page.ReceiveMessage(key, data);
        }
        */
        public class MessageEventArgs : EventArgs
        {
            public string Key { get; }
            public object Data { get; }

            public MessageEventArgs(string key, object data) : base()
            {
                Key = key;
                Data = data;
            }
        }

        public class WeakAction
        {
            public string PageKey { get; set; }
            public Page Recepient { get; set; }
            public Action MessageAction { get; set; }
        }
    }
}
