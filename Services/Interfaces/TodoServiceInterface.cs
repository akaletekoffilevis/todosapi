using TodosApi.Data;
namespace TodosApi.Services.Interfaces;

public interface ITodoService
{
    Task<List<Todo>> GetUserTodos(int userId);
    Task<Todo?> GetUserTodoById(int userId, int todoId);

    Task<Todo> CreateTodo(Todo todo);
    Task<bool> UpdateTodo(int userId, int id, Todo updated);
    Task<bool> DeleteTodo(int userId, int id);
    Task<bool> ToggleCompletion(int userId, int id, bool isCompleted);
}