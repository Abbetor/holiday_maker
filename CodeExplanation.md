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