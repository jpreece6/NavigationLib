using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NavigationLib.MessageSystem;

namespace NavigationLib
{
    

    public partial class Page : UserControl, INotifyPropertyChanged
    {
        public delegate void ShowHarness(object sender, EventArgs e);
        public event ShowHarness OnShow;

        public delegate void LeaveHarness(object sender, EventArgs e);
        public new event LeaveHarness OnLeave;

        public delegate void MessageHandler(object sender, MessageEventArgs e);
        public event MessageHandler MessageReceived;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseController Controller { get; set; }

        public PageSizeModeDef PageSizeMode { get; set; }

        public enum PageSizeModeDef
        {
            Manual,
            Auto
        }

        public Page()
        {

        }

        public Page(BaseController pageController)
        {
            Controller = pageController;
        }

        public new virtual void Show()
        {
            if (PageSizeMode == PageSizeModeDef.Auto)
                this.Size = Parent.Size;

            OnShow?.Invoke(this, new EventArgs());
        }

        public new virtual void Leave()
        {
            OnLeave?.Invoke(this, new EventArgs());
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        internal void ReceiveMessage(string key, object data)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(key, data));
        }
    }
}
