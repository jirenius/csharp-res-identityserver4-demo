using ResgateIO.Service;
using System;

namespace Authorization
{
    class Program
    {
        static bool IsPublic = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Authorization microservice");

            ResService service = new ResService();
            service.AddHandler(">", new DynamicHandler()
                .Access(req =>
                {
                    // Validate the resource is either public, or we have a token, meaning we are logged in.
                    // If needed to, we can check the claims in the token to determine response.
                    if (IsPublic || req.Token != null)
                    {
                        req.AccessGranted();
                    }
                    else
                    {
                        req.AccessDenied();
                    }
                }));

            service.SetOwnedResources(null, new[] { ">" });
            service.Serve("nats://127.0.0.1:4222");

            while (true)
            {
                Console.WriteLine(IsPublic ? "Access for everyone" : "Access only for admins");
                if (Console.ReadLine() == "QUIT")
                {
                    break;
                }
                IsPublic = !IsPublic;
                service.ResetAll();
            }
            service.Shutdown();
        }
    }
}
