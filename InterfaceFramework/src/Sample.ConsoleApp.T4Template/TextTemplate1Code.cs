using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.ConsoleApp.T4Template
{
    public partial class TextTemplate1
    {
        private RequestModel _metaData { get; set; }
        public TextTemplate1(RequestModel MetaData)
        {
            this._metaData = MetaData;
        }


    }
}
