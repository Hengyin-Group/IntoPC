using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyMessageBox.Controls
{
    /// <summary>
    /// 显示消息框.
    /// </summary>
    public sealed class MessageBox
    {
        #region ctors

        static MessageBox()
        {
        }
        private MessageBox()
        {
        }

        #endregion // ctors

        #region custom settings

        /// <summary>
        /// 设置 MIV.Bus.IEMS.MessageBox 的按钮样式.
        /// </summary>
        /// <param name="buttonStyle"></param>
        public static void SetDefaultCtorButtonStyle(Style buttonStyle)
        {
            MessageBoxModule.SetDefaultCtorButtonStyle(buttonStyle);
        }

        /// <summary>
        /// 设置 MIV.Bus.IEMS.MessageBox 的一些自定义信息.
        /// </summary>
        /// <param name="mbCustomIf">MIV.Bus.IEMS.MessageBox 自定义信息结构</param>
        public static void SetMessageBoxCustomDefine(MessageBoxCustomInfo mbCustomIf)
        {
            MessageBoxModule.SetMessageBoxCustomDefine(mbCustomIf);
        }

        public static void ResetMessageBoxCustomDefine()
        {
            MessageBoxModule.ResetMessageBoxCustomDefine();
        }

        #endregion // custom settings

        #region Show functions

        /// <summary>
        /// 显示一个消息框，该消息框包含消息并返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText)
        {
            return MessageBoxModule.Show(messageBoxText);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息和标题栏标题，并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <param name="caption"> 一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return MessageBoxModule.Show(messageBoxText, caption);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题和按钮，并且返回结果。
        /// </summary>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <param name="caption"> 一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <returns>一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBoxModule.Show(messageBoxText, caption, button);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息并返回结果。
        /// </summary>
        /// <param name="owner">一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <returns> 一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return MessageBoxModule.Show(owner, messageBoxText);
        }

        /// <summary>
        ///  在指定窗口的前面显示消息框。该消息框显示消息、标题栏标题和按钮，并且返回结果。
        /// </summary>
        /// <param name="owner"> 一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <param name="caption"> 一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="button">一个 System.Windows.MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮。</param>
        /// <returns> 一个 System.Windows.MessageBoxResult 值，用于指定用户单击了哪个消息框按钮。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, button);
        }

        /// <summary>
        /// 在指定窗口的前面显示消息框。该消息框显示消息、标题栏标题和按钮，并且支持自定义按钮和动作。
        /// </summary>
        /// <param name="owner"> 一个 System.Windows.Window，表示消息框的所有者窗口。</param>
        /// <param name="messageBoxText">一个 System.String，用于指定要显示的文本。</param>
        /// <param name="caption"> 一个 System.String，用于指定要显示的标题栏标题。</param>
        /// <param name="ctrlButtons">一组自定义的按钮和响应动作。</param>
        /// <returns>始终为 MessageBoxResult.None ,返回结果在此无意义。</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, IList<MessageBoxButtonInfo> ctrlButtons)
        {
            return MessageBoxModule.Show(owner, messageBoxText, caption, ctrlButtons);
        }

        #endregion // Show functions
    }
}
