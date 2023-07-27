namespace SMS.Core.Services;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepo _schoolRepo;
    private readonly IMapper _mapper;

    public SchoolService(ISchoolRepo schoolRepo, IMapper mapper)
    {
        _schoolRepo = schoolRepo;
        _mapper = mapper;
    }

    public List<GetSchoolDto> GetAll()
    {
        var modelItems = _schoolRepo.GetTableNoTracking();

        return _mapper.Map<List<GetSchoolDto>>(modelItems);
    }

    public async Task<GetSchoolDto?> GetById(int id)
    {
        var modelItem = await _schoolRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetSchoolDto>(modelItem);
    }

    public async Task<GetSchoolDto> Add(AddSchoolDto dto)
    {
        var modelItem = _mapper.Map<School>(dto);

        var model = await _schoolRepo.AddAsync(modelItem);
        await _schoolRepo.SaveChangesAsync();

        return _mapper.Map<GetSchoolDto>(modelItem);
    }

    public async Task<bool> Update(UpdateSchoolDto dto)
    {
        var modelItem = await _schoolRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _schoolRepo.UpdateAsync(modelItem);
        await _schoolRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _schoolRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _schoolRepo.DeleteAsync(dbModel);
        await _schoolRepo.SaveChangesAsync();
        return true;
    }
}
