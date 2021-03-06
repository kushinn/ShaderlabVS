﻿using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ShaderlabVS
{
    #region Format definition
    /// <summary>
    /// Text
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabText)]
    [Name(Constants.ShaderlabText)]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class ShaderlabTextFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Shaderlab-Text" classification type
        /// </summary>
        public ShaderlabTextFormat()
        {
            this.DisplayName = Constants.ShaderlabText;
            this.ForegroundColor = Colors.Orange;
        }
    }

    /// <summary>
    /// Strings
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabStrings)]
    [Name(Constants.ShaderlabStrings)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabStrings : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Shaderlab-Text" classification type
        /// </summary>
        public ShaderlabStrings()
        {
            this.DisplayName = Constants.ShaderlabStrings;
            this.ForegroundColor = Colors.DarkRed;
        }
    }

    /// <summary>
    /// Functions
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabFunction)]
    [Name(Constants.ShaderlabFunction)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabFunctionFormat : ClassificationFormatDefinition
    {
        public ShaderlabFunctionFormat()
        {
            this.DisplayName = Constants.ShaderlabFunction;
            this.ForegroundColor = Color.FromScRgb(1.0f, 1.0f, 0.5f, 0.0f);
        }
    }

    /// <summary>
    /// Comments
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabComment)]
    [Name(Constants.ShaderlabComment)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabComment : ClassificationFormatDefinition
    {
        public ShaderlabComment()
        {
            this.DisplayName = Constants.ShaderlabComment;
            this.ForegroundColor = Colors.ForestGreen;
        }
    }

    /// <summary>
    /// keywords
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabHLSLCGKeyword)]
    [Name(Constants.ShaderlabHLSLCGKeyword)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabKeyword : ClassificationFormatDefinition
    {
        public ShaderlabKeyword()
        {
            this.DisplayName = Constants.ShaderlabHLSLCGKeyword;
            this.ForegroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Unity3D keywords
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabUnityKeywords)]
    [Name(Constants.ShaderlabUnityKeywords)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabUnityBlockKeywords : ClassificationFormatDefinition
    {
        public ShaderlabUnityBlockKeywords()
        {
            this.DisplayName = Constants.ShaderlabUnityKeywords;
            this.ForegroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Unity3D keywords begin with #
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabUnityKeywordsPara)]
    [Name(Constants.ShaderlabUnityKeywordsPara)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabUnityKeywordsPara : ClassificationFormatDefinition
    {
        public ShaderlabUnityKeywordsPara()
        {
            this.DisplayName = Constants.ShaderlabUnityKeywordsPara;
            this.ForegroundColor = Colors.DarkMagenta;
        }
    }

    /// <summary>
    /// Data type
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabDataType)]
    [Name(Constants.ShaderlabDataType)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabDataType : ClassificationFormatDefinition
    {
        public ShaderlabDataType()
        {
            this.DisplayName = Constants.ShaderlabDataType;
            this.ForegroundColor = Colors.Blue;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [Name(Constants.ShaderlabBracesMarker)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class BraceMarkerDefination : MarkerFormatDefinition
    {
        public BraceMarkerDefination()
        {
            this.DisplayName = Constants.ShaderlabBracesMarker;
            this.BackgroundColor = Colors.Gray;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [Name(Constants.ShaderlabMacro)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class MacroDefination : MarkerFormatDefinition
    {
        public MacroDefination()
        {
            this.DisplayName = Constants.ShaderlabMacro;
            this.ForegroundColor = Color.FromScRgb(1, 1, 0.5f, 1);
        }
    }


    /// <summary>
    /// User Data type
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabUserDataType)]
    [Name(Constants.ShaderlabUserDataType)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabUserDataType : ClassificationFormatDefinition
    {
        public ShaderlabUserDataType()
        {
            this.DisplayName = Constants.ShaderlabUserDataType;
            this.ForegroundColor = Colors.CadetBlue;
        }
    }

    /// <summary>
    /// User Function
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.ShaderlabUserFunction)]
    [Name(Constants.ShaderlabUserFunction)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ShaderlabUserFunction : ClassificationFormatDefinition
    {
        public ShaderlabUserFunction()
        {
            this.DisplayName = Constants.ShaderlabUserFunction;
            this.ForegroundColor = Colors.BlueViolet;
        }
    }

    #endregion //Format definition
}
