namespace Core.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepo _activitiesRepo;
    private readonly IMapper _mapper;

    public ActivityService(IActivityRepo activitysRepo, IMapper mapper)
    {
        _activitiesRepo = activitysRepo;
        _mapper = mapper;
    }

    public Response<List<GetActivityDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _activitiesRepo.GetTableNoTracking();
        ;
        if (schoolId > 0)
        {
            modelItems = modelItems.Include(c => c.School)
                .Where(cr => cr.SchoolId == schoolId);
        }
        else
        {
            modelItems = modelItems.Include(c => c.School);
        }

        var result = PaginatedList<GetActivityDto>.Create(_mapper.Map<List<GetActivityDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetActivityDto>>(result));
    }

    public async Task<Response<GetActivityDto?>> GetById(int id)
    {
        var modelItem = await _activitiesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetActivityDto>(modelItem))!;
    }

    public async Task<Response<GetActivityDto>> Add(AddActivityDto dto)
    {
        var modelItem = _mapper.Map<Activity>(dto);

        var model = await _activitiesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetActivityDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateActivityDto dto)
    {
        var modelItem = await _activitiesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activitiesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _activitiesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _activitiesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}

