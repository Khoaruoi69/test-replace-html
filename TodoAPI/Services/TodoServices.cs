
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TodoAPI.AppDataContext;
using TodoAPI.Contracts;
using TodoAPI.Interface;
using TodoAPI.Models;

namespace TodoAPI.Services
{
    public class TodoServices : ITodoServices
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<TodoServices> _logger;
        private readonly IMapper _mapper;

        public TodoServices(TodoDbContext context, ILogger<TodoServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }




        //  Create Todo for it be save in the datbase 

        public async Task<string> CreateTodoAsync(CreateTodoRequest request)
        {
            try
            {
                //var todo = _mapper.Map<Todo>(request);
                //todo.CreatedAt = DateTime.Now;
                //_context.Todos.Add(todo);
                //await _context.SaveChangesAsync();

                string filePath = @"D:\DOT_NET\To-do-app\Todo\html\test.html";
                

                // Đọc nội dung của file HTML
                string htmlContent = File.ReadAllText(filePath);

                StringBuilder tableBuilder = new StringBuilder();

                for (int i = 0; i < 5; i++)
                {
                    
                    tableBuilder.Append($"<tr><th>Name</th><th>Age {i}</th></tr>");
                    tableBuilder.Append($"<tr><td>John Doe</td><td>{30 + i}</td></tr>");
                    tableBuilder.Append($"<tr><td>Jane Doe</td><td>{25 + i}</td></tr>");
                   
                }

                // Thay thế @tablereplace bằng bảng đã tạo
                string updatedHtml = htmlContent.Replace("@tablereplace", tableBuilder.ToString());

                // Ghi lại file HTML hoặc sử dụng theo nhu cầu
                byte[] htmlBytes = Encoding.UTF8.GetBytes(updatedHtml);
                string base64Html = Convert.ToBase64String(htmlBytes);

                return base64Html;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the todo item.");
                throw new Exception("An error occurred while creating the todo item.");
            }
        }


        public async Task<Todo> GetByIdAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new Exception($" No Items with {id} found ");
            }
            return todo;
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoRequest request)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    throw new Exception($"Todo item with id {id} not found.");
                }

                if (request.Title != null)
                {
                    todo.Title = request.Title;
                }

                if (request.Description != null)
                {
                    todo.Description = request.Description;
                }

                if (request.IsComplete != null)
                {
                    todo.IsComplete = request.IsComplete.Value;
                }

                if (request.DueDate != null)
                {
                    todo.DueDate = request.DueDate.Value;
                }

                if (request.Priority != null)
                {
                    todo.Priority = request.Priority.Value;
                }

                todo.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the todo item with id {id}.");
                throw;
            }
        }
        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            var todo = await _context.Todos.ToListAsync();
            if (todo == null)
            {
                throw new Exception(" No Todo items found");
            }
            return todo;

        }
        public async Task DeleteTodoAsync(Guid id)
        {

            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new Exception($"No  item found with the id {id}");
            }


        }




    }
}