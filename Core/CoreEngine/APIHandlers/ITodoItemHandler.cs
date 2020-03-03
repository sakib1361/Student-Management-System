using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ITodoItemHandler
    {
        Task<List<ToDoItem>> GetToDoItems(int page);
        Task<ActionResponse> AddToDoItem(ToDoItem toDoItem);
        Task<ActionResponse> UpdateToDoItem(ToDoItem toDoItem);
        Task<ActionResponse> DeleteToDoItem(ToDoItem toDoItem);
    }
}