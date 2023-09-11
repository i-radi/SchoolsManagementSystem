namespace Infrastructure.Mapping;

public partial class RecordProfile
{
    public void AddRecordMapping()
    {
        CreateMap<AddRecordDto, Record>();
    }
}