using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configuration.EntityFramework.Entities;

public class ApplicationEntity
{
    [Key]
    [Column(nameof(ApplicationId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ApplicationId { get; set; }

    public Guid DomainId { get; set; } = Guid.NewGuid();

    [Required]
    public string UniqueName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public string? AbbreviatedName { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = false;

    [Required]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationValueEntity> ConfigurationValues { get; set; } = null!;
}