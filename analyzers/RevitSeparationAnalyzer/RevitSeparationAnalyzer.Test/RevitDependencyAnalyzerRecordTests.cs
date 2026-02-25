using Microsoft.CodeAnalysis.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerVerifier<RevitSeparationAnalyzer.RevitDependencyAnalyzer, Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace RevitSeparationAnalyzer.Test
{    public class RevitDependencyAnalyzerRecordTests
    {
        /// <summary>
        /// Helper method that creates compiler error diagnostics for IsExternalInit missing in record types
        /// </summary>
        /// <param name="locations">Locations where the init accessor is causing the error as (line, column) pairs (0-based)</param>
        /// <returns>Collection of expected compiler error diagnostics</returns>
        private static IEnumerable<DiagnosticResult> GetIsExternalInitCompilerErrors(
            params (int line, int column)[] locations)
        {
            var diagnostics = new List<DiagnosticResult>();
            
            foreach (var location in locations)
            {
                diagnostics.Add(
                    DiagnosticResult.CompilerError("CS0518")
                        .WithSpan(location.line, location.column, location.line, location.column + 5)
                        .WithArguments("System.Runtime.CompilerServices.IsExternalInit")
                );
            }
            
            return diagnostics;
        }
        
        /// <summary>
        /// Helper method that creates compiler error diagnostics for missing Revit references
        /// </summary>
        /// <param name="revitUsingLine">The line number of the Autodesk.Revit.DB using statement (0-based)</param>
        /// <param name="elementIdLocations">Collection of locations where ElementId type is referenced, as (line, column) pairs (0-based)</param>
        /// <returns>Collection of expected compiler error diagnostics</returns>
        private static IEnumerable<DiagnosticResult> GetRevitElementIdCompilerErrors(
            int revitUsingLine, 
            params (int line, int column)[] elementIdLocations)
        {
            // Add compiler error for Autodesk namespace
            var diagnostics = new List<DiagnosticResult>
            {
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(revitUsingLine, 7, revitUsingLine, 15)
                    .WithArguments("Autodesk")
            };
            
            // Add compiler errors for each ElementId type reference
            foreach (var location in elementIdLocations)
            {
                diagnostics.Add(
                    DiagnosticResult.CompilerError("CS0246")
                        .WithSpan(location.line, location.column, location.line, location.column + 9)
                        .WithArguments("ElementId")
                );
            }
            
            return diagnostics;
        }

        // Test that record QueryResults with Revit constructor parameters should trigger diagnostics
        [Fact]        public async Task RecordQueryResultWithRevitConstructorParameter_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public record {|#0:GetDocumentTitleQueryResult|}(ElementId Title);
}";

            // Create expected diagnostics list with proper ordering:
            // 1. Compiler errors for Autodesk namespace
            // 2. Our analyzer diagnostic
            // 3. Compiler errors for ElementId
            // 4. Compiler errors for IsExternalInit
            var expected = new List<DiagnosticResult>();
            
            // 1. First, add compiler error for Autodesk namespace
            expected.Add(DiagnosticResult.CompilerError("CS0246")
                .WithSpan(3, 7, 3, 15)
                .WithArguments("Autodesk"));
                
            // 2. Next, add our analyzer diagnostic
            expected.Add(VerifyCS.Diagnostic(RevitDependencyAnalyzer.QueryResultDiagnosticId)
                .WithLocation(0)
                .WithArguments("GetDocumentTitleQueryResult"));
                
            // 3. Next, add compiler error for ElementId
            expected.Add(DiagnosticResult.CompilerError("CS0246")
                .WithSpan(7, 47, 7, 56)
                .WithArguments("ElementId"));
                
            // 4. Finally, add compiler error for IsExternalInit
            expected.Add(DiagnosticResult.CompilerError("CS0518")
                .WithSpan(7, 57, 7, 62)
                .WithArguments("System.Runtime.CompilerServices.IsExternalInit"));

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that record Query with Revit constructor parameters should trigger diagnostics
        [Fact]        public async Task RecordQueryWithRevitConstructorParameter_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public record {|#0:GetElementIdQuery|}(ElementId Id);
}";

            // Create expected diagnostics list with proper ordering:
            var expected = new List<DiagnosticResult>
            {
                // First, compiler error for Autodesk namespace
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(3, 7, 3, 15)
                    .WithArguments("Autodesk"),
                    
                // Next, our analyzer diagnostic
                VerifyCS.Diagnostic(RevitDependencyAnalyzer.QueryDiagnosticId)
                    .WithLocation(0)
                    .WithArguments("GetElementIdQuery"),
                    
                // Next, compiler error for ElementId
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(7, 37, 7, 46)
                    .WithArguments("ElementId"),
                    
                // Finally, compiler error for IsExternalInit
                DiagnosticResult.CompilerError("CS0518")
                    .WithSpan(7, 47, 7, 49)
                    .WithArguments("System.Runtime.CompilerServices.IsExternalInit")
            };

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that correct record QueryResults don't trigger diagnostics
        [Fact]        public async Task RecordQueryResultWithoutRevitReference_NoDiagnostic()
        {
            var test = @"
using System;

namespace TestNamespace
{    public record GetDocumentTitleQueryResult(string Title);
}";

            var expected = new List<DiagnosticResult>
            {
                // Add expected compiler errors for missing IsExternalInit type
                DiagnosticResult.CompilerError("CS0518")
                    .WithSpan(5, 55, 5, 60)
                    .WithArguments("System.Runtime.CompilerServices.IsExternalInit")
            };

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }

        // Test that record with additional properties and Revit references is detected
        [Fact]        public async Task RecordWithAdditionalPropertiesAndRevitReference_ProducesDiagnostic()
        {
            var test = @"
using System;
using Autodesk.Revit.DB;

namespace TestNamespace
{
    public record {|#0:ComplexQueryResult|}(string Name)
    {
        public ElementId Id { get; init; }
    }
}";

            // Create expected diagnostics list with proper ordering
            var expected = new List<DiagnosticResult>
            {
                // First, compiler error for Autodesk namespace
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(3, 7, 3, 15)
                    .WithArguments("Autodesk"),
                    
                // Next, our analyzer diagnostic
                VerifyCS.Diagnostic(RevitDependencyAnalyzer.QueryResultDiagnosticId)
                    .WithLocation(0)
                    .WithArguments("ComplexQueryResult"),
                    
                // Next, compiler error for ElementId
                DiagnosticResult.CompilerError("CS0246")
                    .WithSpan(9, 16, 9, 25)
                    .WithArguments("ElementId"),
                    
                // Next, compiler error for IsExternalInit in constructor
                DiagnosticResult.CompilerError("CS0518")
                    .WithSpan(7, 45, 7, 49)
                    .WithArguments("System.Runtime.CompilerServices.IsExternalInit"),
                    
                // Finally, compiler error for IsExternalInit in property
                DiagnosticResult.CompilerError("CS0518")
                    .WithSpan(9, 36, 9, 40)
                    .WithArguments("System.Runtime.CompilerServices.IsExternalInit")
            };

            await VerifyCS.VerifyAnalyzerAsync(test, expected.ToArray());
        }
    }
}
