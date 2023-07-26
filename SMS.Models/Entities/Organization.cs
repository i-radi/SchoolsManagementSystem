using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SMS.Models.Entities.Identity;

namespace SMS.Models.Entities;

public class Organization
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<School> Schools { get; set; } = new HashSet<School>();
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

}
