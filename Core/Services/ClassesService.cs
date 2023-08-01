namespace Core.Services;

public class ClassesService : IClassesService
{
    private readonly IClassesRepo _classesRepo;
    private readonly IMapper _mapper;

    public ClassesService(IClassesRepo classesRepo, IMapper mapper)
    {
        _classesRepo = classesRepo;
        _mapper = mapper;
    }

    public Response<List<GetClassDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _classesRepo.GetTableNoTracking().Include(m => m.Grade);
        var result = PaginatedList<GetClassDto>.Create(_mapper.Map<List<GetClassDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetClassDto>>(result));
    }

    public async Task<Response<GetClassDto?>> GetById(int id)
    {
        var modelItem = await _classesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetClassDto>(modelItem))!;
    }

    public async Task<Response<GetClassDto>> Add(AddClassDto dto)
    {
        var modelItem = _mapper.Map<Classes>(dto);

        var model = await _classesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetClassDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateClassDto dto)
    {
        var modelItem = await _classesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _classesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _classesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _classesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
