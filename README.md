# Holiday Maker

Konsolapplikation för en resebyråoperatör som bokar semesterboenden.
Databas: MySQL. Applikation: C# (.NET 10) med MySqlConnector.

## Starta programmet

1. Kör SQL-skriptet `holiday_maker.sql` för att skapa tabeller och testdata.
2. Lägg anslutningssträngen i `appsettings.json`.
3. Kör `dotnet run`.

## Testning

Vissa tester är gjorda manuellt i konsolen och vissa har körts automatiskt med hjälp av en AI-assistent (Claude Code).

Tillgängligheten avgörs av villkoret
`Bokning.incheckning < @checkOut AND Bokning.utcheckning > @checkIn`.

| # | Vad testas | Steg | Förväntat resultat | Resultat |
|---|---|---|---|---|
| 1 | Ledigt rum visas, upptaget filtreras bort | Val 1: Ystad, 1 person, JA, JA, 99999, 99999, 2026-12-11 till 2026-12-12 | Hotel Ocean rum 1 (bokat 10–13 dec) visas inte. Beach Resort rum 1 visas. | ✅ |
| 2 | Bokning samma dag som en tidigare kund checkar ut | Val 4: rum id 1, 2026-12-13 till 2026-12-14 | Bokningen godkänns, eftersom utcheckningsdagen inte upptar rummet | ✅ |
| 3a | Överlappande bokning nekas | Val 4: rum id 1, 2026-12-11 till 2026-12-12 | "Rummet är inte ledigt under dessa datum." | ✅ |
| 3b | Omslutande bokning nekas | Val 4: rum id 1, 2026-12-01 till 2026-12-31 | "Rummet är inte ledigt under dessa datum." | ✅ |
| 4 | För stort sällskap nekas | Val 4: 5 personer, rum id 1 (max 1) | "Rummet har plats för max 1 personer." | ✅ |
| 5a | Kombinerade filter | Val 1: pool JA, restaurang JA, max 300 m till strand | Bara boenden som uppfyller alla villkor visas | ✅ |
| 5b | Sortering på pris | Val 1, sortera "pris" | Lägst pris först | ✅ |
| 5c | Sortering på omdöme | Val 1, sortera "omdöme" | Högst omdöme först | ✅ |
| 6a | Ändra antal personer inom kapaciteten | Val 6: bokning i rum med max 4, ändra till 3 | "Antalet personer har ändrats." | ✅ |
| 6b | Ändra antal personer över kapaciteten | Val 6: samma bokning, ändra till 5 | "Rummet har plats för max 4 personer." | ✅ |
| 7 | Avbokning frigör rummet | Boka ett rum, avboka med val 7, boka samma rum och datum igen | Den andra bokningen godkänns | ✅ |
| 8 | Uppgifterna finns kvar efter omstart | Boka, avsluta med val 9, starta igen, visa med val 5 | Bokningen finns kvar | ✅ |

### Felaktig inmatning

| Inmatning | Resultat |
|---|---|
| Text i stället för tal (`abc`) | Felmeddelande, frågar igen |
| 0 eller negativt antal personer | Felmeddelande, frågar igen |
| Fel datumformat (`2026-13-01`) | Felmeddelande, frågar igen |
| Utcheckning före eller samma dag som incheckning | Bokningen stoppas med meddelande |
| Kund- eller rum-id som inte finns | "Kunden finns inte." / "Rummet finns inte." |


Strukturen i README:n togs fram med hjälp av en AI-assistent.