namespace Core.Services;

public class RecordClassService : IRecordClassService
{
    private readonly IRecordClassRepo _recordClassesRepo;
    private readonly IMapper _mapper;

    public RecordClassService(IRecordClassRepo recordClassesRepo, IMapper mapper)
    {
        _recordClassesRepo = recordClassesRepo;
        _mapper = mapper;
    }

    public Response<List<GetRecordClassDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _recordClassesRepo.GetTableNoTracking()
            .Include(c => c.Record)
            .Include(c => c.Classroom);

        var result = PaginatedList<GetRecordClassDto>.Create(_mapper.Map<List<GetRecordClassDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetRecordClassDto>>(result));
    }

    public async Task<Response<GetRecordClassDto?>> GetById(int id)
    {
        var modelItem = await _recordClassesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetRecordClassDto>(modelItem))!;
    }

    public async Task<Response<GetRecordClassDto>> Add(AddRecordClassDto dto)
    {
        var modelItem = _mapper.Map<RecordClass>(dto);

        var model = await _recordClassesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetRecordClassDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateRecordClassDto dto)
    {
        var modelItem = await _recordClassesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _recordClassesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _recordClassesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _recordClassesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
