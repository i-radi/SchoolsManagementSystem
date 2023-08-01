namespace Core.Services;

public class SeasonService : ISeasonService
{
    private readonly ISeasonRepo _seasonsRepo;
    private readonly IMapper _mapper;

    public SeasonService(ISeasonRepo seasonsRepo, IMapper mapper)
    {
        _seasonsRepo = seasonsRepo;
        _mapper = mapper;
    }

    public Response<List<GetSeasonDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _seasonsRepo.GetTableNoTracking().Include(m => m.School);
        var result = PaginatedList<GetSeasonDto>.Create(_mapper.Map<List<GetSeasonDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetSeasonDto>>(result));
    }

    public async Task<Response<GetSeasonDto?>> GetById(int id)
    {
        var modelItem = await _seasonsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetSeasonDto>(modelItem))!;
    }

    public async Task<Response<GetSeasonDto>> Add(AddSeasonDto dto)
    {
        var modelItem = _mapper.Map<Season>(dto);

        var model = await _seasonsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetSeasonDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateSeasonDto dto)
    {
        var modelItem = await _seasonsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _seasonsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _seasonsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _seasonsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
