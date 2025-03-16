using AutoMapper;
using EcommerceWebMVC.Data;
using EcommerceWebMVC.ViewModels;

namespace EcommerceWebMVC.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, KhachHang>();
            //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap();
        }
    }
}
