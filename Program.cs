using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json")
.Build();

string connectionString =
configuration.GetConnectionString("Default")!;

Console.WriteLine("Connection string loaded!");