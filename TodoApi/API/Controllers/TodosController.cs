using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApi.API.DTOs;
using TodoApi.Models;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodosController(TodoContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetTodos()
        {
            var userId = GetUserIdFromJwt();

            var todos = await _context.Todos
                .Where(todo => todo.OwnerId == userId)
                .Include(todo => todo.Category)
                .ToListAsync();

            return Ok(_mapper.Map<List<TodoResponseDto>>(todos));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoResponseDto>> GetTodo(int id)
        {
            var userId = GetUserIdFromJwt();

            var todo = await _context.Todos
                .Where(todo => todo.Id == id && todo.OwnerId == userId)
                .Include(todo => todo.Category)
                .FirstOrDefaultAsync();

            if (todo == null) return NotFound();
            
            return Ok(_mapper.Map<TodoResponseDto>(todo));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Todo>> AddTodo(AddTodoDto todoDto)
        {
            var userId = GetUserIdFromJwt();

            if (todoDto.CategoryId is not null)
            {
                var existingCategory = await _context.Categories
                    .Where(cat => cat.Id == todoDto.CategoryId && cat.OwnerId == userId)
                    .FirstOrDefaultAsync();

                if (existingCategory == null) return BadRequest();
            }

            var todo = _mapper.Map<Todo>(todoDto);
            todo.OwnerId = userId;

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<TodoResponseDto>(todo);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutTodo(int id, PutTodoDto todoDto)
        {
            var userId = GetUserIdFromJwt();

            var existingTodo = await _context.Todos
                .Where(todo => todo.Id == id && todo.OwnerId == userId)
                .FirstOrDefaultAsync();

            if (existingTodo == null) return NotFound();

            if (todoDto.Title is not null) existingTodo.Title = todoDto.Title;
            if (todoDto.Description is not null) existingTodo.Description = todoDto.Description;
            existingTodo.IsComplete = todoDto.IsComplete;
            existingTodo.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var userId = GetUserIdFromJwt();

            var todo = await _context.Todos
                .Where(todo => todo.Id == id && todo.OwnerId == userId)
                .FirstOrDefaultAsync();
            
            if (todo == null) return BadRequest();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private int GetUserIdFromJwt() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
    }
}
