using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoApp.Printer.Pipe
{
    public class PipeEnumHelper
    {
        public enum Subject
        {
            intoapp=1,
            autoupdate=2,
        }
        public enum Action
        {
            start = 1,
            restart = 2,
            close = 3
        }
    }
}
