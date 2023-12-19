using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Configuration.EntityFramework.Entities;

public class ConfigurationValueEntity
{
    [Key]
    [Column(nameof(ConfigurationValueId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConfigurationValueId { get; set; }

    [Required]
    [ForeignKey(nameof(ConfigurationEntryId))]
    public virtual ConfigurationEntryEntity ConfigurationEntry { get; set; } = null!;
    public int ConfigurationEntryId { get; set; }

    [ForeignKey(nameof(ConfigurationValueDataTypeId))]
    public virtual ConfigurationDataTypeEntity? DataType { get; set; }
    public int? ConfigurationValueDataTypeId { get; set; }

    [ForeignKey(nameof(ApplicationId))]
    public virtual ApplicationEntity? Application { get; set; }
    public int? ApplicationId { get; set; }

    [ForeignKey(nameof(EnvironmentId))]
    public virtual EnvironmentEntity? Environment { get; set; }
    public int? EnvironmentId { get; set; }

    [ForeignKey(nameof(LocalContextId))]
    public virtual LocalContextEntity? LocalContext { get; set; }
    public int? LocalContextId { get; set; }

    [Required]
    public string Value { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }
}