using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap.Abstract;
using System.Abstract.IntegationTests.ServiceLocator;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    [TestClass]
    public class StructureMapServiceLocatorIntegrationTests : AbstractServiceLocatorTest
    {
        protected override IServiceLocator CreateServiceLocator() { return new StructureMapServiceLocator(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void Resolve_Should_Return_Valid_Instance() { base.Resolve_Should_Return_Valid_Instance(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void GenericAndNonGeneric_Resolve_Method_Should_Return_Same_Instance_Type() { base.GenericAndNonGeneric_Resolve_Method_Should_Return_Same_Instance_Type(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void Asking_For_UnRegistered_Service_Return_Valid_Instance() { base.Asking_For_UnRegistered_Service_Return_Valid_Instance(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public override void Asking_For_Invalid_Service_Should_Raise_Exception() { base.Asking_For_Invalid_Service_Should_Raise_Exception(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public override void Asking_For_Invalid_Service_Should_Raise_Exception2() { base.Asking_For_Invalid_Service_Should_Raise_Exception2(); }

        #region Named Instances

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void Ask_For_Named_Instance() { base.Ask_For_Named_Instance(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void GenericAndNonGeneric_Resolve_Named_Instance_Should_Return_Same_Instance_Type() { base.GenericAndNonGeneric_Resolve_Named_Instance_Should_Return_Same_Instance_Type(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public override void Ask_For_Unknown_Service_Should_Throw_Exception() { base.Ask_For_Unknown_Service_Should_Throw_Exception(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public override void Ask_For_Unknown_Service_Should_Throw_Exception2() { base.Ask_For_Unknown_Service_Should_Throw_Exception2(); }

        #endregion

        #region ResolveAll

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void ResolveAll_Should_Return_All_Registered_UnNamed_Services() { base.ResolveAll_Should_Return_All_Registered_UnNamed_Services(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void ResolveAll_Should_Return_All_Registered_Named_Services() { base.ResolveAll_Should_Return_All_Registered_Named_Services(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void ResolveAll_For_Unknown_Type_Should_Return_Empty_Enumerable() { base.ResolveAll_For_Unknown_Type_Should_Return_Empty_Enumerable(); }

        [TestMethod, TestCategory("ServiceLocator: StructureMap")]
        public override void GenericAndNonGeneric_ResolveAll_Should_Return_Same_Instace_Types() { base.GenericAndNonGeneric_ResolveAll_Should_Return_Same_Instace_Types(); }

        #endregion
    }
}
