namespace Core.Services;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepo _schoolsRepo;
    private readonly IMapper _mapper;

    public SchoolService(ISchoolRepo schoolsRepo, IMapper mapper)
    {
        _schoolsRepo = schoolsRepo;
        _mapper = mapper;
    }

    public Response<List<GetSchoolDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _schoolsRepo.GetTableNoTracking().Include(m => m.Organization);
        var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetSchoolDto>>(result));
    }

    public async Task<Response<GetSchoolDto?>> GetById(int id)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetSchoolDto>(modelItem))!;
    }

    public async Task<Response<GetSchoolDto>> Add(AddSchoolDto dto)
    {
        var modelItem = _mapper.Map<School>(dto);

        var model = await _schoolsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetSchoolDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateSchoolDto dto)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _schoolsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _schoolsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _schoolsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
