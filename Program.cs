
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

BookingService bookingService = new BookingService();
RoomService roomService = new RoomService();

while (true)
{
    Console.WriteLine("\n--- Holiday Maker ---");
    Console.WriteLine("1. Sök rum");
    Console.WriteLine("2. Sök rum efter ort");
    Console.WriteLine("3. Sök rum efter antal personer");
    Console.WriteLine("4. Skapa bokning");
    Console.WriteLine("5. Visa kundens bokningar");
    Console.WriteLine("6. Ändra antal personer");
    Console.WriteLine("7. Avboka bokning");
    Console.WriteLine("8. Avsluta");


    Console.Write("Välj: ");
    string choice = Console.ReadLine()!;
    if (choice == "1")
    {
        roomService.SearchRooms(connection);
    }
    else if (choice == "2")
    {
        roomService.GetRoomsByTown(connection);
    }
    else if (choice == "3")
    {
        roomService.CheckAmountOfPeople(connection);
    }
    else if (choice == "4")
    {
        bookingService.CreateBooking(connection);
    }
    else if (choice == "5")
    {
        bookingService.ShowCustomerBookings(connection);
    }
    else if (choice == "6")
    {
        bookingService.ChangeAmountOfPeople(connection);
    }
    else if (choice == "7")
    {
        bookingService.CancelBooking(connection);
    }
    else if (choice == "8")
    {
        break;
    }
    else
    {
        Console.WriteLine("Ogiltigt val.");
    }
}