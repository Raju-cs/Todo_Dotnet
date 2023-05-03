using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{


    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        public TodoRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<IReadOnlyList<Todo>> GetTodosAsync()
        {
            return await _context.Todos.OrderBy(t => t.Id)
                                       .Include(t => t.Status)
                                       .ToListAsync();

        }

          public async Task<Todo?> GetTodoAsync(int id)
        {
            return await _context.Todos.Include(t => t.Status).SingleOrDefaultAsync(t => t.Id == id);

        }
    }
}