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

string sql = "SELECT * FROM Boende";

using var command = new MySqlCommand(sql, connection);
using var reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(
        $"{reader["id"]}: {reader["namn"]} - {reader["ort"]}"
    );
}

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
        ON Rum.boende_id = Boende.id;
    """;

    