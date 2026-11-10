
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

void CheckAmountOfPeople(MySqlConnection connection)
{
    string checkMaxAmountOfPeople = """
        SELECT
            Rum.id,
            Rum.rumsnummer,
            Rum.max_personer,
            Rum.pris_per_natt,
            Boende.namn,
            Boende.ort
        FROM Rum
        JOIN Boende
            ON Rum.boende_id = Boende.id
        WHERE Rum.max_personer >= @antalPersoner;
        """;

    Console.Write("Enter amount of people: ");
    int antalpersoner = int.Parse(Console.ReadLine()!);

    using var peopleCommand =
        new MySqlCommand(checkMaxAmountOfPeople, connection);

    peopleCommand.Parameters.AddWithValue(
        "@antalPersoner",
        antalpersoner);

    using var peopleReader = peopleCommand.ExecuteReader();

    bool foundMaxPeople = false;

    while (peopleReader.Read())
    {
        foundMaxPeople = true;

        Console.WriteLine(
            $"Rumsnummer: {peopleReader["rumsnummer"]}: {peopleReader["namn"]}\n" +
            $"{peopleReader["ort"]} : Max antal personer: " +
            $"{peopleReader["max_personer"]} : " +
            $"{peopleReader["pris_per_natt"]}kr\n"
        );
    }

    if (!foundMaxPeople)
    {
        Console.WriteLine(
            "Det finns inga rum som rymmer så många personer.");
    }
}

void GetRoomsByTown(MySqlConnection connection)
{
    string getRooms = """
        SELECT
            Rum.id,
            Rum.rumsnummer,
            Rum.max_personer,
            Rum.pris_per_natt,
            Boende.namn,
            Boende.ort
        FROM Rum
        JOIN Boende
            ON Rum.boende_id = Boende.id
        WHERE Boende.ort = @ort;
        """;

    Console.Write("Ange ort: ");
    string ort = Console.ReadLine()!;

    using var roomCommand =
        new MySqlCommand(getRooms, connection);

    roomCommand.Parameters.AddWithValue("@ort", ort);

    using var roomReader = roomCommand.ExecuteReader();

    bool foundRoom = false;

    while (roomReader.Read())
    {
        foundRoom = true;

        Console.WriteLine(
            $"Rumsnummer: {roomReader["rumsnummer"]}: " +
            $"{roomReader["namn"]}\n" +
            $"{roomReader["ort"]} : Max antal personer: " +
            $"{roomReader["max_personer"]} : " +
            $"{roomReader["pris_per_natt"]}kr\n"
        );
    }

    if (!foundRoom)
    {
        Console.WriteLine("Det finns inga rum på den orten.");
    }
}

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
        GetRoomsByTown(connection);
    }
    else if (choice == "2")
    {
        CheckAmountOfPeople(connection);
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