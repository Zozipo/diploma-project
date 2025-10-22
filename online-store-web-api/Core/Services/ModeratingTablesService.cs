using System.Net;
using Core.Entities;
using Core.Interfaces;
using Core.Models.ModeratingTable;
using Core.Resources;
using AutoMapper;
using Core.Helpers;
using Core.Specifications;
using Core.Models.Category;

namespace Core.Services
{
    public class ModeratingTableService(IRepository<ModeratingTable> moderatingTableRepo,
                                        IImagesService imageService,
                                        IMapper mapper) : IModeratingTableService
    {
        public async Task<IEnumerable<GetModeratingTableDto>> GetAll()
        {
            var moderatingTables = await moderatingTableRepo.GetAllBySpec(new ModeratingTables.All());
            return mapper.Map<IEnumerable<GetModeratingTableDto>>(moderatingTables);
        }

        public async Task<GetModeratingTableDto> GetById(int id)
        {
            var moderatingTable = await moderatingTableRepo.GetBySpec(new ModeratingTables.ById(id)) ??
                                  throw new HttpException(ErrorMessages.ModeratingTableByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetModeratingTableDto>(moderatingTable);
        }

        public async Task<GetModeratingTableDto> GetByName(string moderatingTableName)
        {
            var moderatingTable = await moderatingTableRepo.GetBySpec(new ModeratingTables.ByName(moderatingTableName)) ??
                                  throw new HttpException(ErrorMessages.ModeratingTableByNameNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetModeratingTableDto>(moderatingTable);
        }

        public async Task Create(CreateModeratingTableDto createModeratingTableDto)
        {
            var moderatingTable = mapper.Map<ModeratingTable>(createModeratingTableDto);

            moderatingTable.Image = imageService.SaveImage(createModeratingTableDto.Image);

            await moderatingTableRepo.Insert(moderatingTable);
            await moderatingTableRepo.Save();
        }

        public async Task Update(int id, UpdateModeratingTableDto updateModeratingTableDto)
        {
            var existingModeratingTable = await moderatingTableRepo.GetByID(id) ??
                                throw new HttpException(ErrorMessages.ModeratingTableByIdNotFound, HttpStatusCode.NotFound);

            var image = existingModeratingTable.Image;

            mapper.Map(updateModeratingTableDto, existingModeratingTable);
            existingModeratingTable.Id = id;

            if (updateModeratingTableDto.Image != null)
            {
                imageService.DeleteImage(image);
                existingModeratingTable.Image = imageService.SaveImage(updateModeratingTableDto.Image);
            }
            else
            {
                existingModeratingTable.Image = image;
            }

            await moderatingTableRepo.Update(existingModeratingTable);
            await moderatingTableRepo.Save();
        }

        public async Task Delete(int id)
        {
            var moderatingTable = await moderatingTableRepo.GetByID(id) ??
                throw new HttpException(ErrorMessages.ModeratingTableByIdNotFound, HttpStatusCode.NotFound);

            imageService.DeleteImage(moderatingTable.Image);

            await moderatingTableRepo.Delete(id);
            await moderatingTableRepo.Save();
        }
    }
}
