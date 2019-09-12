<p align="center"><a href="https://resgate.io" target="_blank" rel="noopener noreferrer"><img width="100" src="https://resgate.io/img/resgate-logo.png" alt="Resgate logo"></a></p>


<h2 align="center"><b>Identity Server 4 authentication<br>for Resgate</b></h2>
</p>

---

An Identity Server 4 example for [Resgate](https://github.com/resgateio/resgate), written in C#, demonstrating how to authenticate to the API using an *access token* provided via OpenID Connect. The example also shows how to revoke access to resources in real time when splitting up the roles of authentication, authorization, and serving resources, into three separate microservices.

## Prerequisite

* [Install and run](https://resgate.io/docs/get-started/installation/) *NATS Server* and *Resgate*.

## Install and run

* Clone *csharp-res-identityserver4-demo* repository:
    ```text
    git clone https://github.com/jirenius/csharp-res-identityserver4-demo
    ```
* Open the solution, `Quickstart.sln`, in Visual Studio 2017.
* Set at least the projects listed below as *startup projects* ([how to](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2019)).

  * *Authentication* - Sets the Resgate connection token using an access token
  * *Authorization* - Controls access to the Resgate API resources
  * *AwesomeTicker* - Resource that counts up every second
  * *IdentityServer* - In memory identity Server 4
  * *JavascriptClient* - Client that can login and display the ticker

* Press F5 to build and run.

## Things to try out

### Access the ticker
Access the javascript client at http://localhost:5003

In the Javascript client, click *Call API* to try to access the ticker resource.

### Revoke access to resource

While viewing the counter in the client, open the console for the *Authorization microservice*, and press *Enter* to have it switch from public access to private.

The client should immediately have the ticker resource unsubscribed, and the counter should stop.

### Regain access by logging in
In the client, click *Login* to be redirected to Identity Server's login page. Enter one of the user names:

* Username: *bob* or *alice*
* Password: *password*

Once the permissions are accepted, you will be redirected back. Click *Call API*, and you will once again see the counter.

### Lose access
Click on  *Logout* to logout. Once redirected back to the client, you will have no access to the ticker resource.
