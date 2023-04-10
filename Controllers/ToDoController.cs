using MeuToDo.Data;
using MeuToDo.Models;
using MeuToDo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuToDo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ToDoController : ControllerBase
    {
        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> GetToDoModels([FromServices] AppDbContext context)
        {
            var todos = await context.ToDos.AsNoTracking().ToListAsync();

            return Ok(todos);
        }

        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetTodoModels([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todo = await context.ToDos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(todo);
            }
        }

        [HttpPost]
        [Route("todos")]
        public async Task<IActionResult> PostToDo([FromServices] AppDbContext context, [FromBody] CreateToDoViewModel createToDoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = new ToDoModel
            {
                Date = DateTime.Now,
                Title = createToDoViewModel.Title,
                Done = false,
            };
            try
            {
                await context.ToDos.AddAsync(todo);
                await context.SaveChangesAsync();

                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("todos/{id}")]
        public async Task<IActionResult> PutToDo([FromServices] AppDbContext context, [FromBody] CreateToDoViewModel createToDoViewModel, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    todo.Title = createToDoViewModel.Title;

                    context.ToDos.Update(todo);
                    await context.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception)
                {

                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("todos/{id}")]
        public async Task<IActionResult> Delete([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todo = await context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

            context.ToDos.Remove(todo);
            await context.SaveChangesAsync();

            return Ok("A tarefa foi deletada");
        }
    }
}
