namespace Infrastructure.Mapping;

public partial class RecordProfile
{
    public void GetRecordByIdMapping()
    {
        //CreateMap<Record, GetRecordDto>();

        CreateMap<Record, RecordViewModel>().ReverseMap();
    }
}
