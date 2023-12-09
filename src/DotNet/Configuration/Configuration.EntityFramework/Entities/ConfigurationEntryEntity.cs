using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configuration.EntityFramework.Entities;

public class ConfigurationEntryEntity
{
    [Key]
    [Column(nameof(ConfigurationEntryId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConfigurationEntryId { get; set; }

    [Required]
    public Guid DomainId { get; set; } = Guid.NewGuid();

    [Required]
    [ForeignKey(nameof(ConfigurationEntryTypeId))]
    public virtual ConfigurationEntryTypeEntity ConfigurationEntryType { get; set; } = null!;
    public int ConfigurationEntryTypeId { get; set; }

    [Required]
    public string Key { get; set; } = null!;

    public string? DisplayName { get; set; }

    [ForeignKey(nameof(LabelId))]
    public virtual LabelEntity? Label { get; set; }
    public int? LabelId { get; set; }

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationValueEntity> Values { get; set; } = null!;
}