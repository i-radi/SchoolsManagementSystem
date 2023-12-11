namespace Core.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepo _coursesRepo;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepo coursesRepo, IMapper mapper)
    {
        _coursesRepo = coursesRepo;
        _mapper = mapper;
    }

    public Result<List<GetCourseDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _coursesRepo.GetTableNoTracking();
        ;
        if (schoolId > 0)
        {
            modelItems = modelItems.Include(c => c.School).Where(cr => cr.SchoolId == schoolId);
        }
        else
        {
            modelItems = modelItems.Include(c => c.School);
        }

        var result = PaginatedList<GetCourseDto>.Create(_mapper.Map<List<GetCourseDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetCourseDto>>(result));
    }

    public async Task<Result<GetCourseDto?>> GetById(int id)
    {
        var modelItem = await _coursesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetCourseDto>(modelItem))!;
    }

    public async Task<Result<GetCourseDto>> Add(AddCourseDto dto)
    {
        var modelItem = _mapper.Map<Course>(dto);
        modelItem.CourseDetails = new CourseDetails
        {
            ContentType = dto.ContentType,
            Content = dto.Content ?? ""
        };

        var model = await _coursesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetCourseDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateCourseDto dto)
    {
        var modelItem = await _coursesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);
        if (modelItem.CourseDetails is not null)
        {
            modelItem.CourseDetails.ContentType = dto.ContentType;
            modelItem.CourseDetails.Content = dto.Content ?? "";
        }
        var model = _coursesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _coursesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _coursesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
