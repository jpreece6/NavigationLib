using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationLib
{
    public static class MessageSystem
    {

        private static Dictionary<string, Page> _pages
        {
            get
            {
                return NavigationSystem.GetPages();
            }
        }

        public static void BroadcastMessage(string key, object data)
        {
            foreach(var itmPage in _pages)
            {
                itmPage.Value.ReceiveMessage(key, data);
            }
        }

        public static void SendMessage(string pageKey, string key, object data)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist");

            var page = _pages[pageKey];
            page.ReceiveMessage(key, data);
        }

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
    }
}
