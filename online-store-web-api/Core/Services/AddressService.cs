using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.Address;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Services
{
    public class AddressService(UserManager<User> userManager,
                                    IRepository<Address> addressRepo,
                                    IMapper mapper) : IAddressesService
    {
        public async Task Create(CreateAddressDto addressDto)
        {
            _ = await userManager.FindByIdAsync(addressDto.UserId)
                ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            var address = mapper.Map<Address>(addressDto);
            await addressRepo.Insert(address);
            await addressRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await addressRepo.GetByID(id)
                ?? throw new HttpException(ErrorMessages.AddressByIdNotFound, HttpStatusCode.NotFound);

            await addressRepo.Delete(id);
            await addressRepo.Save();
        }

        public async Task Update(int addressId, UpdateAddressDto address)
        {
            var existingAddress = await addressRepo
                .GetByID(addressId) ?? throw new HttpException(ErrorMessages.CommentByIdNotFound, HttpStatusCode.NotFound);

            mapper.Map(address, existingAddress);
            existingAddress.Id = addressId;

            await addressRepo.Update(existingAddress);
            await addressRepo.Save();
        }

        public async Task<IEnumerable<GetAddressDto>> GetAll()
        {
            var address = await addressRepo.GetAllBySpec(new Addresses.All());

            return mapper.Map<IEnumerable<GetAddressDto>>(address);
        }

        public async Task<GetAddressDto> GetById(int id)
        {
            Address address = await addressRepo.GetBySpec(new Addresses.ById(id))
                ?? throw new HttpException(ErrorMessages.AddressByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetAddressDto>(address);
        }

        public async Task<GetAddressDto> GetByUserId(string userId)
        {
            Address address = await addressRepo.GetBySpec(new Addresses.ByUserId(userId))
                ?? throw new HttpException(ErrorMessages.AddressByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetAddressDto>(address);
        }
    }
}
