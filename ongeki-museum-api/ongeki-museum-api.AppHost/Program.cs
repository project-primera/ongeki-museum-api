var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ongeki_museum_api>("ongeki-museum-api");

builder.Build().Run();
