using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Configuration.EntityFramework.Entities;

public class EnvironmentEntity
{
    [Key]
    [Column(nameof(EnvironmentId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EnvironmentId { get; set; }

    [Required]
    public Guid DomainId { get; set; } = Guid.NewGuid();

    [Required]
    public string UniqueName { get; set; } = null!;

    [Required]
    public string DisplayName { get; set; } = null!;

    public string? AbbreviatedDisplayName { get; set; }

    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = false;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationValueEntity> ConfigurationValues { get; set; } = null!;
}