﻿namespace Infrastructure.Mapping;

public partial class UserProfile : Profile
{
    public UserProfile()
    {
        GetUserByIdMapping();
        UpdateUserMapping();
    }
}
