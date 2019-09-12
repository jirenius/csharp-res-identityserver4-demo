using ResgateIO.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AwesomeTicker
{
    class Program
    {
        static int Count = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("AwesomeTicker microservice");

            ResService service = new ResService("awesomeTicker");
            service.AddHandler("ticker", new DynamicHandler()
                .SetModelGet(req => req.Model(new { count = Count })));

            service.Serve("nats://127.0.0.1:4222");

            Timer timer = new Timer(e =>
            {
                service.With("awesomeTicker.ticker", resource =>
                {
                    Count++;
                    resource.ChangeEvent(new Dictionary<string, object> { { "count", Count } });
                });
            }, null, 1000, 1000);

            Console.ReadLine();
            service.Shutdown();
        }
    }
}
