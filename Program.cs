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
    
string ort = "Ystad"; 
using var roomCommand = new MySqlCommand(getRooms, connection);
roomCommand.Parameters.AddWithValue("@ort", ort);
using var roomReader = roomCommand.ExecuteReader();

while (roomReader.Read())
{
    Console.WriteLine($"{roomReader["rumsnummer"]}: {roomReader["namn"]}: {roomReader["ort"]} : {roomReader["max_personer"]} : {roomReader["pris_per_natt"]}");
}