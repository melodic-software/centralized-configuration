using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Configuration.EntityFramework.Entities;

public class ConfigurationEntryTypeEntity
{
    [Key]
    [Column(nameof(ConfigurationEntryTypeId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConfigurationEntryTypeId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationEntryEntity> ConfigurationEntries { get; set; } = null!;
}