using System.Text.Json;
using TodoWithFiles.Models;

namespace TodoWithFiles.Services
{
    public class ToDoService : IToDoService
    {
        private string? filePath;
        private List<ToDo> notes = new();

        public void SetPath(string filePath)
        {
            this.filePath = filePath;
            LoadNotes(filePath);
        }

        public void LoadNotes(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                    notes = JsonSerializer.Deserialize<List<ToDo>>(json)?? new List<ToDo>();
            }
            notes = notes is not null ? notes : new List<ToDo>();
        }

        private void SaveNotes()
        {
            string json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void AddNote(ToDo newNote)
        {
            if (notes.Any(n => n.Id == newNote.Id))
            {
                Console.WriteLine("A note with this Id already exists.");
                return;
            }

            notes.Add(newNote);
            SaveNotes();
            Console.WriteLine("Note added successfully!");
        }

        public void ModifyNote(ToDo updatedNote)
        {
            var existing = notes.FirstOrDefault(n => n.Id == updatedNote.Id);
            if (existing != null)
            {
                existing.Note = updatedNote.Note;
                SaveNotes();
                Console.WriteLine("Note updated successfully!");
            }
            else
            {
                Console.WriteLine("Note not found.");
            }
        }

        public void DeleteNote(int Id)
        {
            var existing = notes.FirstOrDefault(n => n.Id == Id);
            if (existing != null)
            {
                notes.Remove(existing);
                SaveNotes();
                Console.WriteLine("Note deleted successfully!");
            }
            else
            {
                Console.WriteLine("Note not found.");
            }
        }

        public List<ToDo> GetNotes()
        {
            return notes;
        }
    }
}