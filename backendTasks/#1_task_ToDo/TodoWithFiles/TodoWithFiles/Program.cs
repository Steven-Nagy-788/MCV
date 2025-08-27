using TodoWithFiles.Services;
using TodoWithFiles.Models;

string baseDir = "D:\\projects\\trialCore\\TodoWithFiles\\TodoWithFiles\\profiles";
Console.WriteLine("Available note files in current directory:");
string[] files = Directory.GetFiles(baseDir, "*.json");

IToDoService toDo = new ToDoService();
string filePath;

if (files.Length>0)
{
    for (int i = 0; i < files.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
    }
}
else
    Console.WriteLine("No files found.");
while (true)
{
    Console.WriteLine("\n1 to choose from these files\n2 to add a new file");
    string? choice = Console.ReadLine();
    if (choice == "1")
    {
        Console.Write("Enter file number: ");
        string? fileIndexInput = Console.ReadLine();
        if (int.TryParse(fileIndexInput, out int fileIndex) &&
            fileIndex >= 1 && fileIndex <= files.Length)
        {
            filePath = files[fileIndex - 1];
            toDo.SetPath(filePath);
            break;
        }
        else
            Console.WriteLine("Invalid file number.");
    }
    else if (choice == "2")
    {
        while (true)
        {
            Console.Write("Enter new file name (without extension): ");
            string? newFileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newFileName))
            {
                Console.WriteLine("File name cannot be empty. Please try again.");
                continue;
            }
            filePath = Path.Combine(baseDir, newFileName + ".json");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
                toDo.SetPath(filePath);
                break;
            }
            else
                Console.WriteLine("A file with this name already exists. Please choose a different name.");
        }
        break;
    }
    else
        Console.WriteLine("Invalid Option");
}
while (true)
{
    Console.Write("\n1 to add new note\n2 to modify existing note\n3 to delete note\n4 to show notes\n5 to exit\nEnter here: ");
    string? wanted = Console.ReadLine();
    if (wanted == "1")
    {
        Console.Write("Enter note Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID must be an integer.");
            continue;
        }

        Console.Write("Enter note content: ");
        string? noteContent = Console.ReadLine();
        if (string.IsNullOrEmpty(noteContent))
        {
            Console.WriteLine("Note content cannot be empty.");
            continue;
        }

        var newTodo = new ToDo { Id = id, Note = noteContent };
        toDo.AddNote(newTodo);
    }
    else if (wanted == "2")
    {
        Console.Write("Enter note Id to modify: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID must be an integer.");
            continue;
        }

        Console.Write("Enter new content: ");
        string? noteContent = Console.ReadLine();
        if (string.IsNullOrEmpty(noteContent))
        {
            Console.WriteLine("Note content cannot be empty.");
            continue;
        }

        var updatedTodo = new ToDo { Id = id, Note = noteContent };
        toDo.ModifyNote(updatedTodo);
    }
    else if (wanted == "3")
    {
        Console.Write("Enter note Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID must be an integer.");
            continue;
        }

        toDo.DeleteNote(id);
    }
    else if (wanted == "4")
    {
        var notes = toDo.GetNotes();
        if (notes.Count>0)
        {
            Console.WriteLine("\n--- Your Notes ---");
            foreach (var note in notes)
            {
                Console.WriteLine($"[{note.Id}] {note.Note}");
            }
            Console.WriteLine("------------------");
        }
        else
            Console.WriteLine("No notes found.");
    }
    else if (wanted == "5")
        break;
    else
        Console.WriteLine("Invalid Option");
}