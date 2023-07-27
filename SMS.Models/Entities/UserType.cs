namespace SMS.Models.Entities;

public class UserType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
}
