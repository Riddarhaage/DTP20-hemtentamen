using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace dtp15_todolist
{
    public class Todo
    {
        public static List<TodoItem> list = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;
        public static string StatusToString(int status)
        {
            switch (status)
            {
                case Active: return "aktiv";
                case Waiting: return "väntande";
                case Ready: return "avklarad";
                default: return "(felaktig)";
            }
        }
        public class TodoItem
        {
            public int status;
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task)
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = "";
            }
            public TodoItem()
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = "";
            }
            public TodoItem(string todoLine)
            {
                string[] field = todoLine.Split('|');
                status = Int32.Parse(field[0]);
                priority = Int32.Parse(field[1]);
                task = field[2];
                taskDescription = field[3];
            }
            public void Print(bool verbose = false)
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-20}|");
                if (verbose)
                    Console.WriteLine($"{taskDescription,-40}|");
                else
                    Console.WriteLine();
            }
        }
        public static void ReadListFromFile()
        {
            string todoFileName = "todo.lis";
            Console.Write($"Läser från fil {todoFileName} ... ");
            StreamReader sr = new StreamReader(todoFileName);
            int numRead = 0;

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                TodoItem item = new TodoItem(line);
                list.Add(item);
                numRead++;
            }
            sr.Close();
            Console.WriteLine($"Läste {numRead} rader.");
        }
        private static void PrintHeadOrFoot(bool head, bool verbose)
        {
            if (head)
            {
                Console.Write("|status      |prio  |namn                |");
                if (verbose) Console.WriteLine("beskrivning                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|--------------------|");
            if (verbose) Console.WriteLine("----------------------------------------|");
            else Console.WriteLine();
        }
        private static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);
        }
        public static void PrintTodoList(bool verbose = false)
        {
            if (Todo.list.Count == 0)
            {
                Console.WriteLine("Listan är tom!");
            }
            else
                PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                item.Print(verbose);
            }
            if (Todo.list.Count != 0)
                PrintFoot(verbose);
        }
        public static void PrintTodoListActive(bool verbose = false)
        {
            if (Todo.list.Count == 0)
            {
                Console.WriteLine("Listan är tom!");
            }
            else
                PrintHead(verbose);
            foreach (TodoItem item in list)
                if (item.status == Active)
                {
                    item.Print(verbose);
                }
            if (Todo.list.Count != 0)
                PrintFoot(verbose);
        }
        public static void PrintTodoListWaiting(bool verbose = false)
        {
            if (Todo.list.Count == 0)
            {
                Console.WriteLine("Listan är tom!");
            }
            else
                PrintHead(verbose);
            foreach (TodoItem item in list)
                if (item.status == Waiting)
                {
                    item.Print(verbose);
                }
            if (Todo.list.Count != 0)
                PrintFoot(verbose);
        }
        public static void PrintHelp()
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("hjälp                 lista denna hjälp");
            Console.WriteLine("ladda                 ladda todo.lis");
            Console.WriteLine("lista                 lista alla uppfigter med status 'aktiv' i att-göra-listan");
            Console.WriteLine("lista väntande        lista alla uppfigter med status 'väntande' i att-göra-listan");
            Console.WriteLine("lista allt            lista allt i att-göra-listan");
            Console.WriteLine("ny                    lägg till ny uppgift i listan");
            Console.WriteLine("spara                 Spara alla ändringar i listan");
            Console.WriteLine("beskriv               samma som 'lista' men uppgifts-beskrivning skrivs också ut");
            Console.WriteLine("beskriv allt          samma som 'lista allt' men uppgifts-beskrivning skrivs också ut");
            Console.WriteLine("aktivera /uppgift/    sätter status till 'aktiv' på den uppgift man valt");
            Console.WriteLine("klar /uppgift/        sätter status till 'avklarad' på den uppgift man valt");
            Console.WriteLine("vänta /uppgift/       sätter status till 'väntande' på den uppgift man valt");
            Console.WriteLine("sluta                 spara att-göra-listan och sluta");

        }
        public static void AddNewItem(TodoItem item)
        {
            Console.WriteLine("Ange namn på uppgift: ");
            item.task = Console.ReadLine();
            Console.WriteLine("ange beskrivning av uppgift: ");
            item.taskDescription = Console.ReadLine();
            Console.WriteLine("Ange uppgiftens prioritet från 1 till 4: "); //ändra så det blir tydligare vilka olika prioriteter som finns. TBD
            item.priority = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Ange uppgiftens status: "); //Visa vilka statusar som finns tillgängliga innan det skrivs in.
            item.status = Int32.Parse(Console.ReadLine());
            list.Add(item);
            Console.WriteLine("Uppgift tillagd!");
        }
        public static void SaveList()
        {
            using StreamWriter outfile = new StreamWriter("todo.lis");
            foreach(TodoItem item in list)
            {
                if(item != null)
                   outfile.WriteLine($"{item.priority}|{item.status}|{item.task}|{item.taskDescription}");
            }
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till att-göra-listan!");
            Todo.PrintHelp();
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");
                if (MyIO.Equals(command, "hjälp"))
                {
                    Todo.PrintHelp();
                }
                else if (MyIO.Equals(command, "ladda"))
                {
                    Todo.ReadListFromFile();
                }
                else if (MyIO.Equals(command, "sluta"))
                {
                    Console.WriteLine("Hej då!");
                    break;
                }
                else if (MyIO.Equals(command, "lista"))
                {
                    if (MyIO.HasArgument(command, "allt"))
                        Todo.PrintTodoList();
                    else if (MyIO.HasArgument(command, "väntande"))
                    {
                        Todo.PrintTodoListWaiting();
                    }
                    else
                    {
                        Todo.PrintTodoListActive();
                    }
                }
                else if (MyIO.Equals(command, "beskriv"))
                {
                    Todo.PrintTodoListActive(true);
                    if (MyIO.HasArgument(command, "allt"))
                    {
                        Todo.PrintTodoList(true);
                    }
                        
                }
                else if (MyIO.Equals(command, "ny"))
                {
                    Todo.TodoItem item = new Todo.TodoItem(1, "");
                    Todo.AddNewItem(item);
                }
                else if (MyIO.Equals(command, "spara"))
                {
                    Todo.SaveList();
                }
                else if (MyIO.Equals(command, "aktivera"))
                {
                    MyIO.HasArgument(command, command);
                    setStatusActive(command);
                }
                else if (MyIO.Equals(command, "klar"))
                {
                    MyIO.HasArgument(command, command);
                    setStatusReady(command);
                }
                else if (MyIO.Equals(command, "vänta"))
                {
                    MyIO.HasArgument(command, command);
                    setStatusWaiting(command);
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: {command}");
                }
            }
            while (true);
        }

        private static void setStatusActive(string command)
        {
            string[] words = command.Split(' ');
            foreach (Todo.TodoItem item in Todo.list)
            {
                if (words.Length <= 2)
                {
                    if (item.task == words[1])
                    {
                        item.status = Todo.Active;
                    }
                }
                if (words.Length > 2)
                    if (item.task == words[1] + " " + words[2])
                    {
                        item.status = Todo.Active;
                    }
            }
            Console.WriteLine("status satt till 'aktiv'");
        }
        private static void setStatusReady(string command)
        {
            string[] words = command.Split(' ');
            foreach (Todo.TodoItem item in Todo.list)
            {
                if (words.Length <= 2)
                {
                    if (item.task == words[1])
                    {
                        item.status = Todo.Ready;
                    }
                }
                if (words.Length > 2)
                    if (item.task == words[1] + " " + words[2])
                    {
                        item.status = Todo.Ready;
                    }
            }
            Console.WriteLine("status satt till 'avklarad'");
        }
        private static void setStatusWaiting(string command)
        {
            string[] words = command.Split(' ');
            foreach (Todo.TodoItem item in Todo.list)
            {
                if (words.Length <= 2)
                {
                    if (item.task == words[1])
                    {
                        item.status = Todo.Waiting;
                    }
                }
                if (words.Length > 2)
                    if (item.task == words[1] + " " + words[2])
                    {
                        item.status = Todo.Waiting;
                    }
            }
            Console.WriteLine("status satt till 'väntande'");
        }
    }
    class MyIO
    {
        static public string ReadCommand(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        static public bool Equals(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords[0] == expected) return true;
            }
            return false;
        }
        static public bool HasArgument(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords.Length < 2) return false;
                if (cwords[1] == expected) return true;
            }
            return false;
        }
    }
}
