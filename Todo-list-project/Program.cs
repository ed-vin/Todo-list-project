using System.Text.Json;

class Program //huvudprogrammet 
{
    // Lista för att lagra alla uppgifter
    static List<Task> tasks = new List<Task>();
    // Sökvägen till JSON-filen där uppgifterna sparas (toDoList/bin/debug/ToDoList.json)
    static string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ToDoList.json");

    static void Main(string[] args) // huvudmetoden för programmet
    {
        // Ladda uppgifter från JSON-fil om den finns
        LoadTasks();

        bool running = true;
        while (running) // loopar tills användaren väljer ett alternativ
        {
            Console.Clear();
            // Visa huvudmenyn
            DisplayMenu();
            string choice = Console.ReadLine(); // läs användarens val

            // Beroende på användarens val, utför olika åtgärder genom en switch-sats
            switch (choice)
            {
                case "1": // Visa alla uppgifter i listan
                    ViewTasks();
                    break;
                case "2": // Lägg till en ny uppgift i listan
                    AddTask();
                    break;
                case "3": // Redigera en befintlig uppgift i listan
                    EditTask();
                    break;
                case "4": // Spara uppgifter och avsluta programmet
                    SaveTasks();
                    running = false;
                    break;
                default:
                    // Visa felmeddelande om användaren gör ett ogiltigt val
                    DisplayError("Ogiltigt val, vänligen försök igen.");
                    break;
            }
        }
    }

    // Visar huvudmenyn
    static void DisplayMenu() // static void för att kunna anropas utan att skapa en instans av klassen
    {
        int completedTasks = tasks.Count(t => t.IsDone); // räkna antalet avklarade uppgifter
        int totalTasks = tasks.Count; // räkna det totala antalet uppgifter

        Console.WriteLine("Välkommen till att-göra-listan");
        Console.WriteLine($"Du har {totalTasks} saker på listan varav {completedTasks} saker är avklarade.");
        Console.WriteLine("Välj ett alternativ genom att skriva numret:");
        Console.WriteLine("1. Visa listan för uppgifter (sortera efter datum eller projekt).");
        Console.WriteLine("2. Lägg till uppgift.");
        Console.WriteLine("3. Ändra uppgift (uppdatera, markera som klar, ta bort uppgift).");
        Console.WriteLine("4. Spara och avsluta.");
        Console.Write("Ange ditt val (1-4): ");
    }

    // Visar alla uppgifter i listan som är sparade i JSON-filen
    static void ViewTasks() // static void för att kunna anropas utan att skapa en instans av klassen
    {
        if (tasks.Count == 0) // om det inte finns några uppgifter sparade i listan
        {
            // Om det inte finns några uppgifter, visa ett felmeddelande
            DisplayError("Det finns inga uppgifter.");
        }
        else // sortera listan
        {
            Console.WriteLine("Välj hur du vill sortera listan:");
            Console.WriteLine("1. Sortera efter förfallodatum.");
            Console.WriteLine("2. Sortera efter projekt.");

            string sortChoice = Console.ReadLine();
            switch (sortChoice)
            {
                case "1": // sortera efter förfallodatum
                    tasks = tasks.OrderBy(t => t.DueDate).ToList();
                    break;
                case "2": // sortera efter projekt
                    tasks = tasks.OrderBy(t => t.Project).ToList();
                    break;
                default:
                    DisplayError("Ogiltigt val.");
                    break;
            }

            // Skriv ut rubriker för kolumnerna utan nummer
            Console.WriteLine($"{PadRight("Titel", 30)}{PadRight("Förfallodatum", 15)}{PadRight("Projekt", 20)}{PadRight("Status", 12)}");
            Console.WriteLine(new string('-', 80));  // En linje för att separera rubriker från data

            // Loopar genom alla uppgifter och skriver ut detaljer utan nummer
            foreach (var task in tasks)
            {
                Console.WriteLine($"{PadRight(task.Title, 30)}{PadRight(task.DueDate.ToString("MM-dd"), 15)}{PadRight(task.Project, 20)}{PadRight(task.IsDone ? "Avklarad" : "Ej avklarad", 12)}");
            }
        }

        Console.WriteLine("\nTryck på backspace för att återgå till huvudmenyn.");
        while (Console.ReadKey(true).Key != ConsoleKey.Backspace) { } // vänta på att användaren trycker på backspace för att återgå till huvudmenyn
    }

    // En hjälpmetod för att fylla ut strängar så att de får en viss längd, används för att justera kolumnbredden
    static string PadRight(string input, int length)
    {
        return input.Length > length ? input.Substring(0, length) : input.PadRight(length);
    }

    // Lägg till en ny uppgift genom menyn (alternativ 2)
    static void AddTask() //lägg till uppgift i listan
    {
        Console.Write("Ange uppgiftens titel: ");
        string title = Console.ReadLine();

        Console.Write("Ange förfallodatum (MM-dd): ");
        DateTime dueDate;
        while (!TryParseMonthDay(Console.ReadLine(), out dueDate))
        {
            // Om användaren skriver fel datumformat, be om nytt datum
            DisplayError("Ogiltigt datumformat. Försök igen (MM-dd): ");
        }

        Console.Write("Ange projektets namn: ");
        string project = Console.ReadLine();

        // Skapa en ny uppgift och lägg till den i listan
        Task task = new Task(title, dueDate, project);
        tasks.Add(task);

        // Visa en bekräftelse för användaren
        DisplaySuccess("Uppgift tillagd!");
        Console.WriteLine("Tryck på backspace för att återgå till huvudmenyn.");
        while (Console.ReadKey(true).Key != ConsoleKey.Backspace) { }
    }

