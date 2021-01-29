using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Task = TodoApi.Models.Task;

namespace TodoApi.Controllers
{
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            return await _context.Tasks
                .Select(x => TaskToDto(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(long id)
        {
            var todoItem = await _context.Tasks.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return TaskToDto(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(long id, TaskDto taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Name = taskDto.Name;
            task.Date = taskDto.Date;
            task.IsComplete = taskDto.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTodoItem(TaskDto taskDto)
        {
            var task = new Task
            {
                IsComplete = taskDto.IsComplete,
                Date = taskDto.Date,
                Name = taskDto.Name
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                TaskToDto(task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(long id) =>
             _context.Tasks.Any(e => e.Id == id);

        private static TaskDto TaskToDto(Task task) =>
            new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Date = task.Date,
                IsComplete = task.IsComplete
            };
    }
}
