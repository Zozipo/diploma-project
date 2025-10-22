using Core.Interfaces;
using Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IImagesService, ImageService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<ICartsService, CartsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IFavoriteProductsService, FavoriteProductsService>();
            services.AddScoped<IViewedProductsService, ViewedProductsService>();
            services.AddScoped<ICreditCardService, CreditCardsService>();
            services.AddScoped<IAddressesService, AddressService>();
            services.AddScoped<IBannersService, BannersService>();
            services.AddScoped<IPromoCodesService, PromoCodesService>();
            services.AddScoped<ITablePermissionsService, TablePermissionsService>();
            services.AddScoped<IModeratingTableService, ModeratingTableService>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
