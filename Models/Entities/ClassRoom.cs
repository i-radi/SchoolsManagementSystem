namespace Models.Entities;


public class ClassRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public int GradeId { get; set; }

    [ForeignKey(nameof(GradeId))]
    public virtual Grade? Grade { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
}
