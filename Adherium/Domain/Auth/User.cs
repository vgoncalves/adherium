namespace Adherium.Domain.Auth;

public class User
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
    
    public required string Email { get; set; }
    
    public required string PasswordHash { get; set; }
    
    public required UserRole UserRole { get; set; }
}

        