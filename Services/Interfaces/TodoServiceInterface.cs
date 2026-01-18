using TodosApi.Data;
namespace TodosApi.Services.Interfaces;

public interface ITodoService
{
    // Get todos for a specific user
    Task<List<Todo>> GetUserTodos(int userId);
    
    // Get a specific todo (with user verification)
    Task<Todo?> GetTodoById(int id);
    
    // Get a todo and verify it belongs to the user
    Task<Todo?> GetUserTodoById(int userId, int todoId);
    
    Task<Todo> CreateTodo(Todo todo);
    Task<bool> UpdateTodo(int userId, int id, Todo updated);
    Task<bool> DeleteTodo(int userId, int id);
    Task<bool> ToggleCompletion(int userId, int id, bool isCompleted);
}