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

    public Response<List<GetOrganizationDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _organizationsRepo.GetTableNoTracking();
        var result = PaginatedList<GetOrganizationDto>.Create(_mapper.Map<List<GetOrganizationDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetOrganizationDto>>(result));
    }

    public async Task<Response<GetOrganizationDto?>> GetById(int id)
    {
        var modelItem = await _organizationsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetOrganizationDto>(modelItem))!;
    }

    public async Task<Response<GetOrganizationDto>> Add(AddOrganizationDto dto)
    {
        var modelItem = _mapper.Map<Organization>(dto);

        var model = await _organizationsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetOrganizationDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateOrganizationDto dto)
    {
        var modelItem = await _organizationsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _organizationsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _organizationsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _organizationsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
