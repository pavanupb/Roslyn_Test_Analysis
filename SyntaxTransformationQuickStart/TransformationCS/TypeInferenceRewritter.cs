﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TransformationCS
{
    public class TypeInferenceRewritter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel SemanticModel;

        /// <summary>
        /// Constructor where semanticModel is being assigned to SemanticModel read-only field.
        /// </summary>
        /// <param name="semanticModel"></param>
        public TypeInferenceRewritter(SemanticModel semanticModel) => SemanticModel = semanticModel;

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if(node.Declaration.Variables.Count > 1)
            {
                return node;
            }
            if(node.Declaration.Variables[0].Initializer == null)
            {
                return node;
            }

            VariableDeclaratorSyntax declarator = node.Declaration.Variables.First();
            TypeSyntax variableTypeName = node.Declaration.Type;
                
            ITypeSymbol variableType = (ITypeSymbol)SemanticModel
                .GetSymbolInfo(variableTypeName)
                .Symbol;

            TypeInfo initializerInfo = SemanticModel.GetTypeInfo(declarator.Initializer.Value);

            if(variableType == initializerInfo.Type)
            {
                TypeSyntax varTypeName = IdentifierName("var")
                    .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                    .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

                return node.ReplaceNode(variableTypeName, varTypeName);

            }
            else
            {
                return node;
            }
        }
    }
}
