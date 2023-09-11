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

    public Response<List<GetCourseDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
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

        return ResponseHandler.Success(_mapper.Map<List<GetCourseDto>>(result));
    }

    public async Task<Response<GetCourseDto?>> GetById(int id)
    {
        var modelItem = await _coursesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetCourseDto>(modelItem))!;
    }

    public async Task<Response<GetCourseDto>> Add(AddCourseDto dto)
    {
        var modelItem = _mapper.Map<Course>(dto);

        var model = await _coursesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetCourseDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateCourseDto dto)
    {
        var modelItem = await _coursesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _coursesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _coursesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _coursesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
