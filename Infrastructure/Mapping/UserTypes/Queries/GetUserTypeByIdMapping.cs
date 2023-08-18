﻿namespace Infrastructure.Mapping;

public partial class UserTypeProfile
{
    public void GetUserTypeByIdMapping()
    {
        CreateMap<UserType, GetUserTypeDto>();
        CreateMap<UserType, UserTypeViewModel>().ReverseMap();
    }
}
