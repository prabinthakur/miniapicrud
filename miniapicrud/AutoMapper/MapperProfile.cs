using AutoMapper;
using miniapicrud.Model;
using miniapicrud.Model.dto;

namespace miniapicrud.AutoMapper
{
    public class MapperProfile:Profile
    {

        public MapperProfile()
        {
            
            CreateMap<AddRequestDto, Product> ();
            CreateMap<UpdateRequestDto, Product>();
            CreateMap<Product, TotalPriceDtoResponse>();
            CreateMap<RegisterDto, User>();
          
        }
    }
}
