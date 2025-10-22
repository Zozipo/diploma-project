using AutoMapper;
using Core.Entities;
using Core.Models.Account;
using Core.Models.Address;
using Core.Models.Banner;
using Core.Models.Cart;
using Core.Models.Category;
using Core.Models.Comment;
using Core.Models.CreditCard;
using Core.Models.ModeratingTable;
using Core.Models.Order;
using Core.Models.Permission;
using Core.Models.Product;
using Core.Models.Promocode;
using Core.Models.PromoCode;
using Core.Models.User;

namespace Core.MapperProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, RegisterDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Comment, GetCommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();

            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<UpdateCommentDto, Comment>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Comment, GetCommentDto>().ReverseMap();

            CreateMap<CommentImage, GetCommnetImageDto>().ReverseMap();

            CreateMap<CreateProductDto, Product>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<Product, GetProductPreviewDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.OrderBy(img => img.Id).FirstOrDefault().Image))
                .ReverseMap();

            CreateMap<ProductImage, GetProductImageDto>().ReverseMap();

            CreateMap<Cart, GetCartDto>().ReverseMap();
            CreateMap<Cart, CreateCartDto>().ReverseMap();

            CreateMap<CartItem, GetCartItemDto>().ReverseMap();

            CreateMap<Order, GetOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();

            CreateMap<OrderItem, GetOrderItemDto>().ReverseMap();
            CreateMap<OrderItem, ListProductToOrder>().ReverseMap();

            CreateMap<CreditCard, GetCreditCardDto>().ReverseMap();
            CreateMap<CreditCard, CreateCreditCardDto>().ReverseMap();

            CreateMap<Address, GetAddressDto>().ReverseMap();
            CreateMap<Address, CreateAddressDto>().ReverseMap();
            CreateMap<Address, UpdateAddressDto>().ReverseMap();

            CreateMap<Banner, GetBannerDto>().ReverseMap();
            CreateMap<Banner, CreateBannerDto>().ReverseMap();
            CreateMap<Banner, UpdateBannerDto>().ReverseMap();

            CreateMap<PromoCode, GetPromoCodeDto>().ReverseMap();
            CreateMap<PromoCode, CreatePromoCodeDto>().ReverseMap();
            CreateMap<PromoCode, UpdatePromoCodeDto>().ReverseMap();

            CreateMap<TablePermission, GetTablePermissionDto>().ReverseMap();
            CreateMap<TablePermission, CreateTablePermissionDto>().ReverseMap();

            CreateMap<ModeratingTable, GetModeratingTableDto>().ReverseMap();
            CreateMap<ModeratingTable, CreateModeratingTableDto>().ReverseMap();
            CreateMap<UpdateModeratingTableDto, ModeratingTable>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CartItem, OrderItem>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}