namespace Core.Services;

public class UserRecordService : IUserRecordService
{
    private readonly IUserRecordRepo _userRecordsRepo;
    private readonly IMapper _mapper;

    public UserRecordService(IUserRecordRepo uerRecordsRepo, IMapper mapper)
    {
        _userRecordsRepo = uerRecordsRepo;
        _mapper = mapper;
    }

    public Response<List<GetUserRecordDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _userRecordsRepo.GetTableNoTracking()
            .Include(c => c.Record)
            .Include(c => c.User);

        var result = PaginatedList<GetUserRecordDto>.Create(_mapper.Map<List<GetUserRecordDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetUserRecordDto>>(result));
    }

    public async Task<Response<GetUserRecordDto?>> GetById(int id)
    {
        var modelItem = await _userRecordsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetUserRecordDto>(modelItem))!;
    }

    public async Task<Response<GetUserRecordDto>> Add(AddUserRecordDto dto)
    {
        var modelItem = _mapper.Map<UserRecord>(dto);

        var model = await _userRecordsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetUserRecordDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateUserRecordDto dto)
    {
        var modelItem = await _userRecordsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _userRecordsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _userRecordsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _userRecordsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
