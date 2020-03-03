using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class ToDoItemEngine : BaseApiEngine, ITodoItemHandler
    {
        private const string controllerName = "todoitems";
        public ToDoItemEngine(HttpWorker httpWorker) : base(httpWorker, controllerName)
        {
        }

        public Task<ActionResponse> AddToDoItem(ToDoItem toDoItem)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, toDoItem);
        }

        public Task<ActionResponse> DeleteToDoItem(ToDoItem toDoItem)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, toDoItem);
        }

        public Task<List<ToDoItem>> GetToDoItems(int page)
        {
            return SendRequest<List<ToDoItem>>(HttpMethod.Get, new { page });
        }

        public Task<ActionResponse> UpdateToDoItem(ToDoItem toDoItem)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, toDoItem);
        }
    }
}
