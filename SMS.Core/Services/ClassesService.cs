using Microsoft.EntityFrameworkCore;

namespace SMS.Core.Services;

public class ClassesService : IClassesService
{
    private readonly IClassesRepo _classesRepo;
    private readonly IMapper _mapper;

    public ClassesService(IClassesRepo classesRepo, IMapper mapper)
    {
        _classesRepo = classesRepo;
        _mapper = mapper;
    }

    public List<GetClassDto> GetAll()
    {
        var modelItems = _classesRepo.GetTableNoTracking().Include(m => m.Grade);

        return _mapper.Map<List<GetClassDto>>(modelItems);
    }

    public async Task<GetClassDto?> GetById(int id)
    {
        var modelItem = await _classesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetClassDto>(modelItem);
    }

    public async Task<GetClassDto> Add(AddClassDto dto)
    {
        var modelItem = _mapper.Map<Classes>(dto);

        var model = await _classesRepo.AddAsync(modelItem);
        await _classesRepo.SaveChangesAsync();

        return _mapper.Map<GetClassDto>(modelItem);
    }

    public async Task<bool> Update(UpdateClassDto dto)
    {
        var modelItem = await _classesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _classesRepo.UpdateAsync(modelItem);
        await _classesRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _classesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _classesRepo.DeleteAsync(dbModel);
        await _classesRepo.SaveChangesAsync();
        return true;
    }
}
