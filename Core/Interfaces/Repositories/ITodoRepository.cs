using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface ITodoRepository
    {
        Task<IReadOnlyList<Todo>> GetTodosAsync();
        Task<Todo?> GetTodoAsync(int id);

    }
}