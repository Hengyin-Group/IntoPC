using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MyMessageBox.Controls
{
    public class ShowLoading : Control
    {
        public static event EventHandler Cancel;

        public ShowLoading()
        {
            this.Loaded += ControlLoadDecorator_Loaded;
        }

        private void ControlLoadDecorator_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as Panel;
        }

        private void ShowAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Visibility = Visibility.Visible;
            }
            else
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (adornerLayer != null)
                {
                    var parent = this.Parent as Panel;
                    this.adorner = new LoadingAdorner(parent);
                    this.adorner.Cancel += (s1, e1) => { if (Cancel != null) { Cancel(s1, e1); } };
                    adornerLayer.Add(this.adorner);
                }
            }
        }

        private void HideAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Visibility = Visibility.Hidden;
            }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static void OnIsBusyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ShowLoading;
            bool showDecorator = (bool)e.NewValue;

            if (showDecorator)
            {
                self.ShowAdorner();
            }
            else
            {
                self.HideAdorner();
            }
        }

        public static void OnTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ShowLoading;
            var msg = e.NewValue.ToString();
            self.adorner.SetMessage(msg);
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(ShowLoading), new UIPropertyMetadata(false, OnIsBusyPropertyChangedCallback));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ShowLoading), new UIPropertyMetadata("", OnTextPropertyChangedCallback));


        private LoadingAdorner adorner;
    }

    public class LoadingAdorner : Adorner
    {
        public event EventHandler Cancel;

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public LoadingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.IsHitTestVisible = true;

            this.chrome = new LoadingChrome();
            chrome.DataContext = adornedElement;
            this.AddVisualChild(chrome);
        }



        public void FireCancel()
        {
            if (Cancel != null) { Cancel(this, EventArgs.Empty); }
        }

        public void SetMessage(string msg)
        {
            this.chrome.Message = msg;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.chrome;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        private LoadingChrome chrome;
    }

    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Text", Type = typeof(Label))]
    public class LoadingChrome : Control
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var cancelButton = GetTemplateChild("PART_Cancel") as Button;
            if (cancelButton != null)
            {
                cancelButton.Click += new RoutedEventHandler(cancelButton_Click);
            }
            var msgText = GetTemplateChild("PART_Text") as Label;
            if (msgText != null)
            {
                msgText.Content = this.Message;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this) as LoadingAdorner;
            parent.FireCancel();
        }

        static LoadingChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingChrome), new FrameworkPropertyMetadata(typeof(LoadingChrome)));
        }

        public static void OnAllowCancelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as LoadingChrome;
            bool allow = (bool)e.NewValue;
            var cancelButton = self.GetTemplateChild("PART_Cancel") as Button;

            if (cancelButton != null)
            {
                if (allow)
                {
                    cancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    cancelButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(LoadingChrome), new UIPropertyMetadata(string.Empty, OnMessagePropertyChangedCallback));

        public static void OnMessagePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as LoadingChrome;
            var msgText = self.GetTemplateChild("PART_Text") as Label;
            if (msgText != null)
            {
                msgText.Content = e.NewValue.ToString();
            }
        }

        public bool AllowCancel
        {
            get { return (bool)GetValue(AllowCancelProperty); }
            set { SetValue(AllowCancelProperty, value); }
        }

        public static readonly DependencyProperty AllowCancelProperty =
            DependencyProperty.Register("AllowCancel", typeof(bool), typeof(LoadingChrome), new UIPropertyMetadata(false, OnAllowCancelPropertyChangedCallback));
    }
}
