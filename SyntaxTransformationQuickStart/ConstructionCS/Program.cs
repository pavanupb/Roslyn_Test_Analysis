using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ConstructionCS
{
    class Program
    {
        private const string sampleCode = @"using System;
        using System.Collections;
        using System.Linq;
        using System.Text;
 
        namespace HelloWorld
        {
            class Program
            {
                static void Main(string[] args)
                {
                    Console.WriteLine(""Hello, World!"");
                }
            }
        }";
        static void Main(string[] args)
        {
            //Code to create a node to add in the tree
            NameSyntax name = IdentifierName("System");
            Console.WriteLine($"\tCreated the identifier {name.ToString()}");

            name = QualifiedName(name, IdentifierName("Collections"));
            Console.WriteLine(name.ToString());

            name = QualifiedName(name, IdentifierName("Generic"));
            Console.WriteLine(name.ToString());

            //Creation of the tree
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sampleCode);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            //Since the syntax tree is immutable, the new node has not been added yet.
            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name);
            Console.WriteLine(root.ToString());

            //The below code replaces the old node(oldUsing) with the new node(newUsing)
            root = root.ReplaceNode(oldUsing, newUsing);
            Console.WriteLine(root.ToString());















        }
    }
}
