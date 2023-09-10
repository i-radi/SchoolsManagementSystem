namespace Infrastructure.Mapping;

public partial class RecordClassProfile : Profile
{
    public RecordClassProfile()
    {
        GetRecordClassByIdMapping();
        AddRecordClassMapping();
        UpdateRecordClassMapping();
    }
}
