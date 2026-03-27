namespace DataVerseManagerV2
{
    using DataVerseManagerV2.Models;
    using DataVerseManagerV2.Services;
    using Spectre.Console;
    class Program
    {
        // Generisk DataStore för att lagra alla Workout-objekt
        static DataStore<Workout> store = new DataStore<Workout>();

        // Filnamn för JSON-lagring
        static string filePath = "data.json";
        static void Main()
        {
            // Försök läsa in sparad data från JSON
            try
            {
                    store.LoadFromJson(filePath);
                }
                catch
                {
                    AnsiConsole.MarkupLine("[red]Kunde inte ladda data.[/]");
                }

                ShowWelcome();

            // Huvudloop för programmenyn
            while (true)
            {
                var choice = ShowMenu();

                switch (choice)
                {
                    case "Skapa pass":
                        CreateWorkout();
                        break;

                    case "Visa pass":
                        ShowWorkouts();
                        break;

                    case "Ta bort pass":
                        DeleteWorkout();
                        break;

                    case "Avsluta":
                        store.SaveToJson(filePath);
                        return;
                }
            }
        }
        // Skapa ett nytt träningspass
        static void CreateWorkout()
        {
            try
            {
                // Fråga användaren om namn
                var name = AnsiConsole.Ask<string>("Namn på pass:");
               
                // Skapa nytt Workout-objekt
                var workout = new Workout
                {
                    Name = name,
                    Date = DateTime.Now
                };
                
                // Lägg till passet i DataStore
                store.AddItem(workout);
                store.SaveToJson(filePath);


                // Bekräftelse till användaren
                AnsiConsole.MarkupLine("[green]Pass sparat![/]");
            }
            catch
            {
                // Felmeddelande om något går fel
                AnsiConsole.MarkupLine("[red]Något gick fel![/]");
            }
        }

        static void ShowWelcome()   // Visa välkomstbanner med FigletText och färg
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("DataVerseV2")
                    .Centered()
                    .Color(Color.Green));

            AnsiConsole.MarkupLine("[yellow]Fitness Progress Tracker[/]\n");
        }

        static string ShowMenu() // Visa huvudmenyn och returnera användarens val
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Vad vill du göra?[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                    "Skapa pass",
                    "Visa pass",
                    "Ta bort pass",
                    "Avsluta"
                    }));
        }

        static void ShowWorkouts() // Visa alla pass i en tabell
        {
            var workouts = store.GetAll();

            if (workouts.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Inga pass hittades.[/]");
                return;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[yellow]Namn[/]");
            table.AddColumn("[blue]Datum[/]");

            foreach (var w in workouts)
            {
                table.AddRow(w.Name, w.Date.ToShortDateString());
            }

            AnsiConsole.Write(table);
        }
        static void DeleteWorkout()
        {
            var workouts = store.GetAll();

            if (workouts.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Inga pass att ta bort.[/]");
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Workout>()
                    .Title("Välj pass att ta bort")
                    .UseConverter(w => $"{w.Name} ({w.Date.ToShortDateString()})")
                    .AddChoices(workouts));

            store.RemoveItem(selected);
            store.SaveToJson(filePath);

            AnsiConsole.MarkupLine("[green]Pass borttaget![/]");
        }
    }
}








