using MySqlConnector;
class RoomService
{
    public void GetRoomsByTown(MySqlConnection connection)
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
    public void CheckAmountOfPeople(MySqlConnection connection)
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

        int antalpersoner = InputHelper.ReadInt("Skriv hur många personer: ", 1);

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

    public void SearchRooms(MySqlConnection connection)
    {
        string searchSpecificRooms = """
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
            WHERE Boende.ort = @ort
            AND Rum.max_personer >= @antalpersoner
            AND (@pool = FALSE OR Boende.pool = TRUE)
            AND (@restaurang = FALSE OR Boende.restaurang = TRUE)
            AND (@maxAvstandStrand = 0 OR Boende.avstand_strand <= @maxAvstandStrand)
            AND (@maxAvstandCentrum = 0 OR Boende.avstand_centrum <= @maxAvstandCentrum)

            AND NOT EXISTS (
                SELECT 1
                FROM Bokning
                WHERE Bokning.rum_id = Rum.id
                AND Bokning.incheckning < @checkOut
                AND Bokning.utcheckning > @checkIn
            )
            """;

        Console.Write("Ange ort: ");
        string town = Console.ReadLine()!;

        int amountOfPeople = InputHelper.ReadInt("Antal personer: ", 1);

        Console.Write("Måste boendet ha pool? (JA/NEJ): ");
        bool pool = (Console.ReadLine() ?? "").Trim().ToUpper() == "JA";

        Console.Write("Måste boendet ha restaurang? (JA/NEJ): ");
        bool resturant = (Console.ReadLine() ?? "").Trim().ToUpper() == "JA";

        int beachDistance = InputHelper.ReadInt("Max avstånd till strand i meter (0 = inget krav): ", 0);

        int centrumDistance = InputHelper.ReadInt("Max avstånd till centrum i meter (0 = inget krav): ", 0);

        DateTime checkIn = InputHelper.ReadDate("Incheckning (yyyy-MM-dd): ");

        DateTime checkOut = InputHelper.ReadDate("Utcheckning (yyyy-MM-dd): ");

        if (checkOut <= checkIn)
        {
            Console.WriteLine("Utcheckningen måste vara minst en dag efter incheckningen.");
            return;
        }

        Console.Write("Sortera efter pris eller omdöme: ");
        string sort = Console.ReadLine()!.ToUpper();

        if (sort == "PRIS")
        {
            searchSpecificRooms += " ORDER BY Rum.pris_per_natt ASC";
        }
        else if (sort == "OMDÖME")
        {
            searchSpecificRooms += " ORDER BY Boende.omdome DESC";
        }

        using var SearchCommand = new MySqlCommand(searchSpecificRooms, connection);
         
        SearchCommand.Parameters.AddWithValue("@ort", town);
        SearchCommand.Parameters.AddWithValue("@antalpersoner", amountOfPeople);
        SearchCommand.Parameters.AddWithValue("@pool", pool);
        SearchCommand.Parameters.AddWithValue("@restaurang", resturant);
        SearchCommand.Parameters.AddWithValue("@maxAvstandStrand", beachDistance);    
        SearchCommand.Parameters.AddWithValue("@maxAvstandCentrum", centrumDistance);
        SearchCommand.Parameters.AddWithValue("@checkIn", checkIn);
        SearchCommand.Parameters.AddWithValue("@checkOut", checkOut);

        using var SearchReader = SearchCommand.ExecuteReader();
        bool foundRoom = false;

        while (SearchReader.Read())
        {
            foundRoom = true;
            Console.WriteLine(
                $"Rumsnummer: {SearchReader["rumsnummer"]}: {SearchReader["namn"]}\n" +
                $"{SearchReader["ort"]} : Max antal personer: " +
                $"{SearchReader["max_personer"]} : " +
                $"{SearchReader["pris_per_natt"]}kr\n"
            );
        }

        if (!foundRoom)
        {
            Console.WriteLine(
                "Kriterierna matchade inga hotell");
        }
        
        
    }
}




