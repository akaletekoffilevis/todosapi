
using Microsoft.EntityFrameworkCore;
using TodosApi.Data;
using TodosApi.Services.Interfaces;

namespace TodosApi.Services;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _db;

    public TodoService(TodoDbContext db)
    {
        _db = db;
    }

    public async Task<List<Todo>> GetUserTodos(int userId)
    {
        return await _db.Todos
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Todo?> GetTodoById(int id)
    {
        return await _db.Todos.FindAsync(id);
    }

    public async Task<Todo?> GetUserTodoById(int userId, int todoId)
    {
        return await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == todoId && t.UserId == userId);
    }

    public async Task<Todo> CreateTodo(Todo todo)
    {
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> UpdateTodo(int userId, int id, Todo updated)
    {
        var exist_todo = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (exist_todo == null) return false;

        exist_todo.Title = updated.Title;
        exist_todo.Description = updated.Description;
        exist_todo.IsCompleted = updated.IsCompleted;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTodo(int userId, int id)
    {
        var exist_todo = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (exist_todo == null) return false;

        _db.Todos.Remove(exist_todo);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleCompletion(int userId, int id, bool isCompleted)
    {
        var exist_todo = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (exist_todo == null) return false;

        exist_todo.IsCompleted = isCompleted;
        await _db.SaveChangesAsync();
        return true;
    }
}