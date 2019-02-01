using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyaim.NFC.NFC750.Common
{
    public class BinaryHelper
    {
        public static string HexToStr(string mHex)
        {
            mHex = mHex.Replace(" ", "").Replace("-", "");
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        }

        public static string StrToHex(string mStr)
        {
            return BitConverter.ToString(ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", "");
        }
    }
}
