using MySqlConnector;

class BookingService
{
    public void CreateBooking(MySqlConnection connection)
    {
        string searchCustomers = """
            SELECT id
            FROM Kund;
            """;
        using (var createCommand = new MySqlCommand(searchCustomers, connection))
        using (var CustomerReader = createCommand.ExecuteReader())
        {
            while (CustomerReader.Read())
            {
                Console.WriteLine(
                    $"Kund id: {CustomerReader["id"]}"
                );
            }
        }

        int kundid = InputHelper.ReadInt("Ange kund id: ", 1);

        

        int antalpersoner = InputHelper.ReadInt("Ange antal personer: ", 1);

        DateTime checkIn = InputHelper.ReadDate("Incheckning (yyyy-MM-dd): ");

        DateTime checkOut = InputHelper.ReadDate("Utcheckning (yyyy-MM-dd): ");

        if (checkIn < DateTime.Today)
        {
            Console.WriteLine("Incheckningen kan inte vara i det förflutna.");
            return;
        }

        if (checkOut <= checkIn)
        {
            Console.WriteLine("Utcheckningen måste vara minst en dag efter incheckningen.");
            return;
        }


        string searchRooms = """
            SELECT id, rumsnummer
            FROM Rum;
            """;

        using (var roomCommand = new MySqlCommand(searchRooms, connection))
        using (var roomReader = roomCommand.ExecuteReader())

        while (roomReader.Read())
        {
            Console.WriteLine(
                $"Rum id: {roomReader["id"]}, " +
                $"Rumsnummer: {roomReader["rumsnummer"]}"
            );
        }

        int rumid = InputHelper.ReadInt("Ange rum id: ", 1);

        string checkCapacity = """
        SELECT max_personer
        FROM Rum
        WHERE id = @rumid;
        """;

        int maxPersoner;

        using (var capacityCommand = new MySqlCommand(checkCapacity, connection))
        {
            capacityCommand.Parameters.AddWithValue("@rumid", rumid);

            using (var capacityReader = capacityCommand.ExecuteReader())
            {
                if (!capacityReader.Read())
                {
                    Console.WriteLine("Rummet finns inte.");
                    return;
                }

                maxPersoner = Convert.ToInt32(capacityReader["max_personer"]);
            }
        }

        if (antalpersoner <= 0)
        {
            Console.WriteLine("Antalet personer måste vara minst 1.");
            return;
        }

        if (antalpersoner > maxPersoner)
        {
            Console.WriteLine(
                $"Rummet har plats för max {maxPersoner} personer.");
            return;
        }

        string checkRoom = """
            SELECT id
            FROM Rum
            WHERE id = @rumid
            AND NOT EXISTS (
                SELECT 1 FROM Bokning
                WHERE Bokning.rum_id = Rum.id
                AND Bokning.incheckning < @CheckOut
                AND Bokning.utcheckning > @CheckIn     
            );
        """;
        
        bool roomIsAvailable;

        using (var checkRoomCommand = new MySqlCommand(checkRoom, connection))
        {
            checkRoomCommand.Parameters.AddWithValue("@rumid", rumid);
            checkRoomCommand.Parameters.AddWithValue("@CheckIn", checkIn);
            checkRoomCommand.Parameters.AddWithValue("@CheckOut", checkOut);

            using (var checkRoomReader = checkRoomCommand.ExecuteReader())
            {
                roomIsAvailable = checkRoomReader.Read();
            }
        }
        if (roomIsAvailable)
        {
            Console.WriteLine("Rummet är ledigt.");

            string createBooking = """
                INSERT INTO Bokning
                    (antal_personer, kund_id, rum_id, incheckning, utcheckning)
                VALUES
                    (@antalpersoner, @kundid, @rumid, @checkIn, @checkOut);
                """;

            using var bookingCommand = new MySqlCommand(createBooking, connection);

            bookingCommand.Parameters.AddWithValue("@antalpersoner", antalpersoner);
            bookingCommand.Parameters.AddWithValue("@kundid", kundid);
            bookingCommand.Parameters.AddWithValue("@rumid", rumid);
            bookingCommand.Parameters.AddWithValue("@checkIn", checkIn);
            bookingCommand.Parameters.AddWithValue("@checkOut", checkOut);

            bookingCommand.ExecuteNonQuery();

            Console.WriteLine("Bokning lagd.");
        }


        else
        {
            Console.WriteLine("Rummet är inte ledigt under dessa datum.");
        }

    }

