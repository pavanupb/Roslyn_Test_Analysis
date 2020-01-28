using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MakeConst
{
    public class RuleDescriptor
    {
        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization

        #region Constant_Variable_Analysis
        public const string ConstDiagnosticId = "MakeConst";
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        public const string Category = "Usage";
        #endregion

        #region Null_Pointer_Analysis
        public const string NullDiagnosticId = "RemoveVariable";
        public static readonly LocalizableString Null_Title = new LocalizableResourceString(nameof(Resources.NullAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString Null_MessageFormat = new LocalizableResourceString(nameof(Resources.NullAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString Null_Description = new LocalizableResourceString(nameof(Resources.NullAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        public const string Null_Category = "Usage";
        #endregion







    }
}
