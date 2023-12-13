namespace Core.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepo _organizationsRepo;
    private readonly IMapper _mapper;

    public OrganizationService(IOrganizationRepo organizationsRepo, IMapper mapper)
    {
        _organizationsRepo = organizationsRepo;
        _mapper = mapper;
    }

    public Result<PaginatedList<GetOrganizationDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _organizationsRepo.GetTableNoTracking();
        var result = PaginatedList<GetOrganizationDto>.Create(_mapper.Map<List<GetOrganizationDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetOrganizationDto?>> GetById(int id)
    {
        var modelItem = await _organizationsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetOrganizationDto>(modelItem))!;
    }

    public async Task<Result<GetOrganizationDto>> Add(AddOrganizationDto dto)
    {
        var modelItem = _mapper.Map<Organization>(dto);

        var model = await _organizationsRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetOrganizationDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateOrganizationDto dto)
    {
        var modelItem = await _organizationsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _organizationsRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _organizationsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _organizationsRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
