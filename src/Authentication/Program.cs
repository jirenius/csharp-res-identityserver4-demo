using ResgateIO.Service;
using System.IdentityModel.Tokens.Jwt;
using System;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;

namespace Authentication
{
    class Program
    {
        const string authDomain = "http://localhost:5000";

        static void Main(string[] args)
        {
            Console.WriteLine("Authentication microservice");

            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(authDomain).GetAwaiter().GetResult();
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            ResService service = new ResService("authentication");
            service.AddHandler("user", new DynamicHandler()
                .AuthMethod("jwt", req =>
                {
                    string accessToken = (string)req.Params["token"];
                    if (String.IsNullOrEmpty(accessToken))
                    {
                        req.InvalidParams("Missing access token");
                        return;
                    }

                    // The GetUserInfo endpoint is used to both validate
                    // the access token and to get the user info.
                    // One might also wish to call the IntrospectionEndpoint
                    // to ensure the access token is still active.
                    var response = client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = disco.UserInfoEndpoint,
                        Token = accessToken
                    }).GetAwaiter().GetResult();

                    if (response.IsError)
                    {
                        req.Error(new ResError("authentication.invalidToken", response.Error));
                        return;
                    }

                    // Send the userInfo claims to Resgate.
                    // One could also include the access token claims, or any other arbitrary info, if needed.
                    req.TokenEvent(response.Json);
                    req.Ok();
                }));

            service.Serve("nats://127.0.0.1:4222");
            Console.ReadLine();
            service.Shutdown();
        }
    }
}
