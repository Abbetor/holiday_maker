using System.Globalization;
//Hjälpklass så jag slipper använda TryParse överallt.
static class InputHelper
{
    public static int ReadInt(string prompt, int min = int.MinValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == null)
                Environment.Exit(0);

            if (int.TryParse(input, out int value) && value >= min)
                return value;

            Console.WriteLine(min == int.MinValue
                ? "Ange ett heltal."
                : $"Ange ett heltal som är minst {min}.");
        }
    }

    public static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == null)
                Environment.Exit(0);

            if (DateTime.TryParseExact(input.Trim(), "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime date))
                return date;

            Console.WriteLine("Fel format, Skriv datumet som yyyy-MM-dd");
        }
    }


        public static string ReadText(string prompt, int maxLength)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == null)
                Environment.Exit(0);

            input = input.Trim();

            if (input.Length > 0 && input.Length <= maxLength)
                return input;

            Console.WriteLine($"Får inte vara tomt och max {maxLength} tecken.");
        }
    }

}
