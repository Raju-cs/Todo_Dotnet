using API.Dtos;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TodoController : BaseController
    {
        
       private readonly IUnitOfWork _unitOfWork;
        private readonly ITodoRepository _repo;

        public TodoController(IUnitOfWork unitOfWork, ITodoRepository repo)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }
         [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoDto todoDto)
        {
            var status =await _unitOfWork.Repository<Status>().GetByIdAsync(todoDto.StatusId);

             if(status is null) return NotFound("Status not found");

              Todo todo = new ()
              {
                  Title = todoDto.Title,
                  Description = todoDto.Description,
                  Status = status
              };

              _unitOfWork.Repository<Todo>().Add(todo);

              if (await _unitOfWork.Complete() <= 0)
                return BadRequest("Failed to save");

            return Ok(todo);
        }

        [HttpGet] 
        public async Task<IActionResult> GetTodos()
        {
            var todos = await _repo.GetTodosAsync();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            var todo = await _repo.GetTodoAsync(id);
            if(todo is null) return NotFound("Todo not found");

            return Ok(todo);

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTodos([FromRoute] int id, [FromBody] TodoDto todoDto )
        {
            var status =await _unitOfWork.Repository<Status>().GetByIdAsync(todoDto.StatusId);
             if(status is null) return NotFound("status not found");

            var todo =await _unitOfWork.Repository<Todo>().GetByIdAsync(id);
             if(todo is null) return NotFound("Todo not found");

             

             todo.Status = status;
             todo.Description = todoDto.Description;
             todo.Title = todoDto.Title;

               if (await _unitOfWork.Complete() <= 0) return BadRequest("Failed to save");

            return Ok(todo);
            
            
        }

          [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            var todo = await _unitOfWork.Repository<Todo>().GetByIdAsync(id);

            if (todo is null) return NotFound("todo not found");

            _unitOfWork.Repository<Todo>().Delete(todo);

            if (await _unitOfWork.Complete() <= 0) return BadRequest("Failed to save");

            return NoContent();
        }
       
    }
}