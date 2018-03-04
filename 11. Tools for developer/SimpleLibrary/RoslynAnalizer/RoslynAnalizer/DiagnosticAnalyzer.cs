using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynAnalizer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslynAnalizerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RoslynAnalizer";

        private static readonly LocalizableString TitleInheritanceController = new LocalizableResourceString(nameof(Resources.InheritanceControllerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatInheritanceController = new LocalizableResourceString(nameof(Resources.InheritanceControllerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionInheritanceController = new LocalizableResourceString(nameof(Resources.InheritanceControllerDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryInheritanceController = "InheritanceController";

        private static readonly LocalizableString TitleControllerMustBeAuthorize = new LocalizableResourceString(nameof(Resources.ControllerMustBeAuthorizeTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatControllerMustBeAuthorize = new LocalizableResourceString(nameof(Resources.ControllerMustBeAuthorizeMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionControllerMustBeAuthorize = new LocalizableResourceString(nameof(Resources.ControllerMustBeAuthorizeDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryControllerMustBeAuthorize = "ControllerMustBeAuthorize";

        private static readonly LocalizableString TitleEntitiesMastHaveId = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveIdTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatEntitiesMastHaveId = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveIdMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionEntitiesMastHaveId = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveIdDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryEntitiesMastHaveId = "EntitiesMastHaveId";

        private static readonly LocalizableString TitleEntitiesMastHaveName = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveNameTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatEntitiesMastHaveName = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveNameMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionEntitiesMastHaveName = new LocalizableResourceString(nameof(Resources.EntitiesMastHaveNameDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryEntitiesMastHaveName = "EntitiesMastHaveName";

        private static readonly LocalizableString TitleEntitiesMastBePublic = new LocalizableResourceString(nameof(Resources.EntitiesMastBePublicTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatEntitiesMastBePublic = new LocalizableResourceString(nameof(Resources.EntitiesMastBePublicMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionEntitiesMastBePublic = new LocalizableResourceString(nameof(Resources.EntitiesMastBePublicDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryEntitiesMastBePublic = "EntitiesMastBePublic";

        private static readonly LocalizableString TitleEntitiesMastBeDataContract = new LocalizableResourceString(nameof(Resources.EntitiesMastBeDataContractTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatEntitiesMastBeDataContract = new LocalizableResourceString(nameof(Resources.EntitiesMastBeDataContractMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionEntitiesMastBeDataContract = new LocalizableResourceString(nameof(Resources.EntitiesMastBeDataContractDescription), Resources.ResourceManager, typeof(Resources));
        private const string CategoryEntitiesMastBeDataContract = "EntitiesMastBePublic";

        private static DiagnosticDescriptor RuleInheritanceController = new DiagnosticDescriptor(DiagnosticId, TitleInheritanceController, MessageFormatInheritanceController, CategoryInheritanceController, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionInheritanceController);
        private static DiagnosticDescriptor RuleControllerMustBeAuthorize = new DiagnosticDescriptor(DiagnosticId, TitleControllerMustBeAuthorize, MessageFormatControllerMustBeAuthorize, CategoryControllerMustBeAuthorize, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionControllerMustBeAuthorize);
        private static DiagnosticDescriptor RuleEntitiesMastHaveId = new DiagnosticDescriptor(DiagnosticId, TitleEntitiesMastHaveId, MessageFormatEntitiesMastHaveId, CategoryEntitiesMastHaveId, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionEntitiesMastHaveId);
        private static DiagnosticDescriptor RuleEntitiesMastHaveName = new DiagnosticDescriptor(DiagnosticId, TitleEntitiesMastHaveName, MessageFormatEntitiesMastHaveName, CategoryEntitiesMastHaveName, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionEntitiesMastHaveName);
        private static DiagnosticDescriptor RuleEntitiesMastBePublic = new DiagnosticDescriptor(DiagnosticId, TitleEntitiesMastBePublic, MessageFormatEntitiesMastBePublic, CategoryEntitiesMastBePublic, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionEntitiesMastBePublic);
        private static DiagnosticDescriptor RuleEntitiesMastBeDataContract = new DiagnosticDescriptor(DiagnosticId, TitleEntitiesMastBeDataContract, MessageFormatEntitiesMastBeDataContract, CategoryEntitiesMastBeDataContract, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DescriptionEntitiesMastBeDataContract);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(RuleInheritanceController, RuleControllerMustBeAuthorize, RuleEntitiesMastHaveId, RuleEntitiesMastHaveName, RuleEntitiesMastBePublic, RuleEntitiesMastBeDataContract); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name.Equals("Controller"))
            {
                if (!namedTypeSymbol.Name.EndsWith("Controller"))
                {
                    var diagnostic = Diagnostic.Create(RuleInheritanceController, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                if (!namedTypeSymbol.GetAttributes().Any(a => a.AttributeClass.Name.Equals("Authorize") || a.AttributeClass.Name.Equals("AuthorizeAttribute")))
                {
                    var diagnostic = Diagnostic.Create(RuleControllerMustBeAuthorize, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }

            if (namedTypeSymbol.ContainingNamespace.Name.Equals("Entities"))
            {
                if (!namedTypeSymbol.MemberNames.Any(m => m.Equals("id")))
                {
                    var diagnostic = Diagnostic.Create(RuleEntitiesMastHaveId, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                if (!namedTypeSymbol.MemberNames.Any(m => m.Equals("name")))
                {
                    var diagnostic = Diagnostic.Create(RuleEntitiesMastHaveName, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                if (!namedTypeSymbol.GetAttributes().Any(a => a.AttributeClass.Name.Equals("DataContract")))
                {
                    var diagnostic = Diagnostic.Create(RuleEntitiesMastBeDataContract, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                if (!namedTypeSymbol.DeclaredAccessibility.Equals(Accessibility.Public))
                {
                    var diagnostic = Diagnostic.Create(RuleEntitiesMastBePublic, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
