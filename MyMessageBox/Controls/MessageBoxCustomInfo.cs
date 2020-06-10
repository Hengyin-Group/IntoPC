using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MyMessageBox.Controls
{
    /// <summary>
    /// MIV.Bus.IEMS.MessageBox 自定义信息结构.
    /// </summary>
    public struct MessageBoxCustomInfo
    {
        #region private fields
        // 一下布尔值表示 那些属性为有效的. 在其有效时 才在MessageBox中赋值,否则 继续使用默认提供的值.
        private bool isBackgroundChanged;
        private bool isTitleForegroundChanged;
        private bool isForegroundChanged;
        private bool isBorderBrushChanged;
        private bool isBorderThicknessChanged;

        private Brush mb_background;
        private Brush mb_title_foreground;
        private Brush mb_foreground;
        private Brush mb_borderbrush;
        private Thickness mb_borderthickness;

        #endregion // private fields

        #region public properties

        public bool IsBackgroundChanged
        {
            get { return isBackgroundChanged; }
        }
        public bool IsTitleForegroundChanged
        {
            get { return isTitleForegroundChanged; }
        }
        public bool IsForegroundChanged
        {
            get { return isForegroundChanged; }
        }
        public bool IsBorderBrushChanged
        {
            get { return isBorderBrushChanged; }
        }
        public bool IsBorderThicknessChanged
        {
            get { return isBorderThicknessChanged; }
        }


        public Brush MB_Background
        {
            get { return mb_background; }
            set
            {
                mb_background = value;
                isBackgroundChanged = true;
            }
        }
        public Brush MB_Title_Foreground
        {
            get { return mb_title_foreground; }
            set
            {
                mb_title_foreground = value;
                isTitleForegroundChanged = true;
            }
        }
        public Brush MB_Foreground
        {
            get { return mb_foreground; }
            set
            {
                mb_foreground = value;
                isForegroundChanged = true;
            }
        }
        public Brush MB_Borderbrush
        {
            get { return mb_borderbrush; }
            set
            {
                mb_borderbrush = value;
                isBorderBrushChanged = true;
            }
        }
        public Thickness MB_BorderThickness
        {
            get { return mb_borderthickness; }
            set
            {
                mb_borderthickness = value;
                isBorderThicknessChanged = true;
            }
        }

        #endregion // public properties




    }
}
