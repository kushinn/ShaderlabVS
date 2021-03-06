﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShaderlabVS.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ShaderlabDataManager
    {
        #region Constants
        public const string HLSL_CG_DATATYPE_DEFINATIONFILE = "Data\\HLSL_CG_datatype.def";
        public const string HLSL_CG_FUNCTION_DEFINATIONFILE = "Data\\HLSL_CG_functions.def";
        public const string HLSL_CG_KEYWORD_DEFINATIONFILE = "Data\\HLSL_CG_Keywords.def";

        public const string UNITY3D_DATATYPE_DEFINATIONFILE = "Data\\Unity3D_datatype.def";
        public const string UNITY3D_FUNCTION_DEFINATIONFILE = "Data\\Unity3D_functions.def";
        public const string UNITY3D_KEYWORD_DEFINATIONFILE = "Data\\Unity3D_keywords.def";
        public const string UNITY3D_MACROS_DEFINATIONFILE = "Data\\Unity3D_macros.def";
        public const string UNITY3D_VALUES_DEFINATIONFILE = "Data\\Unity3D_values.def";
        public const string UNITY3D_CGINCLUDES_DEFINATIONFILE = "Data\\CGIncludes.def";
        #endregion

        #region Properties
        public string CurrentAssemblyDir { get; private set; }

        public List<HLSLCGFunction> HLSLCGFunctions { get; private set; }
        public List<string> HLSLCGBlockKeywords { get; private set; }
        public List<string> HLSLCGNonblockKeywords { get; private set; }
        public List<string> HLSLCGSpecialKeywords { get; private set; }
        public List<string> HLSLCGDatatypes { get; private set; }


        public List<UnityBuiltinDatatype> UnityBuiltinDatatypes { get; private set; }
        public List<UnityBuiltinFunction> UnityBuiltinFunctions { get; private set; }
        public List<UnityBuiltinMacros> UnityBuiltinMacros { get; private set; }
        public List<UnityKeywords> UnityKeywords { get; private set; }
        public List<UnityBuiltinValue> UnityBuiltinValues { get; private set; }
        public List<UnityFileEntry> UnityCGIncludes { get; private set; }

        public List<UnityBuiltinDatatype> UserDatatypes { get; private set; }
        public List<UnityBuiltinFunction> UserFunctions { get; private set; }
        #endregion

        #region Singleton

        private static object lockObj = new object();

        private static ShaderlabDataManager _instance;

        public static ShaderlabDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ShaderlabDataManager();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        private ShaderlabDataManager()
        {
            string currentAssemblyDir = (new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(8))).DirectoryName;
            CurrentAssemblyDir = currentAssemblyDir;

            HLSLCGFunctions = DefinationDataProvider<HLSLCGFunction>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_FUNCTION_DEFINATIONFILE));

            List<HLSLCGKeywords> hlslcgKeywords = DefinationDataProvider<HLSLCGKeywords>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_KEYWORD_DEFINATIONFILE));
            HLSLCGBlockKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "block");
            HLSLCGNonblockKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "nonblock");
            HLSLCGSpecialKeywords = GetHLSLCGKeywordsByType(hlslcgKeywords, "special");

            var dts = DefinationDataProvider<HLSLCGDataTypes>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.HLSL_CG_DATATYPE_DEFINATIONFILE)).First();
            if (dts != null)
            {
                HLSLCGDatatypes = dts.DataTypes;
            }
            UserDatatypes = new List<UnityBuiltinDatatype>();
            UserFunctions = new List<UnityBuiltinFunction>();
            UnityBuiltinDatatypes = DefinationDataProvider<UnityBuiltinDatatype>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_DATATYPE_DEFINATIONFILE));
            UnityBuiltinFunctions = DefinationDataProvider<UnityBuiltinFunction>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_FUNCTION_DEFINATIONFILE));
            UnityBuiltinMacros = DefinationDataProvider<UnityBuiltinMacros>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_MACROS_DEFINATIONFILE));
            UnityBuiltinValues = DefinationDataProvider<UnityBuiltinValue>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_VALUES_DEFINATIONFILE));
            UnityKeywords = DefinationDataProvider<UnityKeywords>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_KEYWORD_DEFINATIONFILE));
            UnityCGIncludes = DefinationDataProvider<UnityFileEntry>.ProvideFromFile(Path.Combine(currentAssemblyDir, ShaderlabDataManager.UNITY3D_CGINCLUDES_DEFINATIONFILE));
        }

        public List<string> LoadCGIncludesContents()
        {
            List<string> searchedFiles = LoadCGIncludedFiles();
            List<string> contents = new List<string>();
            try
            {
                foreach (var f in searchedFiles)
                {
                    contents.Add(File.ReadAllText(f));
                }
            }
            catch
            {
            }
            return contents;
        }

        public List<string> LoadCGIncludedFiles()
        {
            List<string> contents = new List<string>();
            foreach (var entry in UnityCGIncludes)
            {
                if (!Directory.Exists(entry.Description)) continue;
                try
                {
                    string[] files = Directory.GetFiles(entry.Description, entry.Format, SearchOption.TopDirectoryOnly);
                    foreach (var f in files)
                    {
                        if (!contents.Contains(f))
                        {
                            contents.Add(f);
                        }
                    }
                }
                catch
                {
                }
            }
            return contents;
        }

        private List<string> GetHLSLCGKeywordsByType(List<HLSLCGKeywords> alltypes, string type)
        {
            var kw = alltypes.First(k => k.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (kw != null)
            {
                return kw.Keywords;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
