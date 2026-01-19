
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TodosApi.Data;
using TodosApi.Services.Interfaces;

namespace TodosApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// Gets all tasks for the authenticated user
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserTasks()
    {
        var userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized(new { message = "Invalid token" });

        var todos = await _todoService.GetUserTodos(userId);
        return Ok(todos);
    }

    /// <summary>
    /// Gets a specific task for the authenticated user
    /// </summary>
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized(new { message = "Invalid token" });

        var todo = await _todoService.GetUserTodoById(userId, id);
        if (todo == null)
            return NotFound(new { message = "Task not found" });

        return Ok(todo);
    }

    public class TodoCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;
    }

    public class TodoUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }

    /// <summary>
    /// Creates a new task for the authenticated user
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserIdFromToken();
        if (userId == -1)
           return Unauthorized(new { message = "Invalid token" });

        var todo = new Todo
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        var created = await _todoService.CreateTodo(todo);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing task for the authenticated user
    /// </summary>
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized(new { message = "Invalid token" });

        var model = new Todo
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty,
            IsCompleted = dto.IsCompleted
        };

        var ok = await _todoService.UpdateTodo(userId, id, model);
        if (!ok)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }

    /// <summary>
    /// Marks a task as completed or incomplete
    /// </summary>
    [Authorize]
    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> SetComplete(int id, [FromQuery] bool value = true)
    {
        var userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized(new { message = "Invalid token" });

        var ok = await _todoService.ToggleCompletion(userId, id, value);
        if (!ok)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }

    /// <summary>
    /// Deletes a task for the authenticated user
    /// </summary>
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized(new { message = "Invalid token" });

        var ok = await _todoService.DeleteTodo(userId, id);
        if (!ok)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }

    /// <summary>
    /// Extracts the user ID from the JWT token claims
    /// </summary>
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return -1;

        return userId;
    }
}