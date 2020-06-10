using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IntoApp.AutoUpdate.View;

namespace IntoApp.AutoUpdate.utils
{
    public class InformHelper
    {
        public void InformShowDialog()
        {
            InformUpdate informUpdate=new InformUpdate();
            bool bo=(bool)informUpdate.ShowDialog();
            if (bo)
            {
                new MainWindow().Show();
            }
            else
            {
                Application.Current.Shutdown();
            }

        }
    }
}
