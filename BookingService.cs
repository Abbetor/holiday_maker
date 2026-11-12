using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
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

        Console.Write("Ange kund id: ");
        int kundid = int.Parse(Console.ReadLine()!);

        

        Console.Write("Ange antal personer: ");
        int antalpersoner = int.Parse(Console.ReadLine()!);

        Console.Write("Incheckning (yyyy-mm-dd): ");
        DateTime checkIn = DateTime.Parse(Console.ReadLine()!);

        Console.Write("Utcheckning (yyyy-mm-dd): ");
        DateTime checkOut = DateTime.Parse(Console.ReadLine()!);

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

        Console.Write("Ange rum id: ");
        int rumid = int.Parse(Console.ReadLine()!);

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
        Console.Write("Ange kund id: ");
        int kundid = int.Parse(Console.ReadLine()!);

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
}


