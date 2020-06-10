using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using IntoApp.ViewModel.Base;
using Skin.WPF.Command;

namespace IntoApp.ViewModel.ContentViewModel.ServerViewModel
{
    public class WinFileUploadViewModel:LocalTrayViewModelBase
    {
        //public MyCommand<object[]> CheckBoxIsChecked
        //{
        //    get
        //    {
        //        return new MyCommand<object[]>(x => Checked(x));
        //    }
        //}
        //public void Checked(object[] obj)
        //{
        //    CheckBox checkBox = obj[0] as CheckBox;
        //    Window window = obj[1] as Window;
        //    bool bo = checkBox.IsChecked == true;
        //    if (bo)
        //    {
        //        window.Topmost = true;
        //        Tooltip = "取消置顶";
        //    }
        //    else
        //    {
        //        window.Topmost = false;
        //        Tooltip = "置顶";
        //    }
        //    //MessageBox.Show(bo.ToString());
        //}

        //private string _tooltip = "置顶";
        //public string Tooltip
        //{
        //    get { return _tooltip; }
        //    set
        //    {
        //        _tooltip = value;
        //        RaisePropertyChanged("Tooltip");
        //    }
        //}

        public WinFileUploadViewModel()
        {
            DrapCommand=new MyCommand<DragEventArgs>(x=>DropDown(x));
        }


        public MyCommand<DragEventArgs> DrapCommand { get; set; }

        public void DropDown(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                int count = ((Array)e.Data.GetData(DataFormats.FileDrop)).Length;
                for (int i = 0; i < count; i++)
                {
                    //MessageBox.Show(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(i).ToString());
                    //FileName.Add(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(i).ToString());
                }
            }
        }

    }
}
