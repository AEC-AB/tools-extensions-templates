using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RevitSeparationAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RevitDependencyAnalyzer : DiagnosticAnalyzer    {   // Rule IDs
        public const string ViewModelDiagnosticId = "REVIT001";
        public const string QueryDiagnosticId = "REVIT002";
        public const string QueryResultDiagnosticId = "REVIT003";

        // Categories
        private const string Category = "Architecture";

        // Descriptors for each rule
        private static readonly DiagnosticDescriptor ViewModelRule = new DiagnosticDescriptor(
            ViewModelDiagnosticId,
            title: "ViewModel can not reference Revit",
            messageFormat: "ViewModel '{0}' can not contain references to Autodesk.Revit assemblies",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "ViewModels can not directly reference Revit assemblies to maintain proper separation of concerns.");
            
        private static readonly DiagnosticDescriptor QueryRule = new DiagnosticDescriptor(
            QueryDiagnosticId,
            title: "Query can not reference Revit",
            messageFormat: "Query '{0}' can not contain references to Autodesk.Revit assemblies",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Query definitions can not directly reference Revit assemblies to maintain proper separation of concerns.");
            
        private static readonly DiagnosticDescriptor QueryResultRule = new DiagnosticDescriptor(
            QueryResultDiagnosticId,
            title: "Query Result can not reference Revit",
            messageFormat: "QueryResult '{0}' can not contain references to Autodesk.Revit assemblies",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Query Result objects can not directly reference Revit assemblies to maintain proper separation of concerns.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ViewModelRule, QueryRule, QueryResultRule);        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for class and record declaration analysis
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeRecordDeclaration, SyntaxKind.RecordDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            // Skip if the class is not one of the types we're interested in
            if (!IsTargetType(classDeclaration))
                return;

            // Get class symbol
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            if (classSymbol == null)
                return;

            // Determine which rule to use based on the class type
            var (isTarget, rule) = GetApplicableRule(classSymbol);

            if (!isTarget)
                return;

            // Check for Revit references in the class
            if (HasRevitReferences(classDeclaration, semanticModel))
            {
                // Report diagnostic
                var diagnostic = Diagnostic.Create(rule, classDeclaration.Identifier.GetLocation(), classSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }        private void AnalyzeRecordDeclaration(SyntaxNodeAnalysisContext context)
        {
            var recordDeclaration = (RecordDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            // Skip if the record is not one of the types we're interested in
            if (!IsTargetType(recordDeclaration))
                return;

            // Get record symbol
            var recordSymbol = semanticModel.GetDeclaredSymbol(recordDeclaration);
            if (recordSymbol == null)
                return;

            // Determine which rule to use based on the record type
            var (isTarget, rule) = GetApplicableRule(recordSymbol);

            if (!isTarget)
                return;

            // Check for Revit references in the record
            if (HasRevitReferences(recordDeclaration, semanticModel))
            {
                // Report diagnostic
                var diagnostic = Diagnostic.Create(rule, recordDeclaration.Identifier.GetLocation(), recordSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
        private bool IsTargetType(ClassDeclarationSyntax classDeclaration)
        {
            // Check if the class name ends with ViewModel, Query, or QueryResult
            string className = classDeclaration.Identifier.Text;
            return className.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase) ||
                   className.EndsWith("Query", StringComparison.OrdinalIgnoreCase) ||
                   className.EndsWith("QueryResult", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsTargetType(RecordDeclarationSyntax recordDeclaration)
        {
            // Check if the record name ends with ViewModel, Query, or QueryResult
            string recordName = recordDeclaration.Identifier.Text;
            return recordName.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase) ||
                   recordName.EndsWith("Query", StringComparison.OrdinalIgnoreCase) ||
                   recordName.EndsWith("QueryResult", StringComparison.OrdinalIgnoreCase);
        }

        private (bool isTarget, DiagnosticDescriptor rule) GetApplicableRule(INamedTypeSymbol classSymbol)
        {
            string className = classSymbol.Name;

            if (className.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase))
                return (true, ViewModelRule);

            if (className.EndsWith("Query", StringComparison.OrdinalIgnoreCase) && 
                !className.EndsWith("QueryResult", StringComparison.OrdinalIgnoreCase))
                return (true, QueryRule);

            if (className.EndsWith("QueryResult", StringComparison.OrdinalIgnoreCase))
                return (true, QueryResultRule);

            // Check interfaces as well
            foreach (var interfaceSymbol in classSymbol.AllInterfaces)
            {
                string interfaceName = interfaceSymbol.Name;
                
                if (interfaceName == "IQuery" || interfaceName.StartsWith("IQuery<"))
                    return (true, QueryRule);
                
                if (interfaceName == "IQueryResult" || interfaceName.StartsWith("IQueryResult<"))
                    return (true, QueryResultRule);
            }

            return (false, null);
        }

        private bool HasRevitReferences(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        {
            // Check for using directives with Autodesk.Revit
            var root = classDeclaration.SyntaxTree.GetRoot();
            var usingDirectives = root.DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Where(u => u.Name.ToString().StartsWith("Autodesk.Revit"));
            
            if (usingDirectives.Any())
                return true;

            // Check for property or field types that are from Autodesk.Revit
            var members = classDeclaration.Members;

            foreach (var member in members)
            {
                if (member is PropertyDeclarationSyntax property)
                {
                    var propertyType = semanticModel.GetTypeInfo(property.Type).Type;
                    if (propertyType != null && IsRevitType(propertyType))
                        return true;
                }
                else if (member is FieldDeclarationSyntax field)
                {
                    var fieldType = semanticModel.GetTypeInfo(field.Declaration.Type).Type;
                    if (fieldType != null && IsRevitType(fieldType))
                        return true;
                }
            }

            // Check for method parameter and return types that are from Autodesk.Revit
            foreach (var member in members.OfType<MethodDeclarationSyntax>())
            {
                // Check return type
                var returnType = semanticModel.GetTypeInfo(member.ReturnType).Type;
                if (returnType != null && IsRevitType(returnType))
                    return true;

                // Check parameters
                foreach (var param in member.ParameterList.Parameters)
                {
                    var paramType = semanticModel.GetTypeInfo(param.Type).Type;
                    if (paramType != null && IsRevitType(paramType))
                        return true;
                }
            }

            return false;
        }

        private bool HasRevitReferences(RecordDeclarationSyntax recordDeclaration, SemanticModel semanticModel)
        {
            // Check for using directives with Autodesk.Revit
            var root = recordDeclaration.SyntaxTree.GetRoot();
            var usingDirectives = root.DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Where(u => u.Name.ToString().StartsWith("Autodesk.Revit"));
            
            if (usingDirectives.Any())
                return true;

            // Check record parameters (primary constructor)
            if (recordDeclaration.ParameterList != null)
            {
                foreach (var param in recordDeclaration.ParameterList.Parameters)
                {
                    var paramType = semanticModel.GetTypeInfo(param.Type).Type;
                    if (paramType != null && IsRevitType(paramType))
                        return true;
                }
            }

            // Check for property or field types that are from Autodesk.Revit
            var members = recordDeclaration.Members;

            foreach (var member in members)
            {
                if (member is PropertyDeclarationSyntax property)
                {
                    var propertyType = semanticModel.GetTypeInfo(property.Type).Type;
                    if (propertyType != null && IsRevitType(propertyType))
                        return true;
                }
                else if (member is FieldDeclarationSyntax field)
                {
                    var fieldType = semanticModel.GetTypeInfo(field.Declaration.Type).Type;
                    if (fieldType != null && IsRevitType(fieldType))
                        return true;
                }
            }

            // Check for method parameter and return types that are from Autodesk.Revit
            foreach (var member in members.OfType<MethodDeclarationSyntax>())
            {
                // Check return type
                var returnType = semanticModel.GetTypeInfo(member.ReturnType).Type;
                if (returnType != null && IsRevitType(returnType))
                    return true;

                // Check parameters
                foreach (var param in member.ParameterList.Parameters)
                {
                    var paramType = semanticModel.GetTypeInfo(param.Type).Type;
                    if (paramType != null && IsRevitType(paramType))
                        return true;
                }
            }

            return false;
        }

        private bool IsRevitType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                return false;

            // Check if the type is from Autodesk.Revit namespace
            var containingNamespace = typeSymbol.ContainingNamespace;
            while (containingNamespace != null)
            {
                if (containingNamespace.Name == "Revit" && 
                    containingNamespace.ContainingNamespace?.Name == "Autodesk")
                    return true;

                containingNamespace = containingNamespace.ContainingNamespace;
            }

            return false;
        }
    }
}
