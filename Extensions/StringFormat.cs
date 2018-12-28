using System;

namespace Client.Extensions
{
    public static class StringFormat
    {
        public static string FormatToAccounting(Double Value = 0, int AfterZero = 2, Boolean DefaultFormat = true){
            string TheeZeros = "".PadLeft(AfterZero, '0');
            string ZeroValue = "";

            if (TheeZeros != "")
                TheeZeros = "." + TheeZeros;

            if (DefaultFormat)
                ZeroValue = "0" + TheeZeros;
            else
                ZeroValue = " — ";

            return string.Format("#,##0" + TheeZeros + ";(#,##0" + TheeZeros + ");" + ZeroValue, Value);
        }
    }
}
