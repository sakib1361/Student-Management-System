using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrasructure.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class TodoItemsController : Controller, ITodoItemHandler
    {
        private readonly TodoItemService _todoService;

        public TodoItemsController(TodoItemService todoItemService)
        {
            _todoService = todoItemService;
        }

        public Task<ActionResponse> AddToDoItem(ToDoItem toDoItem)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _todoService.Add(userId, toDoItem);
        }

        public Task<ActionResponse> DeleteToDoItem(ToDoItem toDoItem)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _todoService.Delete(userId, toDoItem);
        }

        public Task<List<ToDoItem>> GetToDoItems(int page)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _todoService.GetItem(userId, page);
        }

        public Task<ActionResponse> UpdateToDoItem(ToDoItem toDoItem)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _todoService.Update(userId, toDoItem);
        }
    }
}
