using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace todoapi_v00.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly CategoryContext _categorycontext;
        private readonly ListContext _listcontext;

        public TodosController(TodoContext context,CategoryContext categoryC,ListContext listC)
        {
            _context = context;
            _categorycontext = categoryC;
            _listcontext = listC;
        }

        // GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //GET:api/todos/1/Category
        [HttpGet("{Id}/Category")]

        public async Task<ActionResult<Category>> GetCategory(long Id)
        {
            var todoItem = await _context.TodoItems.FindAsync(Id);
            Category? category;
            if (todoItem is not null)
            {
                category = await _categorycontext.Categories.FindAsync(todoItem.CategoryId);
            }
            else
                return NotFound();

            if(category ==null)
                return NotFound();

            return category;
        }

        //GET:api/todos/1/List
        [HttpGet("{Id}/List")]

        public async Task<ActionResult<List>> GetList(long Id)
        {
            var todoItem = await _context.TodoItems.FindAsync(Id);
            List? list;
            if (todoItem is not null)
            {
                list = await _listcontext.Lists.FindAsync(todoItem.ListId);
            }
            else
                return NotFound();

            if (list == null)
                return NotFound();

            return list;
        }
        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}