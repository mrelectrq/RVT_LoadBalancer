using AutoMapper;
using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core.Mapper
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => _mapper.Value;
    }


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NodeData, NodeNeighbor>();
        }
    }
}
