using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configuration.EntityFramework.Entities;

public class LabelEntity
{
    [Key]
    [Column(nameof(LabelId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LabelId { get; set; }

    [Required]
    public string Text { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationEntryEntity> ConfigurationEntries { get; set; } = null!;
}