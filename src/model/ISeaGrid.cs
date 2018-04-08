
CONVERSION ERROR: Code could not be converted. Details:

----- Exception 1 of 1 -----
System.NotImplementedException: Microsoft.CodeAnalysis.VisualBasic.Syntax.PropertyStatementSyntax not implemented!
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.DefaultVisit(SyntaxNode node)
   at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitPropertyStatement(PropertyStatementSyntax node)
   at Microsoft.CodeAnalysis.VisualBasic.Syntax.PropertyStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.<ConvertMembers>b__11_0(StatementSyntax m)
   at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.<ConvertMembers>d__11.MoveNext()
   at Microsoft.CodeAnalysis.CSharp.SyntaxFactory.List[TNode](IEnumerable`1 nodes)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.VisitInterfaceBlock(InterfaceBlockSyntax node)
   at Microsoft.CodeAnalysis.VisualBasic.Syntax.InterfaceBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.<VisitCompilationUnit>b__8_1(StatementSyntax m)
   at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
   at Microsoft.CodeAnalysis.CSharp.SyntaxFactory.List[TNode](IEnumerable`1 nodes)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.NodesVisitor.VisitCompilationUnit(CompilationUnitSyntax node)
   at Microsoft.CodeAnalysis.VisualBasic.Syntax.CompilationUnitSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.Convert(VisualBasicSyntaxNode input, SemanticModel semanticModel, Document targetDocument)
   at RefactoringEssentials.CSharp.Converter.VisualBasicConverter.ConvertText(String text, MetadataReference[] references)

Please check for any errors in the original code and try again.
