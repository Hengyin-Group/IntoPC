using MyMessageBox.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyMessageBox.Controls
{
    internal sealed class MessageBoxModule : Window
    {
        static MessageBoxModule()
        {
            // 指定自定义 控件 搜索的 样式模板类型
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxModule), new FrameworkPropertyMetadata(typeof(MessageBoxModule)));
        }

        public MessageBoxModule()
        {
            try
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.AllowsTransparency = true;
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.ShowInTaskbar = false;
                this.Topmost = true;
                this.MouseLeftButtonDown += (o, e) => { this.DragMove(); };

                // 为MessageBoxModule 挂载资源文件,为了使用资源中的一些 样式定义. 注意 Uri中路径的写法.
                Resources.Source = new Uri(@"MyMessageBox;component/Styles/Generic.xaml", UriKind.Relative);
                if (Resources.Contains("MessageBoxButtonStyle"))
                {
                    MessageBoxModule.SetDefaultCtorButtonStyle(Resources["MessageBoxButtonStyle"] as Style);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

        }

        #region 属性

        public new string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }

        // 自定义响应按钮的集合
        public IList<Button> CtrlButtonCollection
        {
            get { return (IList<Button>)GetValue(CtrlButtonCollectionProperty); }
            set { SetValue(CtrlButtonCollectionProperty, value); }
        }

        public static Style CTRL_BUTTON_STYLE { get; private set; }
        public static MessageBoxCustomInfo MB_CUSTOMINFO { get; private set; }
        public static bool B_USED_CUSTOM_BRUSHES { get; private set; }

        public new static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxModule), new PropertyMetadata("标题"));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxModule), new PropertyMetadata(""));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(MessageBoxModule), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255,255,255))));

        public static readonly DependencyProperty CtrlButtonCollectionProperty =
            DependencyProperty.Register("CtrlButtonCollection", typeof(IList<Button>), typeof(MessageBoxModule), new PropertyMetadata(new List<Button>() /*{ new Button() { Content = "确定" }, new Button() { Content = "取消" } }*/ ));

        #endregion

        #region show方法

        public static MessageBoxResult Show(string messageBoxText)
        {
           return Show(null, messageBoxText, null, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return Show(null, messageBoxText, caption, MessageBoxButton.OK);
        }
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return Show(owner, messageBoxText, null, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(null, messageBoxText, caption, button);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            var mbox = new MessageBoxModule();
            mbox.Message = messageBoxText;
            mbox.Title = caption;
            mbox.Owner = owner;
            // 这个方法是检测本次弹出的消息框是否使用了自定义的颜色配置,而不是使用默认提供的颜色配置.后面再说,这里无视.
            IsUseCustomInfoDefine(ref mbox);

            if (owner != null)
            {
                mbox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            //这里的分支语句提供了传入MessageBoxButton枚举的具体响应实现. 其中CreateCtrlButton_ResultTrue,CreateCtrlButton_ResultFalse
            // 等方法是创建一个按钮 并默认了按钮的响应 和Show方法完成后返回的结果. // 后面在看.

            switch (button)
            {
                case MessageBoxButton.OKCancel:

                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultTrue(mbox, "确定"));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultFalse(mbox, "取消"));
                    break;
                //break;
                case MessageBoxButton.YesNo:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultTrue(mbox, "是"));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultFalse(mbox, "否"));

                    break;
                case MessageBoxButton.YesNoCancel:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultTrue(mbox, "是"));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultFalse(mbox, "否"));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultFalse(mbox, "取消"));
                    break;
                case MessageBoxButton.OK:
                default:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButton_ResultTrue(mbox, "确定"));
                    break;
            }
            var result = mbox.ShowDialog();  // 本行代码是消息框弹出的核心.ShowDialog方法会打开一个模态对话框.等待返回结果.这里的结果不是MessageBoxResult枚举而是可空类型的布尔值(bool?) true对应着MessageBoxResult.Yes ,MessageBoxResult.OK的值.false对应着MessageBoxResult.Cancel,MessageBoxResult.No 的值.
                                             // 了解这些我们就可以对ShowDialog方法返回值做MessageBoxResult转换了.

            switch (button)
            {

                //break;
                case MessageBoxButton.OKCancel:
                    {
                        mbox.CtrlButtonCollection.Clear();
                        return result == true ? MessageBoxResult.OK
                            : result == false ? MessageBoxResult.Cancel :
                            MessageBoxResult.None;
                    }
                //break;
                case MessageBoxButton.YesNo:
                    {
                        mbox.CtrlButtonCollection.Clear();
                        return result == true ? MessageBoxResult.Yes : MessageBoxResult.No;
                    }
                //break;
                case MessageBoxButton.YesNoCancel:
                    {
                        mbox.CtrlButtonCollection.Clear();
                        return result == true ? MessageBoxResult.Yes
                            : result == false ? MessageBoxResult.No :
                            MessageBoxResult.Cancel;
                    }

                case MessageBoxButton.OK:
                default:
                    {
                        mbox.CtrlButtonCollection.Clear();
                        return result == true ? MessageBoxResult.OK : MessageBoxResult.None;
                    }
            }
        }

        //特色
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, IList<MessageBoxButtonInfo> ctrlButtons)
        {
            var mbox = new MessageBoxModule();
            mbox.Message = messageBoxText;
            mbox.Title = caption;
            mbox.Owner = owner;

            IsUseCustomInfoDefine(ref mbox); // 同上,检测是否使用自定义主题颜色的配置.

            if (owner != null)
            {
                mbox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            if (null != ctrlButtons && ctrlButtons.Count > 0)
            {
                foreach (var btnInfo in ctrlButtons)
                {
                    switch (btnInfo.Result)
                    {
                        case MessageBoxResult.Cancel:
                        case MessageBoxResult.No:
                            {
                                var btn = CreateCtrlButton_ResultFalse(mbox, btnInfo.ContentText);
                                btn.Command = new MyCommand(btnInfo.Action);  // 为按钮关联响应动作. 这里我把Action封装为了命令.MessageBoxCommand为自定义的命令.
                                mbox.CtrlButtonCollection.Add(btn);
                            }
                            break;
                        case MessageBoxResult.None:
                            {
                                var btn = CreateCtrlButton_ResultNull(mbox, btnInfo.ContentText);
                                btn.Command = new MyCommand(btnInfo.Action);
                                mbox.CtrlButtonCollection.Add(btn);
                            }
                            break;
                        case MessageBoxResult.OK:
                        case MessageBoxResult.Yes:
                        default:
                            {
                                var btn = CreateCtrlButton_ResultTrue(mbox, btnInfo.ContentText);
                                btn.Command = new MyCommand(btnInfo.Action);
                                mbox.CtrlButtonCollection.Add(btn);
                            }
                            break;
                    }
                }

                var result = mbox.ShowDialog(); //同上一个Show方法.这里调用会显示一个模态窗口.
                return MessageBoxResult.None;//为啥我说MessageBoxButtonInfo类中的Result没用了,因为这里我始终返回None了.返回结果的目的是为了根据不同的结果做不同的处理,而这里的Action已经对其作出了响应.所以返回结果的用处不大.
            }
            else
            {
                return Show(owner, messageBoxText, caption, MessageBoxButton.OK);
            }


        }


        #endregion

        #region 三个对话框返回值

        private static Button CreateCtrlButton_ResultTrue(MessageBoxModule mbox, string content)
        {
            return CreateCtrlButton(content, new RoutedEventHandler((o, e) =>
            {
                try
                {   // 这里的DialogResult = true 赋值,表示着 Show方法的返回值可能为 Yes OK的值. 
                    // 所以这个方法支持了MessageBoxButton.OKCancel,MessageBoxButton.YesNo,MessageBoxButton.OK ,MessageBoxButton.YesNoCancel
                    // 枚举中的Yes Ok部分的支持. 同理另外2个方法类似.

                    mbox.DialogResult = true;
                    mbox.Close();
                }
                catch { }
            }));
        }

        private static Button CreateCtrlButton_ResultFalse(MessageBoxModule mbox, string content)
        {
            return CreateCtrlButton(content, new RoutedEventHandler((o, e) =>
            {
                try
                {
                    mbox.DialogResult = false;
                    mbox.Close();
                }
                catch { }
            }));
        }

        private static Button CreateCtrlButton_ResultNull(MessageBoxModule mbox, string content)
        {
            return CreateCtrlButton(content, new RoutedEventHandler((o, e) =>
            {
                try
                {
                    mbox.DialogResult = null;
                    mbox.Close();
                }
                catch { }
            }));
        }

        private static Button CreateCtrlButton(string content, RoutedEventHandler routedEventHandler)
        {
            Button btn=new Button
            {
                Content = content,
                Style = CTRL_BUTTON_STYLE
            };
            btn.Click+=new RoutedEventHandler(routedEventHandler);
            return btn;
        }

        #endregion

        #region 判断是否为自定义属性

        private static void IsUseCustomInfoDefine(ref MessageBoxModule mbox)
        {
            // 判断每一个属性,有效时才赋值 否则忽略.
            if (B_USED_CUSTOM_BRUSHES && null != mbox)
            {
                if (MB_CUSTOMINFO.IsBackgroundChanged)
                {
                    mbox.Background = MB_CUSTOMINFO.MB_Background;
                }
                if (MB_CUSTOMINFO.IsBorderBrushChanged)
                {
                    mbox.BorderBrush = MB_CUSTOMINFO.MB_Borderbrush;
                }
                if (MB_CUSTOMINFO.IsBorderThicknessChanged)
                {
                    mbox.BorderThickness = MB_CUSTOMINFO.MB_BorderThickness;
                }
                if (MB_CUSTOMINFO.IsForegroundChanged)
                {
                    mbox.Foreground = MB_CUSTOMINFO.MB_Foreground;
                }
                if (MB_CUSTOMINFO.IsTitleForegroundChanged)
                {
                    mbox.TitleForeground = MB_CUSTOMINFO.MB_Title_Foreground;
                }
            }
        }

        #endregion

        #region 

        // Public Static Functions
        // 设置 消息框中响应按钮的样式. 静态存储.针对所有调用的消息框.
        public static void SetDefaultCtorButtonStyle(Style buttonStyle)
        {
            CTRL_BUTTON_STYLE = buttonStyle;
        }
        // 设置 消息框中的 标题 文本 边框 前景 背景等 以配合我们的应用程序主题风格.
        // 其中 MessageBoxCustomInfo 为自定义的结构 存储以上的信息.

        public static void SetMessageBoxCustomDefine(MessageBoxCustomInfo mbCustomIf)
        {
            if (!default(MessageBoxCustomInfo).Equals(mbCustomIf))
            {
                MessageBoxModule.MB_CUSTOMINFO = mbCustomIf;
                MessageBoxModule.B_USED_CUSTOM_BRUSHES = true;
            }
            else
            {
                MessageBoxModule.MB_CUSTOMINFO = default(MessageBoxCustomInfo);
                MessageBoxModule.B_USED_CUSTOM_BRUSHES = false;
            }
        }

        public static void ResetMessageBoxCustomDefine()
        {
            CTRL_BUTTON_STYLE = Button.StyleProperty.DefaultMetadata.DefaultValue as Style;
            MB_CUSTOMINFO = default(MessageBoxCustomInfo);
            B_USED_CUSTOM_BRUSHES = false;
        }

        // Show MessageBox Functions

        #endregion


    }
}
