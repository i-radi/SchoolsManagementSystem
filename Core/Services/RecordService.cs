namespace Core.Services;

public class RecordService : IRecordService
{
    private readonly IRecordRepo _recordsRepo;
    private readonly IMapper _mapper;

    public RecordService(IRecordRepo recordsRepo, IMapper mapper)
    {
        _recordsRepo = recordsRepo;
        _mapper = mapper;
    }

    public Response<List<GetRecordDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _recordsRepo.GetTableNoTracking();
        ;
        if (schoolId > 0)
        {
            modelItems = modelItems.Include(c => c.School).Where(cr => cr.SchoolId == schoolId);
        }
        else
        {
            modelItems = modelItems.Include(c => c.School);
        }

        var result = PaginatedList<GetRecordDto>.Create(_mapper.Map<List<GetRecordDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetRecordDto>>(result));
    }

    public async Task<Response<GetRecordDto?>> GetById(int id)
    {
        var modelItem = await _recordsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetRecordDto>(modelItem))!;
    }

    public async Task<Response<GetRecordDto>> Add(AddRecordDto dto)
    {
        var modelItem = _mapper.Map<Record>(dto);

        var model = await _recordsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetRecordDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateRecordDto dto)
    {
        var modelItem = await _recordsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _recordsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _recordsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _recordsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