    // Försök att parsa datumet från formatet MM-dd och använd det aktuella året
    static bool TryParseMonthDay(string input, out DateTime date)
    {
        date = DateTime.MinValue;

        // Försök att parsa MM-dd med aktuellt år
        if (DateTime.TryParseExact(input, "MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            // Byt till det aktuella året
            date = new DateTime(DateTime.Now.Year, date.Month, date.Day);
            return true;
        }

        return false;
    }

    // Redigera en befintlig uppgift i menyn (alternativ 3)
    static void EditTask()
    {
        Console.Clear();
        Console.WriteLine("Välj vilken uppgift du vill ändra:");
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            Console.WriteLine($"{PadRight((i + 1).ToString(), 6)}{PadRight(task.Title, 30)}{PadRight(task.DueDate.ToString("MM-dd"), 15)}{PadRight(task.Project, 20)}{PadRight(task.IsDone ? "Avklarad" : "Ej avklarad", 12)}");
        }

        Console.Write("Ange uppgiftens nummer att ändra: ");
        int taskNumber;
        if (int.TryParse(Console.ReadLine(), out taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            Task task = tasks[taskNumber - 1];

            Console.Clear();
            Console.WriteLine($"Redigerar uppgift: {task.Title}");
            Console.WriteLine("Vad vill du göra?");
            Console.WriteLine("1. Uppdatera titel.");
            Console.WriteLine("2. Markera som klar.");
            Console.WriteLine("3. Ta bort uppgift.");
            Console.Write("Välj ett alternativ (1-3): ");
            string editChoice = Console.ReadLine();

            bool madeChanges = false; // flagga för att spåra om några ändringar gjordes

            // Beroende på vad användaren väljer, utför olika åtgärder med en switch-sats
            switch (editChoice)
            {
                case "1": // Uppdatera titel
                    Console.Write("Ange ny titel (eller lämna blankt för att behålla den nuvarande): ");
                    string newTitle = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newTitle)) 
                    {
                        task.Title = newTitle;
                        madeChanges = true;
                    }
                    break;
                case "2": // Markera som klar
                    task.IsDone = true;
                    DisplaySuccess("Uppgiften markerad som klar!");
                    madeChanges = true;
                    break;
                case "3": // Ta bort uppgift
                    tasks.RemoveAt(taskNumber - 1);
                    DisplaySuccess("Uppgiften borttagen!");
                    madeChanges = true;
                    break;
                default: // Ogiltigt val. Visa felmeddelande
                    DisplayError("Ogiltigt val.");
                    break;
            }

            // Om några ändringar gjordes, visa bekräftelse
            if (madeChanges)
            {
                Console.WriteLine("\nÄndringarna har sparats. Tryck på backspace för att återgå till huvudmenyn.");
                while (Console.ReadKey(true).Key != ConsoleKey.Backspace) { }
            }
            else
            {
                // Om inga ändringar gjordes, informera användaren
                Console.WriteLine("Inga ändringar gjordes. Tryck på backspace för att återgå till huvudmenyn.");
                while (Console.ReadKey(true).Key != ConsoleKey.Backspace) { }
            }
        }
        else
        {
            // Om användaren angav ett ogiltigt uppgiftsnummer
            DisplayError("Ogiltigt uppgiftsnummer.");
            Console.WriteLine("Tryck på backspace för att återgå till huvudmenyn.");
            while (Console.ReadKey(true).Key != ConsoleKey.Backspace) { } 
        }
    }

    // Spara uppgifter till JSON-fil (ToDoList.json i mappen där programmet körs, om den inte finns, skapa den)
    static void SaveTasks()
    {
        try
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            DisplaySuccess("Uppgifter sparade till ToDoList.json.");
        }
        catch (Exception ex)
        {
            // Visa ett felmeddelande om det sker ett undantag vid sparande
            DisplayError($"Ett fel inträffade vid sparande av uppgifter: {ex.Message}");
        }
    }

    // Ladda uppgifter från JSON-fil (ToDoList.json i mappen där programmet körs)
    static void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
            }
            catch (Exception ex)
            {
                // Visa ett felmeddelande om det sker ett undantag vid inläsning
                DisplayError($"Ett fel inträffade vid inläsning av uppgifter: {ex.Message}");
            }
        }
    }

    // Funktion för att visa felmeddelanden i röd färg
    static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Funktion för att visa framgångsmeddelanden i grön färg
    static void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

// Klass för att representera en uppgift i att-göra-listan
class Task
{
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public string Project { get; set; }
    public bool IsDone { get; set; }

    public Task(string title, DateTime dueDate, string project, bool isDone = false) // konstruktor för att skapa en ny uppgift
    {
        Title = title;
        DueDate = dueDate;
        Project = project;
        IsDone = isDone;
    }
}
