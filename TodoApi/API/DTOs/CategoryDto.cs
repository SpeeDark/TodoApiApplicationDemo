using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.API.DTOs
{
    #region RequestDtos

    public class AddCategoryDto
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(256)]
        public string? Description { get; set; }
    }

    public class PutCategoryDto
    {
        [StringLength(100)]
        public string? Title { get; set; }
        
        [StringLength(256)]
        public string? Description { get; set; }
    }

    #endregion

    #region ResponseDtos

    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
    }

    public class CategoryWithTodoListResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public List<TodoResponseDto>? Todos { get; set; }
    }

    #endregion
}
