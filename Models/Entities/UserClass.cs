using Models.Entities.Identity;

namespace Models.Entities;

public class UserClass
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public virtual Classes? Classes { get; set; }

    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    public int UserTypeId { get; set; }
    [ForeignKey(nameof(UserTypeId))]
    public virtual UserType? UserType { get; set; }

    public int SeasonId { get; set; }
    [ForeignKey(nameof(SeasonId))]
    public virtual Season? Season { get; set; }

}
