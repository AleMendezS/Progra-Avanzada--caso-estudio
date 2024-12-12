using System;
using System.Collections.Generic;

namespace APW2.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
