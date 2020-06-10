using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyMessageBox.Controls
{
    /// <summary>
    /// Vito.Csharp.Wpf.Controls.MessageBoxModule 组件中 MessageBoxButton 的自定义设置信息.
    /// </summary>
    public class MessageBoxButtonInfo
    {
        #region fields

        private string _contentText = "";
        private MessageBoxResult _result = MessageBoxResult.OK;
        private Action<object> _action = null;

        #endregion // fields

        #region ctor

        /// <summary>
        /// 初始化 MIV.Bus.IEMS.MessageBox 自定义按钮的基本信息.
        /// </summary>
        /// <param name="contentText">按钮的文本内容</param>
        /// <param name="result">按钮响应的返回结果</param>
        /// <param name="action">按钮的响应动作</param>
        public MessageBoxButtonInfo(string contentText, MessageBoxResult result, Action<object> action)
        {
            this._contentText = contentText;
            this._result = result;
            if (null != action)
            {
                this._action = action;
            }
            else
            {
                this._action = new Action<object>((o) =>
                {

                });
            }
        }

        #endregion // ctor

        #region Readonly Properties

        /// <summary>
        /// 获取 MIV.Bus.IEMS.MessageBox 按钮的文本内容.
        /// </summary>
        public string ContentText
        {
            get { return _contentText; }
        }

        /// <summary>
        /// 获取 MIV.Bus.IEMS.MessageBox 按钮响应的返回结果.
        /// </summary>
        public MessageBoxResult Result
        {
            get { return _result; }
        }

        /// <summary>
        /// 获取 MIV.Bus.IEMS.MessageBox 按钮的响应动作.
        /// </summary>
        public Action<object> Action
        {
            get { return _action; }
        }

        #endregion // Readonly Properties
    }
}
