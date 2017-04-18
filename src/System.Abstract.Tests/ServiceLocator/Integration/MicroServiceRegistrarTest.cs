using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceLocator;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    [TestClass]
    public class MicroServiceRegistrarTest : AbstractServiceRegistrarTest
    {
        protected override IServiceLocator CreateServiceLocator() { return new MicroServiceLocator(); }

        #region Enumerate

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void HasRegistered() { base.HasRegistered(); }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void GetRegistrationsFor() { base.GetRegistrationsFor(); }

        #endregion

        #region Register Type
        #endregion

        #region Register Implementation

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_Specified_Type_Should_Return_Same_Type() { base.Register_With_Specified_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_Implementation_Type_Should_Return_Same_Type() { base.Register_With_Implementation_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_Keyed_Type_Should_Return_Same_Type() { base.Register_With_Keyed_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_Specified_Service_And_Type_Should_Return_Same_Type() { base.Register_With_Specified_Service_And_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_Specified_Service_Should_Return_Same_Type() { base.Register_With_Specified_Service_Should_Return_Same_Type(); }
        
        #endregion

        #region Register Instance

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void RegisterInstance_Generic_Should_Return_Same_Object() { }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void RegisterInstance_GenericNamed_Should_Return_Same_Object() { }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void RegisterInstance_Should_Return_Same_Object() { }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void RegisterInstance_Named_Should_Return_Same_Object() { }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void RegisterInstance_Should_Return_Same_Object_For_Same_Type() { }

        #endregion

        #region Register Method

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_Generic_With_FactoryMethod_Should_Return_Result_From_Factory() { }

        [TestMethod, TestCategory("ServiceLocator: Micro")]
        public override void Register_With_FactoryMethod_Should_Return_Result_From_Factory() { }

        #endregion
    }
}