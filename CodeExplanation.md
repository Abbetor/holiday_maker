# KodFörklaring – Holiday Maker

## Konfiguration

configurationen från appsettings.json och läggs sparar konfigurationsobjektet variabeln "configuration".

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
```

Här skapar jag en Variabel som heter connectionString som lagrar värdet som finns under Default inne i appsettings.json alltså:
  
"Default": "Server=localhost;Port=3306;User ID=USERNAME;Password=DatabasePassword;Database=holiday_maker"


```csharp
string connectionString =
    configuration.GetConnectionString("Default")!;
```

---

## Databas koppling

```csharp
using var connection = new MySqlConnection(connectionString);

connection.Open();
```

MySqlConnection: Här skapas ett nytt objekt som representerar anslutningen till MySql-databsen.

connectionString: Denna säger till programmet vilken server, användarnamn, lösenord och databas som ska användas.

connection.Open(): Öppnar anslutningen till MySql

---

## Sql query

```csharp
string sql = "SELECT * FROM Boende";
```

Querien är sparad i variabeln sql

"SELECT * FROM Boende" betyder att man vill hämta alla kolumner och alla rows från boende tabellen. 

---

## MySqlCommand

```csharp
using var command = new MySqlCommand(sql, connection);
```

MySqlCommand är en klass som används för att skapa ett SQL kommando.

De två parametrarna i MySqlCommand:

- 'sql' Vilket kommando som ska köras
- 'connection' Vart för någonstans det ska köras.

---

## ExecuteReader

```csharp
using var reader = command.ExecuteReader();
```

'ExecuteReader' kör kommandot (sql) i den specifierade databasen alltså (connection)

---

## While Loop

```csharp
while (reader.Read())
{
    Console.WriteLine(
        $"{reader["id"]}: {reader["namn"]} - {reader["ort"]}"
    );
}
```

Här använder jag en while loop eftersom man inte vet hur många rader som kommer returneras.

'reader.Read' går till nästa rad.

Så länge det finns en till rad så returneras TRUE, vilket får programmet att försätta köra koden i loopen.

När det inte finns några mer rader så returneras false, vilket stoppar loopen.

Loopen läser och skriver ut en rad i taget.

## getRooms query

```csharp
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
```
Här skapade jag en sql query som hämtar informationen om rum och vilket boende det rummet tillhör. Använder JOIN för att koppla ihop rum tabellen  genom att använda boende_id och id.

```SQL
WHERE Boende.ort = @ort
```
Här används WHERE för att filtrera resultatet, jag vill bara få fram rum utifrån den ort som använder skriver in. @ort är en parameter som är kopplad till C# variabeln ort. Värdet i ort används sen av sql queryn så att man kan filtrera vilka rum som visas baserat på vilken ort det är.

## Parametrar och använderinput i programmet

```csharp
Console.Write("Ange ort: ");
string ort = Console.ReadLine()!;
```

Här frågar programmet vilken ort som den vill söka efter. Svaret som användaren skriver sparas i ort

Sedan kopplas värdet från ort in till parametern @ort:

```csharp
roomCommand.Parameters.AddWithValue("@ort", ort);
```

## Metoder

```csharp
void GetRoomsByTown(MySqlConnection connection)
{
    // koden
}
```
Parametern "MySqlConnection connection" gör så att metoden får tillgång till databasen som skapades innan i programmet.

```csharp
void GetRoomsByTown(MySqlConnection connection)
{
    // koden
}
```

Samma sak här med parametern som föra metoden,

## Felhantering för om något hittades eller inte
```csharp
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
```
Ganska self-explanitory.

## RoomService filen

Jag skapade en klass RoomService och flyttade både "GetRoomsByTown()" & "CheckAmountOfPeople()" metoderna in till RoomService Från Program.cs för att få mer struktur i programmet, så jag lägger all kod som handlar om rum i RoomService.


## Booking service filen

Jag skapade en klass som heter BookingService för mera struktur den klass samlar all kod som handlar om bokningar. Mycket här är liknande kod exempel så går inte igenom koden här.


