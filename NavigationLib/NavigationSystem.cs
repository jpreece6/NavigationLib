using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationLib
{
    public static class NavigationSystem
    {
        private static Dictionary<string, Page> _pages = new Dictionary<string, Page>();
        private static FixedSizedStack<Page> _historic = new FixedSizedStack<Page>(10);
        public static Page CurrentPage { get; set; }
        public static Page HomePage { get; set; }

        public delegate void NavigationHandler(NavigationEvent e);
        public static event NavigationHandler OnNavigation;

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="pageKey">Key name of the page to navigate to</param>
        public static void NavigateTo(string pageKey)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist");

            if (CurrentPage != null)
            {
                CurrentPage.Leave();
                _historic.Enqueue(CurrentPage);
            }

            CurrentPage = _pages[pageKey];
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));

            CurrentPage.Show();
        }

        public static void NavigateTo(string pageKey, object data)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist");

            if (CurrentPage != null)
            {
                CurrentPage.Leave();
                _historic.Enqueue(CurrentPage);
            }

            CurrentPage = _pages[pageKey];
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));

            CurrentPage.Show();
        }

        /// <summary>
        /// Returns to the previous page.
        /// </summary>
        public static void GoBack()
        {
            if (_historic.Count == 0)
                return;

            CurrentPage.Leave();
            CurrentPage = _historic.Dequeue();
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));

            CurrentPage.Show();
        }

        /// <summary>
        /// Returns to the home page if set
        /// </summary>
        public static void GoHome()
        {
            if (HomePage == null)
                throw new ArgumentException("Home page not set");

            if (CurrentPage != null)
            {
                CurrentPage.Leave();
                _historic.Enqueue(CurrentPage);
            }

            CurrentPage = HomePage;
            OnNavigation?.Invoke(new NavigationEvent(CurrentPage));
            CurrentPage.Show();
        }

        /// <summary>
        /// Sets the application home page
        /// </summary>
        /// <param name="navPage"></param>
        public static void SetHome(string pageKey)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist");

            HomePage = _pages[pageKey];
        }

        /// <summary>
        /// Registers a new page with the system
        /// </summary>
        /// <param name="pageKey">Key name of the page</param>
        /// <param name="navPage">Page instance to register</param>
        public static void Register(string pageKey, Page navPage)
        {
            if (_pages.ContainsKey(pageKey))
                throw new ArgumentException("Page already exists.");

            if (_pages.Count == 0 && HomePage == null)
                HomePage = navPage;

            _pages.Add(pageKey, navPage);
        }

        /// <summary>
        /// Unregisters a selected page
        /// </summary>
        /// <param name="pageKey">Key of the page to unregister</param>
        public static void UnRegister(string pageKey)
        {
            if (_pages.ContainsKey(pageKey) == false)
                throw new ArgumentException("Page does not exist.");

            _pages.Remove(pageKey);
        }

        public static Dictionary<string, Page> GetPages()
        {
            return _pages;
        }

        public static FixedSizedStack<Page> GetHistory()
        {
            return _historic;
        }
    }

    public class NavigationEvent : EventArgs
    {
        public Page CurrentPage { get; } 

        public NavigationEvent(Page currentPage) : base()
        {
            CurrentPage = currentPage;
        }
    }
}
