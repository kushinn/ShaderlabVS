using Microsoft.VisualStudio.Text;
using System;

namespace ShaderlabVS
{
    internal class Utilities
    {
        public static readonly char[] sWordSeparators = { '{', '}', ' ', '\t', '(', ')', '[', ']', '+', '-', '*', '/', '%', '^', '>', '<', ':',
                                '.', ';', '\"', '\'', '?', '\\', '&', '|', '`', '$', '#', ','};
        private static readonly char[] sSpace = { '\t', ' ' };

        public static bool IsTokenSeparator(char c)
        {
            for (var i = 0; i < sWordSeparators.Length; ++i)
            {
                if (sWordSeparators[i] == c) return true;
            }
            return false;
        }

        public static bool IsCommentLine(string lineText)
        {
            string checkText = lineText.Trim();
            if (checkText.StartsWith("//")
               || checkText.StartsWith("/*")
               || checkText.EndsWith("*/"))
            {
                return true;
            }

            return false;
        }

        public static bool IsInCommentLine(SnapshotPoint position)
        {
            string lineText = position.GetContainingLine().GetText();
            return Utilities.IsCommentLine(lineText);
        }

        public static int IndexOfNonWhitespaceCharacter(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsWhiteSpace(text[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool IsInCGOrHLSLFile(string filePath)
        {
            var lower = filePath.ToLower();
            return lower.EndsWith(".cg") || lower.EndsWith(".hlsl");
        }

        public static string RemoveSpace(string tk)
        {
            if (tk.IndexOfAny(sSpace) < 0) return tk;
            string[] newTkArray = tk.Split(sSpace, StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(newTkArray);
        }
    }
}
