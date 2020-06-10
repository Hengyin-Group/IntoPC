using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace IntoApp.Command
{
    public class MyEventCommand : TriggerAction<DependencyObject>
    {
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty,value);}
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MyEventCommand), new PropertyMetadata(null));

        public object CommandParateter
        {
            get { return GetValue(CommandParateterProperty); }
            set { SetValue(CommandParateterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParateter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParateterProperty =
            DependencyProperty.Register("CommandParateter", typeof(object), typeof(MyEventCommand), new PropertyMetadata(null));

        protected override void Invoke(object parameter)
        {
            if (CommandParateter!=null)
            {
                parameter = CommandParateter;
            }
            var cmd = Command;
            if (cmd!=null)
            {
                cmd.Execute(parameter);
            }
        }
    }
}
