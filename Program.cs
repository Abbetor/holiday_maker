
using MySqlConnector;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString =
    configuration.GetConnectionString("Default")!;

Console.WriteLine("Connection string loaded!");

using var connection = new MySqlConnection(connectionString);
connection.Open();

Console.WriteLine("MySQL connection is working");

RoomService roomService = new RoomService();

while (true)
{
    Console.WriteLine("\n--- Holiday Maker ---");
    Console.WriteLine("1. Sök rum efter ort");
    Console.WriteLine("2. Sök rum efter antal personer");
    Console.WriteLine("3. Avsluta");

    Console.Write("Välj: ");
    string choice = Console.ReadLine()!;

    if (choice == "1")
    {
        roomService.GetRoomsByTown(connection);
    }
    else if (choice == "2")
    {
        roomService.CheckAmountOfPeople(connection);
    }
    else if (choice == "3")
    {
        break;
    }
    else
    {
        Console.WriteLine("Ogiltigt val.");
    }
}