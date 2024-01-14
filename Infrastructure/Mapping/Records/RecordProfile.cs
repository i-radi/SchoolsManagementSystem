namespace Infrastructure.Mapping;

public partial class RecordProfile : Profile
{
    public RecordProfile()
    {
        GetRecordByIdMapping();
        AddRecordMapping();
        UpdateRecordMapping();
    }
}
