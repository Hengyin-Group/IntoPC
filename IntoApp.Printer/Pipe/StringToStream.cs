using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace IntoApp.Printer.Pipe
{
    public class StringToStream
    {

        private string Contents;
        private StreamString streamString;


        public StringToStream(StreamString ss, string contents)
        {
            Contents = contents;
            streamString = ss;
        }

        public StringToStream(StreamString ss,JObject jo)
        {

        }

        public void Start()
        {
            //string contents = File.ReadAllText(fn);
            streamString.WriterString(Contents);

        }

    }
}
