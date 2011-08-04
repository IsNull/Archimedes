using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Utils
{
    public static class AsciiHelper
    {
        //48 - 57
        public static bool IsAsciiNum(char c) {
            return (c > 47) && (c < 58);
        }

        public static bool IsAsciiHexNum(char c) {
            return IsAsciiNum(c) || ((c > 0x40) && (c < 0x47)) || ((c > 0x60) && (c < 0x67));
        }

        //65 -90
        public static bool IsAsciiLetterHigh(char c) {
            return (c > 64) && (c < 91);
        }

        //97 - 122
        public static bool IsAsciiLetterLow(char c) {
            return (c > 96) && (c < 123);
        }

        /// <summary>
        /// Is the given char a default AsciiLetter? (a-zA-Z0-9)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAsciiLiteralLetter(char c){
            return ((c > 47) && (c < 58)) || ((c > 64) && (c < 91)) || ((c > 96) && (c < 123));
        }
    }
}
