using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace SMS.Core.Services;

public class SeasonService : ISeasonService
{
    private readonly ISeasonRepo _seasonRepo;
    private readonly IMapper _mapper;

    public SeasonService(ISeasonRepo seasonRepo, IMapper mapper)
    {
        _seasonRepo = seasonRepo;
        _mapper = mapper;
    }

    public List<GetSeasonDto> GetAll()
    {
        var modelItems = _seasonRepo.GetTableNoTracking().Include(m => m.School);

        return _mapper.Map<List<GetSeasonDto>>(modelItems);
    }

    public async Task<GetSeasonDto?> GetById(int id)
    {
        var modelItem = await _seasonRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetSeasonDto>(modelItem);
    }

    public async Task<GetSeasonDto> Add(AddSeasonDto dto)
    {
        var IsSchoolHasCurrentSeason = _seasonRepo
            .GetTableAsTracking()
            .Where(s => s.SchoolId == dto.SchoolId )
            .Any(s => s.IsCurrent);

        if (dto.IsCurrent && IsSchoolHasCurrentSeason)
            throw new InvalidDataException("This school has already current season");

        var modelItem = _mapper.Map<Season>(dto);

        var model = await _seasonRepo.AddAsync(modelItem);
        await _seasonRepo.SaveChangesAsync();

        return _mapper.Map<GetSeasonDto>(modelItem);
    }

    public async Task<bool> Update(UpdateSeasonDto dto)
    {
        var modelItem = await _seasonRepo.GetByIdAsync(dto.Id);
        var IsSchoolHasCurrentSeason = _seasonRepo
            .GetTableAsTracking()
            .Where(s => s.SchoolId == dto.SchoolId && s.Id != dto.Id)
            .Any(s => s.IsCurrent);

        if ( dto.IsCurrent && IsSchoolHasCurrentSeason)
            return false;

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _seasonRepo.UpdateAsync(modelItem);
        await _seasonRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _seasonRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _seasonRepo.DeleteAsync(dbModel);
        await _seasonRepo.SaveChangesAsync();
        return true;
    }
}
