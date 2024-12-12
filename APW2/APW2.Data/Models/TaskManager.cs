using System;
using System.Collections.Generic;

namespace APW2.Data.Models;

public partial class TaskManager
{
    public int TaskId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public string? AssignedTo { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Category { get; set; }

    public string? Notes { get; set; }

    public bool? IsArchived { get; set; }
}
