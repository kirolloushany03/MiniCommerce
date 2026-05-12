using Microsoft.EntityFrameworkCore;

namespace MiniCommerce.UserService.Models;

[Index(nameof(Email), IsUnique = true)]

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal WalletBalance { get; set; }
}
