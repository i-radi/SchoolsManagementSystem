namespace Core.Services;

public class ClassRoomService : IClassRoomService
{
    private readonly IClassRoomRepo _classRoomsRepo;
    private readonly IMapper _mapper;

    public ClassRoomService(IClassRoomRepo classRoomsRepo, IMapper mapper)
    {
        _classRoomsRepo = classRoomsRepo;
        _mapper = mapper;
    }

    public Response<List<GetClassRoomDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _classRoomsRepo.GetTableNoTracking().Include(m => m.Grade);
        var result = PaginatedList<GetClassRoomDto>.Create(_mapper.Map<List<GetClassRoomDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetClassRoomDto>>(result));
    }

    public async Task<Response<GetClassRoomDto?>> GetById(int id)
    {
        var modelItem = await _classRoomsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetClassRoomDto>(modelItem))!;
    }

    public async Task<Response<GetClassRoomDto>> Add(AddClassRoomDto dto)
    {
        var modelItem = _mapper.Map<ClassRoom>(dto);

        var model = await _classRoomsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetClassRoomDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateClassRoomDto dto)
    {
        var modelItem = await _classRoomsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _classRoomsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _classRoomsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _classRoomsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
