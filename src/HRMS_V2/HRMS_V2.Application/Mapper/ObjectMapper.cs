using AutoMapper;
using HRMS_V2.Application.Models;
using HRMS_V2.Core.Entities;

namespace HRMS_V2.Application.Mapper
{
    public class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<AdminDtoMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;

    }

    public class AdminDtoMapper : Profile
    {
        public AdminDtoMapper()
        {
            CreateMap<Holiday, HolidayModel>().ReverseMap();
        }
    }

}
