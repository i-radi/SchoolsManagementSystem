namespace SMS.Core.Services;

public class GradesService : IGradeService
{
    private readonly IGradeRepo _gradesRepo;
    private readonly IMapper _mapper;

    public GradesService(IGradeRepo gradesRepo, IMapper mapper)
    {
        _gradesRepo = gradesRepo;
        _mapper = mapper;
    }

    public Response<List<GetGradeDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _gradesRepo.GetTableNoTracking().Include(m => m.School);
        var result = PaginatedList<GetGradeDto>.Create(_mapper.Map<List<GetGradeDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetGradeDto>>(result));
    }

    public async Task<Response<GetGradeDto?>> GetById(int id)
    {
        var modelItem = await _gradesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetGradeDto>(modelItem))!;
    }

    public async Task<Response<GetGradeDto>> Add(AddGradeDto dto)
    {
        var modelItem = _mapper.Map<Grade>(dto);

        var model = await _gradesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetGradeDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateGradeDto dto)
    {
        var modelItem = await _gradesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _gradesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _gradesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _gradesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
