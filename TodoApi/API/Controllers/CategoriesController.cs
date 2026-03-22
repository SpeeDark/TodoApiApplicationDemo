using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using TodoApi.Models;
using TodoApi.API.DTOs;
using System;
using System.Security.Claims;

namespace TodoApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            var userId = GetUserIdFromJwt();

            var categories = await _context.Categories
                .Where(category => category.OwnerId == userId)
                .ToListAsync();

            var response = _mapper.Map<List<CategoryResponseDto>>(categories);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
        {
            var userId = GetUserIdFromJwt();

            var category = await _context.Categories
                .Where(category => category.Id == id && category.OwnerId == userId)
                .FirstOrDefaultAsync();

            if (category == null) return NotFound();

            var response = _mapper.Map<CategoryResponseDto>(category);

            return Ok(response);
        }

        [HttpGet("{id}/todos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetTodosInCategory(int id)
        {
            var userId = GetUserIdFromJwt();

            var category = await _context.Categories
                .Include(category => category.Todos)
                .FirstOrDefaultAsync(category => category.Id == id && category.OwnerId == userId);

            if (category == null) return NotFound();

            return Ok(_mapper.Map<List<TodoResponseDto>>(category.Todos));
        }

        [HttpGet("list-with-todo")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryWithTodoListResponseDto>>> GetCategoryListWithTodo()
        {
            var userId = GetUserIdFromJwt();

            var categories = await _context.Categories
                .Include(category => category.Todos)
                .Where(category => category.OwnerId == userId)
                .ToListAsync();

            var response = _mapper.Map<List<CategoryWithTodoListResponseDto>>(categories);
            
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CategoryResponseDto>> AddCategory(AddCategoryDto categoryDto)
        {
            var userId = GetUserIdFromJwt();
            
            var existingCategory = await _context.Categories
                .AnyAsync(category => category.OwnerId == userId && category.Title == categoryDto.Title);

            if (existingCategory) return BadRequest("Category already exists");

            var category = _mapper.Map<Category>(categoryDto);
            category.OwnerId = userId;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<CategoryResponseDto>(category);

            return CreatedAtAction(nameof(AddCategory), new { id = category.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> PutCategory(int id, PutCategoryDto categoryDto)
        {
            var userId = GetUserIdFromJwt();

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(category => category.Id == id && category.OwnerId == userId);

            if (existingCategory == null) return NotFound();

            if (categoryDto.Title is not null) existingCategory.Title = categoryDto.Title;
            if (categoryDto.Description is not null) existingCategory.Description = categoryDto.Description;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var userId = GetUserIdFromJwt();

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(category => category.Id == id && category.OwnerId == userId);

            if (existingCategory == null) return NotFound();

            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private int GetUserIdFromJwt() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
    }
}
