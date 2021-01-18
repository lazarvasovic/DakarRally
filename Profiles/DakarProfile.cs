using AutoMapper;
using DakarRally.Dtos;
using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Profiles
{
    public class DakarProfile : Profile
    {
        public DakarProfile() 
        {
            CreateMap<RaceModel, RaceDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<VehicleCreateDto, VehicleModel>()
                .ForMember(dest => dest.VehicleSubtype, opt => opt.Ignore());

            CreateMap<VehicleUpdateDto, VehicleModel>()
                .ForMember(dest => dest.VehicleSubtype, opt => opt.Ignore());

            CreateMap<VehicleModel, VehicleLeaderboardDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.VehicleStatus.ToString()))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleSubtype.VehicleType.TypeName))
                .ForMember(dest => dest.VehicleSubtype, opt => opt.MapFrom(src => src.VehicleSubtype.SubtypeName));

            CreateMap<MalfunctionStatisticModel, MalfunctionStatisticDto>()
                .ForMember(dest => dest.MalfunctionType, opt => opt.MapFrom(src => src.MalfunctionType.ToString()));

            CreateMap<VehicleModel, VehicleStatisticDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.VehicleStatus.ToString()));

            CreateMap<VehicleModel, VehicleInfoDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.VehicleStatus.ToString()))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleSubtype.VehicleType.TypeName))
                .ForMember(dest => dest.VehicleSubtype, opt => opt.MapFrom(src => src.VehicleSubtype.SubtypeName));
        }
    }
}
