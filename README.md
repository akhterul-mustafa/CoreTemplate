# net-core-20-service-template
Based on the "Keep It Tight" project written by Rene Herrero, found in the [DCS-Architecture repo](http://git.dell.com/DcsArchitecture/PCF-Samples).

This repository contains a template facilitating the rapid creation of .NET Core 2.0 microservice/WebAPI solutions. Right now, the targeted deployment platform for these services is Pivotal CloudFoundry (PCF) and that will be the context of the examples provided below.

### Primary elements
1.) Minimal footprint
* When using Visual Studio to create a WebAPI project, the entirety of the .NET Core WebAPI/MVC stack is included as dependencies of the project. In order to keep the footprint small and streamlined, only the references specific to a REST-based API using JSON formatting have been retained.

2.) Logging
* This solution uses the default .NET logging for now.
* Default logging levels are set to Warning. However logging messages from any assembly starting with "Dell" having a log level of "Information" or higher are explicitly piped to the Console. This is required for logging visibility in PCF.

3.) Correlation
* In our IIS-hosted services, we utilize an IHttpModule implementation to get/set a CorrelationId HTTP header for every request. HttpModules have been replaced with the concept of [Middleware](https://docs.microsoft.com/en-us/aspnet/core/migration/http-modules) in ASP.NET Core. Refer to the "Middleware" directory in this project for the implementation of CorrelationId handling.

4.) Health Check
* PCF uses a health check to monitor the health and availability of a given service. The implementation provided in this sample is an HTTP health check whereby the root of the application is called. See the manifest-SampleService-Dev.yml file.

5.) SwaggerUI
* This sample application is integrated with the .NET Core version of [Swagger UI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore). You should be able to access the UI via the following path.

### Testing Locally
Because the project has been stripped down to only include the minimal assemblies, you cannot use IIS Express via Visual Studio to debug these applications. You must use the dotnet command line in order to start the application, and then attach to the process.

Example:
```
cd <project_dir>\bin\Debug\netcoreapp2.0
dotnet Dell.Solution.Service.Sample.dll
```
At that point you should be able to call the relevant URLs via localhost on the specified port.
```
http://localhost:5000/api/values
```
Local Swagger UI should be available here.
```
http://localhost:5000/swagger
```

### Deploying to CloudFoundry
Assuming you have access to a CloudFoundry space, you can deploy the application.
```
cf push -f manifest-SampleService-Dev.yml
```
NOTE: During the deployment, you will see the process pause at the following step.
```
Publishing application using Dotnet CLI
```
This can take several minutes. It will eventually complete. Once completed, and assuming the default configuration in this project, you should be able to hit the following endpoint:
```
https://sampleservice-dev.cfapps.pcf1.vc1.pcf.dell.com/api/values
```
Example Swagger UI should be available here.
```
https://sampleservice-dev.cfapps.pcf1.vc1.pcf.dell.com/swagger
```

### Regarding DRY versus Code Proliferation
In a distributed microservices eco-system, you have a choice whereby you can use common libraries for things like shared implementation or service contracts. An example of shared implementation would be the CorrelationId handling above. However this shared logic is frequently packaged as Nugets with the justification that it only has to be written once and can be distributed everywhere. This is true, but as we found out on the Phoenix project, it also leads to coupling between teams and applications. Therefore I am advocating that we embrace DRY (Don't Repeat Yourself) within the context of a given microservice, but be very wary of doing so via shared libraries across microservices. Shared libraries cause all microservices that consume them to go through a Nuget process, which can be a bottleneck if the code in the library is volatile (such as during new development).

Borrowing an idea from "[Building Microservices](https://www.amazon.com/Building-Microservices-Designing-Fine-Grained-Systems/dp/1491950358/)" by Sam Newman, this service template can be thought of as the equivalent of a compiled library distributed via Nuget. If you create a service based on this template, then your shared implementation will always be common. If your service needs to tweak some aspect of the shared implementation, then go ahead and do so for your particular service.

Over time, we may add to or change this template as standards and technologies evolve. Other kinds of specialized templates for different kinds of services may also be created.
