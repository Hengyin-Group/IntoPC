using IntoApp.Core.WIN32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace IntoApp.Controls
{
    public partial class ShadowWindow : Window, INotifyPropertyChanged
    {
        #region UI更新接口
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private double _WindowShadowSize = 10.0;
        [Description("窗体阴影大小"), Category("Skin")]
        public double WindowShadowSize
        {
            get
            {
                return _WindowShadowSize;
            }

            set
            {
                _WindowShadowSize = value;
                OnPropertyChanged("WindowShadowSize");
            }
        }

        private Color _WindowShadowColor = Color.FromArgb(255, 200, 200, 200);
        [Description("窗体阴影颜色"), Category("Skin")]
        public Color WindowShadowColor
        {
            get
            {
                return _WindowShadowColor;
            }

            set
            {
                _WindowShadowColor = value;
                OnPropertyChanged("WindowShadowColor");
            }
        }


        private Brush _WindowShadowBackColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        [Description("窗体阴影背景颜色"), Category("Skin")]
        public Brush WindowShadowBackColor
        {
            get
            {
                return _WindowShadowBackColor;
            }

            set
            {
                _WindowShadowBackColor = value;
                OnPropertyChanged("WindowShadowBackColor");
            }
        }

        public ShadowWindow()
        {
            InitializeComponent();
            DataContext = this;
            SourceInitialized += MainWindow_SourceInitialized;
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr Handle = new WindowInteropHelper(this).Handle;
            int exStyle = (int)NativeMethods.GetWindowLong(Handle, -20);
            exStyle = NativeConstants.WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLong(Handle, -20, (IntPtr)exStyle);
        }
    }
}
