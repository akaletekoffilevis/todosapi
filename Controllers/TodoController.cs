using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoService todoService, ILogger<TodoController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    /// <summary>Gets all tasks for the authenticated user</summary>
    /// <returns>List of tasks belonging to the current user</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserTasks()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var todos = await _todoService.GetUserTodos(userId.Value);
        var result = todos.Select(t => new TodoResponse(
            t.Id, t.Title, t.Description, t.IsCompleted, t.CreatedAt, t.UserId
        ));
        return Ok(result);
    }

    /// <summary>Gets a specific task by ID</summary>
    /// <param name="id">Task ID</param>
    /// <returns>The task if found and belongs to user, 404 otherwise</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var todo = await _todoService.GetUserTodoById(userId.Value, id);
        if (todo == null)
            return NotFound(new ErrorResponse("Task not found"));

        return Ok(new TodoResponse(
            todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt, todo.UserId
        ));
    }

    /// <summary>Creates a new task</summary>
    /// <param name="dto">Title (required) and description (optional, max 2000 chars)</param>
    /// <returns>201 with the created task</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoCreateRequest dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var todo = new Todo
        {
            Title = dto.Title.Trim(),
            Description = (dto.Description ?? string.Empty).Trim(),
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId.Value
        };

        var created = await _todoService.CreateTodo(todo);

        _logger.LogInformation("Task created: {Id} by user {UserId}", created.Id, userId.Value);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new TodoResponse(
            created.Id, created.Title, created.Description, created.IsCompleted, created.CreatedAt, created.UserId
        ));
    }

    /// <summary>Updates an existing task</summary>
    /// <param name="id">Task ID</param>
    /// <param name="dto">New title, description, and completion status</param>
    /// <returns>204 if successful, 404 if task not found</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateRequest dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var todo = new Todo
        {
            Title = dto.Title.Trim(),
            Description = (dto.Description ?? string.Empty).Trim(),
            IsCompleted = dto.IsCompleted
        };

        var ok = await _todoService.UpdateTodo(userId.Value, id, todo);
        if (!ok)
            return NotFound(new ErrorResponse("Task not found"));

        _logger.LogInformation("Task updated: {Id} by user {UserId}", id, userId.Value);
        return NoContent();
    }

    /// <summary>Marks a task as completed or not completed</summary>
    /// <param name="id">Task ID</param>
    /// <param name="value">true = completed, false = not completed (default: true)</param>
    /// <returns>204 if successful, 404 if task not found</returns>
    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> SetComplete(int id, [FromQuery] bool value = true)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var ok = await _todoService.ToggleCompletion(userId.Value, id, value);
        if (!ok)
            return NotFound(new ErrorResponse("Task not found"));

        _logger.LogInformation("Task {Id} completion set to {Value} by user {UserId}", id, value, userId.Value);
        return NoContent();
    }

    /// <summary>Deletes a task</summary>
    /// <param name="id">Task ID</param>
    /// <returns>204 if successful, 404 if task not found</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new ErrorResponse("Invalid token"));

        var ok = await _todoService.DeleteTodo(userId.Value, id);
        if (!ok)
            return NotFound(new ErrorResponse("Task not found"));

        _logger.LogInformation("Task deleted: {Id} by user {UserId}", id, userId.Value);
        return NoContent();
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null || !int.TryParse(claim.Value, out var userId))
            return null;
        return userId;
    }
}
