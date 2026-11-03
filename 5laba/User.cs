using System;

public enum Role
{
    User,
    Manager,
    Admin
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime Date { get; set; }
    public Role UserRole { get; set; }

    
    public override string ToString()
    {
        return $"Id:{Id} | {Name} | Email: {Email} | {UserRole}";
    }
}