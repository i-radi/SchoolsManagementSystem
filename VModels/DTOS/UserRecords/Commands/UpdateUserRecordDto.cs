namespace VModels.DTOS;

public class UpdateUserRecordDto
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public int UserId { get; set; }
    public DateTime? CreateDate { get; set; } = DateTime.Now;
    public bool IsDone { get; set; }
    public DateTime? DoneDate { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}