    public void ShowCustomerBookings(MySqlConnection connection)
    {
        int kundid = InputHelper.ReadInt("Ange kund id: ", 1);

        string searchBookings = """
            SELECT
                Bokning.id,
                Bokning.antal_personer,
                Bokning.incheckning,
                Bokning.utcheckning,
                Rum.rumsnummer,
                Boende.namn,
                Boende.ort
            FROM Bokning
            JOIN Rum
                ON Bokning.rum_id = Rum.id
            JOIN Boende
                ON Rum.boende_id = Boende.id
            WHERE Bokning.kund_id = @kundid;
            """;

        using (var bookingCommand = new MySqlCommand(searchBookings, connection))
        {
            bookingCommand.Parameters.AddWithValue("@kundid", kundid);

            using (var bookingReader = bookingCommand.ExecuteReader())
            {
                bool foundbooking = false;
                while (bookingReader.Read())
                {
                    foundbooking = true;
                    Console.WriteLine(
                    $"Bokning id: {bookingReader["id"]}\n" +
                    $"Boende: {bookingReader["namn"]}\n" +
                    $"Ort: {bookingReader["ort"]}\n" +
                    $"Rum: {bookingReader["rumsnummer"]}\n" +
                    $"Antal personer: {bookingReader["antal_personer"]}\n" +
                    $"Incheckning: {bookingReader["incheckning"]}\n" +
                    $"Utcheckning: {bookingReader["utcheckning"]}\n"
                    );
                }
                if (!foundbooking)
                {
                    Console.WriteLine("Kunden har inga bokningar.");
                }
            }
        }
    }

    public void ChangeAmountOfPeople(MySqlConnection connection)
    {
        int bokningid = InputHelper.ReadInt("Ange bokning id: ", 1);

        string searchBooking = """
            SELECT
                Bokning.antal_personer,
                Rum.max_personer
            FROM Bokning
            JOIN Rum
                ON Bokning.rum_id = Rum.id
            WHERE Bokning.id = @bokningid;
            """;

        int maxpersoner;

        using (var bookingCommand = new MySqlCommand(searchBooking, connection))
        {
            bookingCommand.Parameters.AddWithValue("@bokningid", bokningid);

            using (var bookingReader = bookingCommand.ExecuteReader())
            {
                if (!bookingReader.Read())
                {
                    Console.WriteLine("Bokningen finns inte.");
                    return;
                }

                Console.WriteLine(
                    $"Nuvarande antal personer: {bookingReader["antal_personer"]}"
                );

                maxpersoner = Convert.ToInt32(bookingReader["max_personer"]);
            }
        }

        int antalpersoner = InputHelper.ReadInt("Ange nytt antal personer: ", 1);

        if (antalpersoner <= 0)
        {
            Console.WriteLine("Antalet personer måste vara minst 1.");
            return;
        }

        if (antalpersoner > maxpersoner)
        {
            Console.WriteLine(
                $"Rummet har plats för max {maxpersoner} personer."
            );
            return;
        }

        string updateBooking = """
            UPDATE Bokning
            SET antal_personer = @antalpersoner
            WHERE id = @bokningid;
            """;

        using var updateCommand = new MySqlCommand(updateBooking, connection);

        updateCommand.Parameters.AddWithValue("@antalpersoner", antalpersoner);
        updateCommand.Parameters.AddWithValue("@bokningid", bokningid);

        updateCommand.ExecuteNonQuery();

        Console.WriteLine("Antalet personer har ändrats.");
    }

    public void CancelBooking(MySqlConnection connection)
    {
        int bokningid = InputHelper.ReadInt("Ange bokning id: ", 1);

        string checkBooking = """
            SELECT id
            FROM Bokning
            WHERE id = @bokningid;
            """;

        using (var bookingCommand = new MySqlCommand(checkBooking, connection))
        {
            bookingCommand.Parameters.AddWithValue("@bokningid", bokningid);

            using (var bookingReader = bookingCommand.ExecuteReader())
            {
                if (!bookingReader.Read())
                {
                    Console.WriteLine("Bokningen finns inte.");
                    return;
                }
            }
        }

        string cancelBooking = """
            DELETE FROM Bokning
            WHERE id = @bokningid;
            """;

        using var deleteCommand = new MySqlCommand(cancelBooking, connection);

        deleteCommand.Parameters.AddWithValue("@bokningid", bokningid);

        deleteCommand.ExecuteNonQuery();

        Console.WriteLine("Bokningen är avbokad.");
    }

}


