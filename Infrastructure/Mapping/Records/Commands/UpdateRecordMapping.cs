namespace Infrastructure.Mapping;

public partial class RecordProfile
{
    public void UpdateRecordMapping()
    {
        CreateMap<UpdateRecordDto, Record>();
    }
}
