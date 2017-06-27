using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NavigationLib
{
    public class NavigationSystem
    {
        private Dictionary<string, Page> _pages = new Dictionary<string, Page>();
        private FixedSizedStack<Page> _historic = new FixedSizedStack<Page>(10);
        public Page CurrentPage { get; set; }
        public Page HomePage { get; set; }
        public string ParentForm { get; private set; }

        public delegate void NavigationHandler(NavigationEvent e);
        public event NavigationHandler OnNavigation;

        private static NavigationSystem _default;

        public static NavigationSystem Default
        {
            get
            {
                return _default ?? (_default = new NavigationSystem());
            }
        }

        public NavigationSystem() { }
        public NavigationSystem(Form frm)
        {
            SetParentForm(frm);
        }

        /// <summary>
        /// Registers the window to contain the views
        /// </summary>
        /// <param name="frm"></param>
        public void SetParentForm(Form frm)
        {
            ParentForm = frm.Name;
        }

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="pageKey">Key name of the page to navigate to</param>
        public void NavigateTo(string pageKey, object data = null)
        {
            string key = pageKey.ToLower();
            if (_pages.ContainsKey(key) == false)
                throw new ArgumentException("Page does not exist");

            if (CurrentPage != null)
            {
                CurrentPage.Leave();
                _historic.Enqueue(CurrentPage);
            }

            CurrentPage = _pages[key];
            UpdateForm();

            OnNavigation?.Invoke(new NavigationEvent(CurrentPage, data));

            CurrentPage.Show();
        }

        /// <summary>
        /// Returns to the previous page.
        /// </summary>
        public void GoBack()
        {
            if (_historic.Count == 0)
                return;

            CurrentPage.Leave();
            CurrentPage = _historic.Dequeue();
            UpdateForm();
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));

            CurrentPage.Show();
        }

        /// <summary>
        /// Returns to the home page if set
        /// </summary>
        public void GoHome()
        {
            if (HomePage == null)
                throw new ArgumentException("Home page not set");

            if (CurrentPage != null)
            {
                CurrentPage.Leave();
                _historic.Enqueue(CurrentPage);
            }

            CurrentPage = HomePage;
            UpdateForm();
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));
            CurrentPage.Show();
        }

        /// <summary>
        /// Sets the application home page
        /// </summary>
        /// <param name="navPage"></param>
        public void SetHome(string pageKey)
        {
            string key = pageKey.ToLower();
            if (_pages.ContainsKey(key) == false)
                throw new ArgumentException("Page does not exist");

            HomePage = _pages[key];
        }

        /// <summary>
        /// Registers a new page with the system
        /// </summary>
        /// <param name="pageKey">Key name of the page</param>
        /// <param name="navPage">Page instance to register</param>
        public void Register(string pageKey, Page navPage)
        {
            string key = pageKey.ToLower();
            if (_pages.ContainsKey(key))
                throw new ArgumentException("Page already exists.");

            if (_pages.Count == 0 && HomePage == null)
                HomePage = navPage;

            _pages.Add(key, navPage);
        }

        /// <summary>
        /// Unregisters a selected page
        /// </summary>
        /// <param name="pageKey">Key of the page to unregister</param>
        public void UnRegister(string pageKey)
        {
            string key = pageKey.ToLower();
            if (_pages.ContainsKey(key) == false)
                throw new ArgumentException("Page does not exist.");

            _pages.Remove(key);
        }

        public Dictionary<string, Page> GetPages()
        {
            return _pages;
        }

        public FixedSizedStack<Page> GetHistory()
        {
            return _historic;
        }

        /// <summary>
        /// Inserts the current page into the parent form
        /// </summary>
        private void UpdateForm()
        {
            if (ParentForm == null || ParentForm == "")
                throw new ArgumentException("A parent form must be registered first!");

            var target = Application.OpenForms[ParentForm];

            if (target.InvokeRequired)
            {
                target.Invoke(new Action(() => target.Controls.Clear()));
                target.Invoke(new Action(() => target.Controls.Add(CurrentPage)));
            }
            else
            {

                target.Controls.Clear();
                target.Controls.Add(CurrentPage);
            }
        }
    }

    public class NavigationEvent : EventArgs
    {
        public Page CurrentPage { get; } 
        public object Data { get; }

        public NavigationEvent(Page currentPage)
        {
            CurrentPage = currentPage;
        }

        public NavigationEvent(Page currentPage, object data) : this(currentPage)
        {
            Data = data;
        }
    }
}
