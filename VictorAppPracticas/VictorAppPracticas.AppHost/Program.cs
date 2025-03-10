var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.VictorAppPracticas_ApiService>("apiservice");

builder.AddProject<Projects.VictorAppPracticas_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
