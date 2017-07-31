using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using ShaderlabVS.Lexer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ShaderlabVS.Data;

namespace ShaderlabVS
{
    #region Provider definition

    /// <summary>
    /// Apply settings for all .shader files
    /// </summary>
    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ContentType)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class ShaderlabVSClassifierProvider : ITaggerProvider
    {
        [Export]
        [Name(Constants.ContentType)]
        [BaseDefinition(Constants.BaseDefination)]
        public static ContentTypeDefinition ShaderlabContentType = null;

        [Export]
        [FileExtension(Constants.ShaderFileNameExt)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition ShaderlabFileType = null;

        [Export]
        [FileExtension(Constants.ComputeShaderFileNameExt)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition ComputeShaderFileType = null;

        [Export]
        [FileExtension(Constants.CGIncludeFileExt)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition CgIncludeFileType = null;

        [Export]
        [FileExtension(Constants.GLSLIncludeFileExt)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition GLSLIncludeFileType = null;

        [Export]
        [FileExtension(Constants.CGFile)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition cgFileType = null;

        [Export]
        [FileExtension(Constants.HLSLFile)]
        [ContentType(Constants.ContentType)]
        public static FileExtensionToContentTypeDefinition hlslFileType = null;

        [Import]
        internal IClassificationTypeRegistryService classificationTypeRegistry = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new ShaderlabClassifier(buffer, classificationTypeRegistry) as ITagger<T>;
        }
    }
    #endregion //provider def

    #region Classifier

    internal sealed class ShaderlabClassifier : ITagger<ClassificationTag>
    {
        private static readonly char[] sStrcutSeparators = new char[] { '{', ' ', '\n', '\r' };

        static ShaderlabClassifier()
        {
            Scanner.LoadTableDataFromLex();
        }

        private static readonly Dictionary<ShaderlabToken, IClassificationType> classTypeDict = new Dictionary<ShaderlabToken, IClassificationType>();
        Scanner scanner;
        ITextBuffer textBuffer;


        public ShaderlabClassifier(ITextBuffer buffer, IClassificationTypeRegistryService registerService)
        {
            textBuffer = buffer;
            scanner = new Scanner();
            classTypeDict.Clear();

            classTypeDict.Add(ShaderlabToken.TEXT, registerService.GetClassificationType(Constants.ShaderlabText));
            classTypeDict.Add(ShaderlabToken.COMMENT, registerService.GetClassificationType(Constants.ShaderlabComment));
            classTypeDict.Add(ShaderlabToken.HLSLCGDATATYPE, registerService.GetClassificationType(Constants.ShaderlabDataType));
            classTypeDict.Add(ShaderlabToken.HLSLCGFUNCTION, registerService.GetClassificationType(Constants.ShaderlabFunction));
            classTypeDict.Add(ShaderlabToken.HLSLCGKEYWORD, registerService.GetClassificationType(Constants.ShaderlabHLSLCGKeyword));
            classTypeDict.Add(ShaderlabToken.HLSLCGKEYWORDSPECIAL, registerService.GetClassificationType(Constants.ShaderlabHLSLCGKeyword));
            classTypeDict.Add(ShaderlabToken.UNITYKEYWORD, registerService.GetClassificationType(Constants.ShaderlabUnityKeywords));
            classTypeDict.Add(ShaderlabToken.UNITYKEYWORD_PARA, registerService.GetClassificationType(Constants.ShaderlabUnityKeywordsPara));
            classTypeDict.Add(ShaderlabToken.UNITYDATATYPE, registerService.GetClassificationType(Constants.ShaderlabDataType));
            classTypeDict.Add(ShaderlabToken.UNITYFUNCTION, registerService.GetClassificationType(Constants.ShaderlabFunction));
            classTypeDict.Add(ShaderlabToken.STRING_LITERAL, registerService.GetClassificationType(Constants.ShaderlabStrings));
            classTypeDict.Add(ShaderlabToken.UNITYVALUES, registerService.GetClassificationType(Constants.ShaderlabUnityKeywords));
            classTypeDict.Add(ShaderlabToken.UNITYMACROS, registerService.GetClassificationType(Constants.ShaderlabMacro));
            classTypeDict.Add(ShaderlabToken.USERDATATYPE, registerService.GetClassificationType(Constants.ShaderlabUserDataType));
            classTypeDict.Add(ShaderlabToken.USERFUNCTION, registerService.GetClassificationType(Constants.ShaderlabUserFunction));
            classTypeDict.Add(ShaderlabToken.UNDEFINED, registerService.GetClassificationType(Constants.ShaderlabText));
        }

        private static bool IsTokenSeparator(char c)
        {
            return c == ' '
               || c == '\n'
               || c == '\r'
               || c == ']'
               || c == '['
               || c == '{'
               || c == '}'
               || c == '+'
               || c == '-'
               || c == '*'
               || c == '/'
               || c == '%'
               || c == '^'
               || c == '&'
               || c == '#'
               || c == '('
               || c == ')'
               || c == '!'
               || c == '~'
               || c == '?'
               || c == ':'
               || c == ','
               || c == ';'
               || c == '='
               || c == '|';
        }

        private static bool MoveToEndOfToken(string text, ref int pos)
        {
            while (pos < text.Length - 1 && !IsTokenSeparator(text[pos + 1]))
            {
                ++pos;
            }

            return pos < text.Length;
        }

        private static bool MoveToNextFrontOfToken(string text, ref int pos)
        {
            while ((pos < text.Length) && IsTokenSeparator(text[pos]))
            {
                ++pos;
            }

            return pos < text.Length;
        }

        private bool IsDataType(string tk)
        {
            if (tk == "void") return true;
            return ShaderlabDataManager.Instance.HLSLCGDatatypes.Contains(tk);
        }

        private bool IsStruct(string tk, int lastPos, ref int pos, ref int length, string text, out IClassificationType special)
        {
            special = null;
            if (tk == "struct")
            {
                if (!MoveToNextFrontOfToken(text, ref pos)) return false;

                int endIndex = pos;
                if (MoveToEndOfToken(text, ref endIndex))
                {
                    length = endIndex - pos + 1;
                    if (length > 0 && pos > lastPos)
                    {
                        ShaderlabDataManager.Instance.UserDatatypes.Add(new UnityBuiltinDatatype() { Name = text.Substring(pos, length) });
                        return classTypeDict.TryGetValue(ShaderlabToken.USERDATATYPE, out special);
                    }
                }
            }
            return false;
        }

        private bool IsFunction(string text, int pos)
        {
            while (++pos < text.Length)
            {
                char c = text[pos];
                if (c != ' ')
                {
                    return c == '(';
                }
            }
            return false;
        }

        private bool TryCreateSpecialToken(ShaderlabToken token, ref int pos, ref int length, string text, out IClassificationType special)
        {
            special = null;
            if (!MoveToNextFrontOfToken(text, ref pos)) return false;

            int lastPos = pos + length;
            if (token == ShaderlabToken.HLSLCGKEYWORD) // struct
            {
                string tk = text.Substring(pos, length).Trim();
                length = 0;
                pos = lastPos;

                // strcut define
                if (IsStruct(tk, lastPos, ref pos, ref length, text, out special))
                {
                }
                else if (IsDataType(tk))
                {
                    if (!MoveToNextFrontOfToken(text, ref pos)) return false;

                    int endIndex = pos;
                    if (MoveToEndOfToken(text, ref endIndex))
                    {
                        length = endIndex - pos + 1;
                        if (length > 0 && pos > lastPos && endIndex < text.Length - 1)
                        {
                            tk = text.Substring(pos, length);
                            if (IsFunction(text, pos + length))
                            {
                                ShaderlabDataManager.Instance.UserFunctions.Add(new UnityBuiltinFunction() { Name = tk });
                                return classTypeDict.TryGetValue(ShaderlabToken.USERFUNCTION, out special);
                            }
                        }
                    }
                }
            }

            return special != null;
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            ShaderlabCompletionSource.SetWordsInEditDocuments(spans[0].Snapshot.GetText());

            string text = " " + spans[0].Snapshot.GetText().ToLower();
            scanner.SetSource(text, 0);
            int token;
            IClassificationType cf;

            do
            {
                token = scanner.NextToken();
                int pos = scanner.GetPos();
                int length = scanner.GetLength();

                if (pos < 0 || length < 0 || pos > text.Length)
                {
                    continue;
                }

                if (pos + length > text.Length)
                {
                    length = text.Length - pos;
                }

                if (classTypeDict.TryGetValue((ShaderlabToken)token, out cf))
                {
                    switch ((ShaderlabToken)token)
                    {
                        case ShaderlabToken.HLSLCGKEYWORD:
                        case ShaderlabToken.UNITYKEYWORD:
                        case ShaderlabToken.UNITYKEYWORD_PARA:
                        case ShaderlabToken.HLSLCGDATATYPE:
                        case ShaderlabToken.HLSLCGFUNCTION:
                        case ShaderlabToken.UNITYFUNCTION:
                        case ShaderlabToken.UNITYMACROS:
                        case ShaderlabToken.UNITYDATATYPE:
                        case ShaderlabToken.UNITYVALUES:
                            length = length - 2;
                            scanner.PushbackText(length + 1);
                            break;
                        case ShaderlabToken.HLSLCGKEYWORDSPECIAL:
                            pos--;
                            length = length - 1;
                            scanner.PushbackText(length);
                            break;
                        case ShaderlabToken.STRING_LITERAL:
                        case ShaderlabToken.COMMENT:
                            pos--;
                            break;
                    }

                    if (pos < 0 || length < 0 || pos > text.Length)
                    {
                        continue;
                    }

                    yield return new TagSpan<ClassificationTag>(new SnapshotSpan(spans[0].Snapshot, new Span(pos, length)),
                                                                new ClassificationTag(cf));

                    int nexttfPos = pos;
                    int nexttLength = length;
                    IClassificationType nexttf;
                    if (TryCreateSpecialToken((ShaderlabToken)token, ref nexttfPos, ref nexttLength, text, out nexttf))
                    {
                        yield return new TagSpan<ClassificationTag>(new SnapshotSpan(spans[0].Snapshot, new Span(nexttfPos - 1, nexttLength)),
                                               new ClassificationTag(nexttf));
                    }
                }

            } while (token > (int)Tokens.EOF);

        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }

    #endregion //Classifier
}
