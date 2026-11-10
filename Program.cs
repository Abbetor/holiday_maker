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



using var roomCommand = new MySqlCommand(getRooms, connection);
roomCommand.Parameters.AddWithValue("@ort", ort);
using var roomReader = roomCommand.ExecuteReader();
bool foundRoom = false;
while (roomReader.Read())
{
    foundRoom = true;
    Console.WriteLine($"Rumsnummer: {roomReader["rumsnummer"]}: {roomReader["namn"]}\n{roomReader["ort"]} : Max antal personer: {roomReader["max_personer"]} : {roomReader["pris_per_natt"]}kr\n");
}
if (!foundRoom)
{
    Console.WriteLine("Det finns inga rum på den orten.");
}

