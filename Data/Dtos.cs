using System.ComponentModel.DataAnnotations;

namespace TodosApi.Data;

public record RegisterRequest(
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    string Username,
    [Required(ErrorMessage = "Password is required")]
    [StringLength(int.MaxValue, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    string Password
);

public record LoginRequest(
    [Required(ErrorMessage = "Username is required")]
    string Username,
    [Required(ErrorMessage = "Password is required")]
    string Password
);

public record TodoCreateRequest(
    [Required(ErrorMessage = "Title is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
    string Title,
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    string? Description
);

public record TodoUpdateRequest(
    [Required(ErrorMessage = "Title is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
    string Title,
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    string? Description,
    bool IsCompleted
);

public record TodoResponse(
    int Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime CreatedAt,
    int UserId
);

public record UserResponse(
    int Id,
    string Username
);

public record AuthResponse(
    string Token,
    UserResponse User,
    string Message
);

public record RegisterResponse(
    int Id,
    string Username,
    string Message
);

public record ErrorResponse(
    string Message
);
