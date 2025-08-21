using System.Collections.Generic;
using TodoWithFiles.Models;

namespace TodoWithFiles.Services
{
    public interface IToDoService
    {
        void AddNote(ToDo newNote);
        void ModifyNote(ToDo updatedNote);
        void DeleteNote(int Id);
        List<ToDo> GetNotes();
        void SetPath(string filePath);
    }
}