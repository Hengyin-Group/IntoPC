using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntoApp.AutoUpdate.Model;

namespace IntoApp.AutoUpdate.ViewModel.Base
{
    public class DownloadViewModelBase:DownloadModel
    {
        private ObservableCollection<DownloadModel> _downloadModels;

        public ObservableCollection<DownloadModel> DownloadModels
        {
            get
            {
                if (_downloadModels == null)
                {
                    _downloadModels = new ObservableCollection<DownloadModel>();
                }
                return _downloadModels;
            }
            set
            {
                _downloadModels = value;
                RaisePropertyChanged("DownloadModels");
            }
        }
    }
}
