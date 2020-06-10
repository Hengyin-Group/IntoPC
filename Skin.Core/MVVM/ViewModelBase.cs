using System.ComponentModel;

namespace Skin.Core.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region UI更新接口
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region 是否正在加载
        private bool isLoad;

        /// <summary>
        /// 是否加载
        /// </summary>
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                isLoad = value;
                RaisePropertyChanged(nameof(IsLoad));
            }
        }
        #endregion

        #region 是否需要刷新
        private bool update;
        /// <summary>
        /// 刷新
        /// </summary>
        public bool Update
        {
            get { return update; }
            set
            {
                update = value;
                RaisePropertyChanged(nameof(Update));
            }
        }
        #endregion
    }
}
