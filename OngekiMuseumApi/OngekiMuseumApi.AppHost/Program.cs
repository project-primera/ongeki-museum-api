var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OngekiMuseumApi>("ongekimuseumapi");

builder.Build().Run();
