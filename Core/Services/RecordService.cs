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

    public Result<List<GetRecordDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _recordsRepo
            .GetTableNoTracking()
            .Include(c => c.School)
            .Where(c => c.Available);
        ;
        if (schoolId > 0)
        {
            modelItems = modelItems.Where(cr => cr.SchoolId == schoolId);
        }

        var result = PaginatedList<GetRecordDto>.Create(_mapper.Map<List<GetRecordDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetRecordDto>>(result));
    }

    public async Task<Result<GetRecordDto?>> GetById(int id)
    {
        var modelItem = await _recordsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetRecordDto>(modelItem))!;
    }

    public async Task<Result<GetRecordDto>> Add(AddRecordDto dto)
    {
        var modelItem = _mapper.Map<Record>(dto);

        var model = await _recordsRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetRecordDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateRecordDto dto)
    {
        var modelItem = await _recordsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _recordsRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _recordsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        dbModel.Available = false;

        var model = _recordsRepo.UpdateAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
