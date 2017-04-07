using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceLocator;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    public class MicroServiceRegistrarTest : AbstractServiceRegistrarTest
    {
        protected override IServiceLocator CreateServiceLocator() { return new MicroServiceLocator(); }

        #region Register Instance

        [TestMethod]
        public override void RegisterInstance_Generic_Should_Return_Same_Object() { }
        [TestMethod]
        public override void RegisterInstance_GenericNamed_Should_Return_Same_Object() { }
        [TestMethod]
        public override void RegisterInstance_Should_Return_Same_Object() { }
        [TestMethod]
        public override void RegisterInstance_Named_Should_Return_Same_Object() { }
        [TestMethod]
        public override void RegisterInstance_Should_Return_Same_Object_For_Same_Type() { }

        #endregion

        #region Register Method

        [TestMethod]
        public override void Register_Generic_With_FactoryMethod_Should_Return_Result_From_Factory() { }
        [TestMethod]
        public override void Register_With_FactoryMethod_Should_Return_Result_From_Factory() { }

        #endregion
    }
}