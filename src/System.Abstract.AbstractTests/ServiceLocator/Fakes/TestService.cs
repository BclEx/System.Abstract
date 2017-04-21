namespace System.Abstract.AbstractTests.ServiceLocator
{
    public interface ITestService { }
    public class TestService : ITestService { }
    public class TestServiceFuture : ITestService { }
    //public class TestService3 : ITestService { }
    public class TestServiceN: ITestService { }

    public class TestServiceWithServiceDependency : ITestService
    {
        [ServiceDependency]
        public ITestDependency DependencyThatDoesNotExistOnInterface { get; set; }
    }
}