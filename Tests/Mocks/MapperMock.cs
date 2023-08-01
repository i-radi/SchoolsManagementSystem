using Infrastructure.Mapping;

namespace Test;

public static class MapperMock
{
    public static IMapper GetMapperMock(IConfiguration conf)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AuthenticationProfile());
            cfg.AddProfile(new ClassRoomProfile());
            cfg.AddProfile(new GradeProfile());
            cfg.AddProfile(new OrganizationProfile());
            cfg.AddProfile(new SchoolProfile());
            cfg.AddProfile(new SeasonProfile());
            cfg.AddProfile(new UserClassProfile());
            cfg.AddProfile(new UserTypeProfile());
        });

        var mapper = config.CreateMapper();

        return mapper;
    }
}