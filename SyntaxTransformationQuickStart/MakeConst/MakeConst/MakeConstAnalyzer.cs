using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MakeConst
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MakeConstAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor ConstRule = new DiagnosticDescriptor(RuleDescriptor.ConstDiagnosticId, RuleDescriptor.Title, RuleDescriptor.MessageFormat, RuleDescriptor.Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: RuleDescriptor.Description);
        private static DiagnosticDescriptor NullRule = new DiagnosticDescriptor(RuleDescriptor.NullDiagnosticId, RuleDescriptor.Null_Title, RuleDescriptor.Null_MessageFormat, RuleDescriptor.Null_Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: RuleDescriptor.Null_Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(ConstRule,NullRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

            //Below code is used to get the declarartion statement
            var variable = localDeclaration.Declaration.Variables.Single();

            //Below code is used to get the type of the variable "variable" from the above statement
            var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);

            var assignedTypeInfo = variable.Initializer.Value.Kind();

            if(localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword) && !assignedTypeInfo.Equals(SyntaxKind.NullLiteralExpression))
            {
                return;
            }

            //Perform data flow analysis on local declaration
            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

            //Check for variable is null. From the data flow analysis we will come to know if the variable is being assigned elsewhere.
            if (assignedTypeInfo.Equals(SyntaxKind.NullLiteralExpression) && !dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(NullRule, context.Node.GetLocation()));
            }

            //From the data flow analysis we will come to know if the variable is being assigned elsewhere.
            if(!dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(ConstRule, context.Node.GetLocation()));
            }

            return; 
        }        
    }
}
