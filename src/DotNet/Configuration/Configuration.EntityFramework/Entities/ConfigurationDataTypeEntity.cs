using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configuration.EntityFramework.Entities;

public class ConfigurationDataTypeEntity
{
    [Key]
    [Column(nameof(ConfigurationValueDataTypeId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConfigurationValueDataTypeId { get; set; }

    public string Name { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationValueEntity> ConfigurationValues { get; set; } = null!;
}