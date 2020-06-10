﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntoApp.AutoUpdate.utils
{
    interface INavigate:INavigationService
    {
        void GoBack();
        void NavigateTo(string pageKey);
        void NavigateTo(string pageKey, object parameter);
        void FrameNavigateTo(string pageKey, Frame frame);
        void FrameNavigateTo(string pageKey, object parameter, Frame frame);
    }
    public class NavigationService : ViewModelBase, INavigate
    {
        private readonly Dictionary<string, Uri> _pagesByKey;
        private readonly List<string> _historic;
        private string _currentPageKey;

        #region Properties
        public string CurrentPageKey
        {
            get
            {
                return _currentPageKey;
            }

            private set
            {
                Set(() => CurrentPageKey, ref _currentPageKey, value);
            }
        }
        public object Parameter { get; private set; }
        #endregion

        #region Ctors and Methods
        public NavigationService()
        {
            _pagesByKey = new Dictionary<string, Uri>();
            _historic = new List<string>();
        }
        public void GoBack()
        {
            if (_historic.Count > 1)
            {
                _historic.RemoveAt(_historic.Count - 1);

                NavigateTo(_historic.Last(), "Back");
            }
        }
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, "Next");
        }

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            #region MyRegion

            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), "pageKey");
                }

                var frame = GetDescendantFromName(Application.Current.MainWindow, "Frame") as Frame;

                if (frame != null)
                {
                    frame.Source = _pagesByKey[pageKey];
                }
                Parameter = parameter;
                if (parameter.ToString().Equals("Next"))
                {
                    _historic.Add(pageKey);
                }
                CurrentPageKey = pageKey;
            }

            #endregion


        }

        public void Configure(string key, Uri pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    _pagesByKey[key] = pageType;
                }
                else
                {
                    _pagesByKey.Add(key, pageType);
                }
            }
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }
            return null;
        }

        public void FrameNavigateTo(string pageKey, Frame frame)
        {
            FrameNavigateTo(pageKey, "Next", frame);
        }
        public virtual void FrameNavigateTo(string pageKey, object parameter, Frame frame)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), "pageKey");
                }
                if (frame != null)
                {
                    frame.Source = _pagesByKey[pageKey];
                }
                Parameter = parameter;
                if (parameter.ToString().Equals("Next"))
                {
                    _historic.Add(pageKey);
                }
                CurrentPageKey = pageKey;

            }
        }



        /// <summary>
        /// 指定frame对象跳转加载页
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="frame"></param>


        #endregion
    }
}
