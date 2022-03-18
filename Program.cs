using InvokeDynamically;
using Microsoft.Extensions.DependencyInjection;
using System;

public class Program
{
    public static void Main()
    {
        try
        {
            var serviceProvider = ConfigureContainer();
            var obj = serviceProvider.GetService<Methods>();

            obj.InvokeClassWithoutDI("WithoutDI", "GetMyClass");
            obj.InvokeClassWithDI("WithDI", "GetMyClass");

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    static IServiceProvider ConfigureContainer()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    // Configuring the Interface and Class as a singleton
    static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITestingDI, TestingDI>();
        services.AddSingleton<Methods>();
    }

    public class Methods
    {
        private static ITestingDI _testingDI;

        public Methods(ITestingDI testingDI)
        {
            _testingDI = testingDI;
        }

        public void InvokeClassWithoutDI(string myclass, string mymethod)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("The class without CTOR will be invoked.");
            Console.WriteLine("-----------------------------------------");

            try
            {
                var type = Type.GetType(myclass);
                var obj = Activator.CreateInstance(type);
                var method = type.GetMethod(mymethod);
                method.Invoke(obj, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InvokeClassWithDI(string className, string methodName)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("The class which has CTOR will be invoked.");
            Console.WriteLine("-----------------------------------------");

            try
            {
                var type = Type.GetType(className);

                var argumentType = new Type[] { typeof(ITestingDI) };

                var constructor = type.GetConstructor(argumentType);

                var classObject = constructor.Invoke(new object[] { _testingDI });

                var method = type.GetMethod(methodName);
                method.Invoke(classObject, new object[] { });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

public class WithoutDI
{
    public void GetMyClass()
    {
        Console.WriteLine("Dynamically called without DI");
    }
}

public class WithDI
{
    private static ITestingDI _testingDI;

    public WithDI(ITestingDI testingDI)
    {
        _testingDI = testingDI;
    }

    public void GetMyClass()
    {
        try
        {
            _testingDI.InvokeMe();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}
