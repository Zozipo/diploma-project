using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Core.Models.User;
using Core.Models.Account;
using System.Net;
using Core.Helpers;
using Core.Resources;
using Core.Models;
using Core.Models.Permission;
using Core.Specification;

namespace Core.Services
{
    public class AccountsService(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 ITablePermissionsService permissionsService,
                                 IRepository<TablePermission> permissionsRepo,
                                 IRepository<Cart> cartsRepo,
                                 IMapper mapper,
                                 IJwtService jwtService,
                                 IImagesService imagesService) : IAccountsService
    {
        public async Task<IEnumerable<GetUserDto>> GetAll()
        {
            var users = await userManager.Users.ToListAsync();

            var usersWithRoles = new List<GetUserDto>();

            foreach (var user in users)
            {
                var userDto = mapper.Map<GetUserDto>(user);
                var roles = await userManager.GetRolesAsync(user);
                userDto.Roles = [.. roles];
                userDto.Permissions = (await permissionsService.GetByUserId(user.Id)).ToList();

                usersWithRoles.Add(userDto);
            }

            return usersWithRoles;
        }

        public async Task<GetUserDto> GetById(string id)
        {
            var user = await userManager
                .FindByIdAsync(id) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var userDto = mapper.Map<GetUserDto>(user);

            userDto.Roles = (List<string>)await userManager.GetRolesAsync(user);

            var permissions = (await permissionsService.GetByUserId(user.Id)).ToList();
            userDto.Permissions = mapper.Map<List<GetTablePermissionDto>>(permissions);

            return userDto;
        }

        public async Task<GetUserDto> GetByUserName(string userName)
        {
            var user = await userManager.Users.Where(u => u.UserName == userName)
                .FirstOrDefaultAsync() ?? throw new HttpException(ErrorMessages.UserByUserNameNotFound, HttpStatusCode.NotFound);

            var userDto = mapper.Map<GetUserDto>(user);

            userDto.Roles = (List<string>)await userManager.GetRolesAsync(user);

            return userDto;
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new HttpException(ErrorMessages.InvalidCreds, HttpStatusCode.BadRequest);

            await signInManager.SignInAsync(user, true);

            return new LoginResponseDto()
            {
                Token = jwtService.CreateToken(jwtService.GetClaims(user))
            };
        }

        public async Task<LoginResponseDto> GoogleLogin(LoginGoogleDto loginDto)
        {
            if (loginDto == null)
            {
                throw new HttpException("Invalid input data", HttpStatusCode.BadRequest);
            }

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = loginDto.Email,
                    UserName = loginDto.Email,
                    FirstName = loginDto.Given_name,
                    LastName = loginDto.Family_name,
                    PhoneNumber = null,
                    Sex = "",
                    Position = "",
                    Image = "default_user.webp",
                };

                Console.WriteLine(user);
                var result = await userManager.CreateAsync(user);

                await userManager.AddToRoleAsync(user, "User");

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                    Console.WriteLine(errors);

                    string message = string.Join(", ", result.Errors.Select(x => x.Description));
                    throw new HttpException(message, HttpStatusCode.BadRequest);
                }
            }

            await signInManager.SignInAsync(user, true);

            return new LoginResponseDto
            {

                Token = jwtService.CreateToken(jwtService.GetClaims(user))
            };
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task Register(RegisterDto registerDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
                throw new HttpException(ErrorMessages.EmailAlreadyInUse, HttpStatusCode.BadRequest);

            User user = mapper.Map<User>(registerDto);

            if (registerDto.Image != null)
                user.Image = imagesService.SaveImage(registerDto.Image);
            else
                user.Image = "default_user.webp";

            user.Sex = "";
            user.Position = "";
            
            var result = await userManager.CreateAsync(user, registerDto.Password);

            await userManager.AddToRoleAsync(user, "User");

            var cart = new Cart
            {
                UserId = user.Id,
            };

            await cartsRepo.Insert(cart);
            await cartsRepo.Save();

            if (!result.Succeeded)
            {
                string message = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new HttpException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task Update(string userId, UpdateUserDto updateUserDto)
        {
            if (updateUserDto.UserName != null)
            {
                var existingUserWithSameName = await userManager.FindByNameAsync(updateUserDto.UserName);
                if (existingUserWithSameName != null && existingUserWithSameName.Id != userId)
                {
                    throw new HttpException(ErrorMessages.UserNameAlreadyExists, HttpStatusCode.Conflict);
                }
            }

            var existingUser = await userManager.FindByIdAsync(userId);

            string image = existingUser.Image;

            mapper.Map(updateUserDto, existingUser);

            if (updateUserDto.Image != null)
            {
                imagesService.DeleteImage(image);
                existingUser.Image = imagesService.SaveImage(updateUserDto.Image);
            }
            else
            {
                existingUser.Image = image;
            }

            if (updateUserDto.Roles != null && updateUserDto.Roles.Any())
            {
                var existingRoles = await userManager.GetRolesAsync(existingUser);

                var rolesToRemove = existingRoles.Except(updateUserDto.Roles);
                await userManager.RemoveFromRolesAsync(existingUser, rolesToRemove);

                var rolesToAdd = updateUserDto.Roles.Except(existingRoles);
                await userManager.AddToRolesAsync(existingUser, rolesToAdd);

                var permissions = await permissionsRepo.GetAllBySpec(new Permissions.ByUserId(userId));
                foreach (var permission in permissions)
                {
                    await permissionsRepo.Delete(permission);
                    await permissionsRepo.Save();
                }
            }

            await userManager.UpdateAsync(existingUser);
        }

        public async Task Delete(string id)
        {
            var user = await userManager
                .FindByIdAsync(id) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            if (user.Image != "default_user.webp")
                imagesService.DeleteImage(user.Image);

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                string message = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new HttpException(message, HttpStatusCode.BadRequest);
            }
        }

        public async Task<PaginatedList<GetUserDto>> GetByFilter(ItemsFilter itemsFilter)
        {
            var query = userManager.Users;

            if (!string.IsNullOrEmpty(itemsFilter.SearchTerm))
            {
                var searchTermLower = itemsFilter.SearchTerm.ToLower();

                query = query.Where(u =>
                    u.UserName.ToLower().Contains(searchTermLower) ||
                    u.Email.ToLower().Contains(searchTermLower));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / itemsFilter.PageSize);

            var sortingMap = new Dictionary<string, Func<IQueryable<User>, IOrderedQueryable<User>>>
            {
                { "Id", q => itemsFilter.SortDirection == "asc" ? q.OrderBy(user => user.Id) : q.OrderByDescending(user => user.Id) },
                { "Name", q => itemsFilter.SortDirection == "asc" ? q.OrderBy(user => user.UserName) : q.OrderByDescending(user => user.UserName) },
                { "Email", q => itemsFilter.SortDirection == "asc" ? q.OrderBy(user => user.Email) : q.OrderByDescending(user => user.Email) },
                { "DateCreated", q => itemsFilter.SortDirection == "asc" ? q.OrderBy(user => user.DateCreated) : q.OrderByDescending(user => user.DateCreated) },
                { "PhoneNumber", q => itemsFilter.SortDirection == "asc" ? q.OrderBy(user => user.PhoneNumber) : q.OrderByDescending(user => user.PhoneNumber) },
            };

            if (sortingMap.TryGetValue(itemsFilter.SortBy, out var sortingFunction))
                query = sortingFunction(query);

            var skipAmount = (itemsFilter.PageNumber - 1) * itemsFilter.PageSize;
            query = query.Skip(skipAmount).Take(itemsFilter.PageSize);

            var users = await query.ToListAsync();

            var usersWithRoles = new List<GetUserDto>();

            foreach (var user in users)
            {
                var userDto = mapper.Map<GetUserDto>(user);
                var roles = await userManager.GetRolesAsync(user);
                userDto.Roles = [.. roles];

                usersWithRoles.Add(userDto);
            }

            PaginatedList<GetUserDto> userPaginatedList = new()
            {
                TotalPages = totalPages,
                Items = usersWithRoles,
            };

            return userPaginatedList;
        }

        public async Task ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByNameAsync(changePasswordDto.UserName);

            if (user == null || !await userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword)
                || changePasswordDto.NewPassword != changePasswordDto.NewPasswordConfirmation)
                throw new HttpException(ErrorMessages.InvalidPasswordConfirm, HttpStatusCode.BadRequest);

            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                string message = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new HttpException(message, HttpStatusCode.BadRequest);
            }
        }
    }
}