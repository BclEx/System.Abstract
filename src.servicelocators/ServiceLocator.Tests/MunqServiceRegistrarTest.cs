﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !NET35
using Munq.Abstract;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceLocator;

namespace ServiceLocator.Tests
{
    [TestClass]
    public class MunqServiceRegistrarTest : AbstractServiceRegistrarTest
    {
        protected override IServiceLocator CreateServiceLocator() { return new MunqServiceLocator(); }

        #region Enumerate

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void HasRegistered() { base.HasRegistered(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void GetRegistrationsFor() { base.GetRegistrationsFor(); }

        #endregion

        #region Register Type
        #endregion

        #region Register Implementation

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_Specified_Type_Should_Return_Same_Type() { base.Register_With_Specified_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_Implementation_Type_Should_Return_Same_Type() { base.Register_With_Implementation_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_Keyed_Type_Should_Return_Same_Type() { base.Register_With_Keyed_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_Specified_Service_And_Type_Should_Return_Same_Type() { base.Register_With_Specified_Service_And_Type_Should_Return_Same_Type(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_Specified_Service_Should_Return_Same_Type() { base.Register_With_Specified_Service_Should_Return_Same_Type(); }
        
        #endregion

        #region Register Instance

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void RegisterInstance_Generic_Should_Return_Same_Object() { base.RegisterInstance_Generic_Should_Return_Same_Object(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void RegisterInstance_GenericNamed_Should_Return_Same_Object() { base.RegisterInstance_GenericNamed_Should_Return_Same_Object(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void RegisterInstance_Should_Return_Same_Object() { base.RegisterInstance_Should_Return_Same_Object(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void RegisterInstance_Named_Should_Return_Same_Object() { base.RegisterInstance_Named_Should_Return_Same_Object(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void RegisterInstance_Should_Return_Same_Object_For_Same_Type() { base.RegisterInstance_Should_Return_Same_Object_For_Same_Type(); }

        #endregion

        #region Register Method

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_Generic_With_FactoryMethod_Should_Return_Result_From_Factory() { base.Register_Generic_With_FactoryMethod_Should_Return_Result_From_Factory(); }

        [TestMethod, TestCategory("ServiceLocator: Munq")]
        public override void Register_With_FactoryMethod_Should_Return_Result_From_Factory() { base.Register_With_FactoryMethod_Should_Return_Result_From_Factory(); }

        #endregion
    }
}
#endif