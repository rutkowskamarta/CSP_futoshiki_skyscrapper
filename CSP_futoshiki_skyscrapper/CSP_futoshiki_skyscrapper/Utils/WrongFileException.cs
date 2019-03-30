using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class WrongFileException : Exception
    {
        public WrongFileException(string message) : base(message) {}
    }
}
