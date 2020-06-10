using System.IO;
using System.Windows;
using System.Windows.Input;
using IntoApp.Printer.ViewModel;

namespace IntoApp.Printer.View
{
    /// <summary>
    /// ModifyName.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyNameWindow
    {
        private int _number;

        //public delegate void TransfDelegate(string value);
        //public event TransfDelegate TransfEvent;

        public delegate void TransfDelegate_New(string filePath, string copies, string isColor, string documentName, string pageCount);

        public event TransfDelegate_New TransfEvent_New;


        //public ModifyNameWindow(string str)
        //{
        //    InitializeComponent();
        //    ModifyNameViewModel model = new ModifyNameViewModel(str);
        //    model.TransfEvent += Vm_TransfEvent;
        //    this.DataContext = model;
        //    this.Loaded += ModifyName_Loaded;
        //}


        public ModifyNameWindow(string filePath, string copies, string isColor, string documentName, string pageCount)
        {
            InitializeComponent();
            ModifyNameViewModel model = new ModifyNameViewModel(filePath, copies, isColor, documentName, pageCount);
            model.TransfEvent_New += Vm_TransfEvent_New;
            this.DataContext = model;
            this.Loaded += ModifyName_Loaded;
        }


        void ModifyName_Loaded(object sender, RoutedEventArgs e)
        {
            //输入框获取焦点全选
            Keyboard.Focus(TbFile);
            TbFile.SelectAll();
        }

        //void Vm_TransfEvent(string value)
        //{
        //    TransfEvent(value);
        //}

        void Vm_TransfEvent_New(string filePath, string copies, string isColor, string documentName, string pageCount)
        {
            TransfEvent_New(filePath, copies, isColor, documentName, pageCount);
        }

        private void BtnInc_Click(object sender, RoutedEventArgs e)
        {
            _number++;
            TbNumber.Text = _number.ToString();
        }

        private void BtnDec_Click(object sender, RoutedEventArgs e)
        {
            _number = _number - 1 < 0 ? 0 : _number - 1;
            TbNumber.Text = _number.ToString();
        }
    }
}
