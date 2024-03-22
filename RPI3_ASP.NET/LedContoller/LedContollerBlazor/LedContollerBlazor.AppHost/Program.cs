var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LedContollerBlazor>("ledcontollerblazor");

builder.Build().Run();
