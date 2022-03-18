using System;

namespace InvokeDynamically
{
    // Testing class. If it is invoke successfully by dynamically then it will print the message.
    public class TestingDI : ITestingDI
    {
        public void InvokeMe()
        {
            Console.WriteLine("Dynamically called with DI");
        }
    }
}
