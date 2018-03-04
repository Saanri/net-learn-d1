using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using RoslynAnalizer;
using System.Web.Mvc;

namespace RoslynAnalizer.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void TestInheritanceController()
        {
            var test = @"
    using System;
    using System.Web.Mvc;

    namespace ConsoleApplication1
    {
        [Authorize]
        class MyClass : Controller
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("InheritanceControllerMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        [TestMethod]
        public void TestControllerMustBeAuthorize()
        {
            var test = @"
    using System;
    using System.Web.Mvc;

    namespace ConsoleApplication1
    {
        class MyClassController : Controller
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("ControllerMustBeAuthorizeMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        [TestMethod]
        public void TestEntitiesMastHaveId()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1.Entities
    {
        [DataContract]
        public class MyClass
        {   
            public string name;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("EntitiesMastHaveIdMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 22)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        [TestMethod]
        public void TestEntitiesMastHaveName()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1.Entities
    {
        [DataContract]
        public class MyClass
        {   
            public string id;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("EntitiesMastHaveNameMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 22)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        [TestMethod]
        public void TestEntitiesMastBePublic()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1.Entities
    {
        [DataContract]
        class MyClass
        {   
            public string id;
            public string name;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("EntitiesMastBePublicMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        [TestMethod]
        public void TestEntitiesMastBeDataContract()
        {
            var test = @"
    using System;

    namespace ConsoleApplication1.Entities
    {
        public class MyClass
        {   
            public string id;
            public string name;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RoslynAnalizer",
                Message = String.Format("EntitiesMastBeDataContractMessageFormat"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 6, 22)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        /*
                protected override CodeFixProvider GetCSharpCodeFixProvider()
                {
                    return new RoslynAnalizerCodeFixProviderderder();
                }
        */
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new RoslynAnalizerAnalyzer();
        }
    }
}