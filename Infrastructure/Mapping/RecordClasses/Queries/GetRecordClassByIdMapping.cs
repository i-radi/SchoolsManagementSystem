namespace Infrastructure.Mapping;

public partial class RecordClassProfile
{
    public void GetRecordClassByIdMapping()
    {
        CreateMap<RecordClass, GetRecordClassDto>();

        CreateMap<RecordClass, RecordClassViewModel>().ReverseMap();
    }
}
