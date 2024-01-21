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

    public async Task<Result<List<GetOrganizationDto>>> GetAllWithSchools()
    {
        var modelItems = await _organizationsRepo
            .GetTableNoTracking()
            .Include(s => s.Schools)
            .ToListAsync();

        var orgs = new List<GetOrganizationDto>();

        if (modelItems != null && modelItems.Any())
        {
            foreach (var organization in modelItems)
            {
                var orgDto = new GetOrganizationDto
                {
                    Name = organization.Name,
                    PicturePath = organization.PicturePath,
                    Schools = organization.Schools.Select(school => new GetSchoolDto
                    {
                        Name = school.Name,
                        PicturePath = school.PicturePath,
                        Description = school.Description,
                        Order = school.Order
                    }).ToList()
                };

                orgs.Add(orgDto);
            }
        }

        return ResultHandler.Success(orgs);
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
