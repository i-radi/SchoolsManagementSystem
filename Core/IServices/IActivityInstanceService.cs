﻿namespace Core.IServices;

public interface IActivityInstanceService
{
    Response<List<GetActivityInstanceDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetActivityInstanceDto?>> GetById(int id);
    Task<Response<GetActivityInstanceDto>> Add(AddActivityInstanceDto model);
    Task<Response<bool>> Update(UpdateActivityInstanceDto model);
    Task<Response<bool>> Delete(int id);
}