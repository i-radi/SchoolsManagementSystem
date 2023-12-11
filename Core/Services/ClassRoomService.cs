namespace Core.Services;

public class ClassroomService : IClassroomService
{
    private readonly IClassroomRepo _classroomsRepo;
    private readonly IMapper _mapper;

    public ClassroomService(IClassroomRepo classroomsRepo, IMapper mapper)
    {
        _classroomsRepo = classroomsRepo;
        _mapper = mapper;
    }

    public Result<List<GetClassroomDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _classroomsRepo.GetTableNoTracking();
        ;
        if (schoolId > 0)
        {
            modelItems = modelItems.Include(c => c.Grade).Where(cr => cr.Grade != null && cr.Grade.SchoolId == schoolId);
        }
        else
        {
            modelItems = modelItems.Include(c => c.Grade);
        }

        var result = PaginatedList<GetClassroomDto>.Create(_mapper.Map<List<GetClassroomDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetClassroomDto>>(result));
    }

    public async Task<Result<GetClassroomDto?>> GetById(int id)
    {
        var modelItem = await _classroomsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetClassroomDto>(modelItem))!;
    }

    public async Task<Result<GetClassroomDto>> Add(AddClassroomDto dto)
    {
        var modelItem = _mapper.Map<Classroom>(dto);

        var model = await _classroomsRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetClassroomDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateClassroomDto dto)
    {
        var modelItem = await _classroomsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _classroomsRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _classroomsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _classroomsRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
