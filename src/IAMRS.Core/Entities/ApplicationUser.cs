namespace IAMRS.Core.Entities;

/// <summary>
/// Extended user entity with additional profile information.
/// Note: This is a POCO class; Identity integration is in Infrastructure layer.
/// </summary>
public class ApplicationUser
{
    /// <summary>
    /// User identifier (matches ASP.NET Identity Id).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Username for login.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Full display name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// User's job title.
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// Department the user belongs to.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Phone number for contact.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Whether the user account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp of last login.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Timestamp when user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User's timezone for display purposes.
    /// </summary>
    public string? TimeZone { get; set; }

    /// <summary>
    /// Notification preferences (JSON).
    /// </summary>
    public string? NotificationPreferences { get; set; }
}
