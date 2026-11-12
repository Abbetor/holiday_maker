using MySqlConnector;

class CustomerService
{
    public void RegisterCustomer(MySqlConnection connection)
    {
        string namn = InputHelper.ReadText("Förnamn: ", 100);
        string efternamn = InputHelper.ReadText("Efternamn: ", 100);

        string email = InputHelper.ReadText("E-post: ", 255);
        while (!email.Contains('@'))
        {
            Console.WriteLine("E-postadressen måste innehålla @.");
            email = InputHelper.ReadText("E-post: ", 255);
        }

        string telefon = InputHelper.ReadText("Telefon: ", 30);

        string insertCustomer = """
            INSERT INTO Kund (namn, efternamn, email, telefon)
            VALUES (@namn, @efternamn, @email, @telefon);
            """;

        using var command = new MySqlCommand(insertCustomer, connection);
        command.Parameters.AddWithValue("@namn", namn);
        command.Parameters.AddWithValue("@efternamn", efternamn);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@telefon", telefon);

        command.ExecuteNonQuery();

        Console.WriteLine($"Kunden är registrerad med kund id: {command.LastInsertedId}");
    }

    public void ShowCustomers(MySqlConnection connection)
    {
        string getCustomers = """
            SELECT id, namn, efternamn, email
            FROM Kund
            ORDER BY efternamn, namn;
            """;

        using var command = new MySqlCommand(getCustomers, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(
                $"Kund id: {reader["id"]} - " +
                $"{reader["namn"]} {reader["efternamn"]} ({reader["email"]})"
            );
        }
    }
}
