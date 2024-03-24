using AutoMapper;
using Market.Models;
using Market.Models.DTO;

namespace Market.repo
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductModel>(MemberList.Destination).ReverseMap();
            CreateMap<Category, CategoryModel>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageModel>(MemberList.Destination).ReverseMap();
        }
    }
}
