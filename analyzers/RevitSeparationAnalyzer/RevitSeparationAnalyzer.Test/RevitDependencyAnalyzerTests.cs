using Microsoft.CodeAnalysis.Testing;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerVerifier<RevitSeparationAnalyzer.RevitDependencyAnalyzer, Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace RevitSeparationAnalyzer.Test
{
    public class RevitDependencyAnalyzerTests
    {
        /// <summary>
        /// Helper method that creates compiler error diagnostics for missing Revit references
        /// </summary>
        /// <param name="revitUsingLine">The line number of the Autodesk.Revit.DB using statement (0-based)</param>
        /// <param name="documentTypeLocations">Collection of locations where Document type is referenced, as (line, column) pairs (0-based)</param>
        /// <returns>Collection of expected compiler error diagnostics</returns>
        private static IEnumerable<DiagnosticResult> GetRevitReferenceCompilerErrors(
            int revitUsingLine, 
            params (int line, int column)[] documentTypeLocations)
        {
            // Add compiler error for Autodesk namespace
            var diagnostics = new List<DiagnosticResult>
            {
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(revitUsingLine, 7, revitUsingLine, 15)
                    .WithArguments("Autodesk")
            };
            
            // Add compiler errors for each Document type reference
            foreach (var location in documentTypeLocations)
            {
                diagnostics.Add(
                    DiagnosticResult.CompilerError("CS0246")
                        .WithSpan(location.line, location.column, location.line, location.column + 8)
                        .WithArguments("Document")
                );
            }
            
            return diagnostics;
        }

        // Test that ViewModels should not reference Revit
        [Fact]
        public async Task ViewModelWithRevitReference_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public class {|#0:HomeViewModel|}
    {
        public Document RevitDocument { get; set; }
    }
}";

            // Combine our analyzer diagnostic with compiler errors for missing Revit references
            var expected = new List<DiagnosticResult>
            {
                // Our analyzer diagnostic
                VerifyCS.Diagnostic(RevitDependencyAnalyzer.ViewModelDiagnosticId)
                    .WithLocation(0)
                    .WithArguments("HomeViewModel")
            };
            
            // Add expected compiler errors for missing Revit references
            expected.AddRange(GetRevitReferenceCompilerErrors(
                revitUsingLine: 3,
                (line: 9, column: 16) // Document in property
            ));

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that correct ViewModels don't trigger diagnostics
        [Fact]
        public async Task ViewModelWithoutRevitReference_NoDiagnostic()
        {
            var test = @"
using System;

namespace TestNamespace
{
    public class HomeViewModel
    {
        public string Title { get; set; }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Test that Queries should not reference Revit
        [Fact]
        public async Task QueryWithRevitReference_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public class {|#0:GetDocumentTitleQuery|}
    {
        public Document Document { get; }

        public GetDocumentTitleQuery(Document document)
        {
            Document = document;
        }
    }
}";

            // Combine our analyzer diagnostic with compiler errors for missing Revit references
            var expected = new List<DiagnosticResult>
            {
                // Our analyzer diagnostic
                VerifyCS.Diagnostic(RevitDependencyAnalyzer.QueryDiagnosticId)
                    .WithLocation(0)
                    .WithArguments("GetDocumentTitleQuery")
            };
            
            // Add expected compiler errors for missing Revit references
            expected.AddRange(GetRevitReferenceCompilerErrors(
                revitUsingLine: 3,
                (line: 9, column: 16),  // Document in property
                (line: 11, column: 38)  // Document in parameter
            ));

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that correct Queries don't trigger diagnostics
        [Fact]
        public async Task QueryWithoutRevitReference_NoDiagnostic()
        {
            var test = @"
using System;

namespace TestNamespace
{
    public class GetDocumentTitleQuery : IQuery<GetDocumentTitleQueryResult>
    {
    }

    public interface IQuery<TResult> { }

    public class GetDocumentTitleQueryResult
    {
        public string Title { get; set; }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Test that QueryResults should not reference Revit
        [Fact]
        public async Task QueryResultWithRevitReference_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public class {|#0:GetDocumentTitleQueryResult|}
    {
        public string Title { get; set; }
        public Document Document { get; set; }
    }
}";

            // Combine our analyzer diagnostic with compiler errors for missing Revit references
            var expected = new List<DiagnosticResult>
            {
                // Our analyzer diagnostic
                VerifyCS.Diagnostic(RevitDependencyAnalyzer.QueryResultDiagnosticId)
                    .WithLocation(0)
                    .WithArguments("GetDocumentTitleQueryResult")
            };
            
            // Add expected compiler errors for missing Revit references
            expected.AddRange(GetRevitReferenceCompilerErrors(
                revitUsingLine: 3,
                (line: 10, column: 16)  // Document in property
            ));

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that correct QueryResults don't trigger diagnostics
        [Fact]
        public async Task QueryResultWithoutRevitReference_NoDiagnostic()
        {
            var test = @"
using System;

namespace TestNamespace
{
    public class GetDocumentTitleQueryResult
    {
        public string Title { get; set; }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
