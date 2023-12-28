using AutoMapper;

namespace Configuration.API.Tests.UnitTests.Services;

public static class AutoMapperInstanceCreator
{
    public static IMapper CreateWithProfile<TProfile>() where TProfile : Profile, new()
    {
        // we don't use a mocked mapper here, instead we use a mapping configuration with the registered profile type
        // this technically is a bleed into an integration test, but it doesn't really make sense to write mapping tests on a mock (mapper) object
        // the alternative approach is to split out mapping tests into their own tests, and leave any mapping tests out of the controller / action result tests
        // in most cases it's less overhead to be pragmatic about the number and type of assertions made in a single test

        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<TProfile>());
        IMapper mapper = new Mapper(mapperConfiguration);
        return mapper;
    }
}