using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Command
{
    public interface IDefaultCommand
    {
        event EventHandler CanExecuteChanged;
        bool CanExecute(object sender, object parameter);
        void Execute(object sender, object parameter);
    }
}
