using Infrastructure.Mapping;
using System.Reflection;

namespace Test;

public static class MapperMock
{
    public static IMapper GetAllProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            var profileTypes = Assembly.Load("Infrastructure")
                .GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var profileType in profileTypes)
            {
                var profile = (Profile)Activator.CreateInstance(profileType)!;
                cfg.AddProfile(profile);
            }
        });

        var mapper = config.CreateMapper();

        return mapper;
    }

    public static IMapper GetOrganizationProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new OrganizationProfile());
        });

        var mapper = config.CreateMapper();

        return mapper;
    }

}