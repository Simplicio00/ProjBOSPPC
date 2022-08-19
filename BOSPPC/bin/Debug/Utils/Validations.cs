using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WinAPP.Utils
{
    internal static class Validations
    {
        internal static bool FileIsValid(string formato)
        {
            var info = new List<string>() { "txt", "TXT" };

            if (info.Contains(formato).Equals(false)) return false;

            return true;
        }
    }
}
