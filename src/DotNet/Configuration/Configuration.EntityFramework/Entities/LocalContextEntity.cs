using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Configuration.EntityFramework.Entities;

public class LocalContextEntity
{
    [Key]
    [Column(nameof(LocalContextId))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LocalContextId { get; set; }

    [Required]
    public string Identifier { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }

    public virtual List<ConfigurationValueEntity> ConfigurationValues { get; set; } = null!;
}