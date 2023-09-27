﻿namespace Core.IServices;

public interface IUserClassService
{
    Response<List<GetUserClassDto>> GetAll(int pageNumber, int pageSize, int userId = 0);
    Task<Response<GetUserClassDto?>> GetById(int id);
    Task<Response<GetUserClassDto>> Add(AddUserClassDto model);
    Task<Response<bool>> Update(UpdateUserClassDto model);
    Task<Response<bool>> Delete(int id);
}