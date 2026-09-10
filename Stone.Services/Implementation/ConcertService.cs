using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;
using System.ComponentModel;
using System.Xml.Linq;

namespace Stone.Services.Implementation
{
    public class ConcertService : IConcertService
    {
        private readonly IConcertRepository repository;
        private readonly ILogger<ConcertService> logger;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "concerts";

        public ConcertService(IConcertRepository repository, ILogger<ConcertService> logger, IMapper mapper, IFileStorage fileStorage)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        
        public async Task<BaseResponseGeneric<ICollection<ConcertResponseDto>>> GetAsync(string? title, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<ConcertResponseDto>>();
            try
            {
                var data = await repository.GetAsync(title, pagination);
                response.Data = mapper.Map<ICollection<ConcertResponseDto>>(data);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obteenr la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ConcertResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<ConcertResponseDto>();
            try
            {
                var data = await repository.GetAsync(id);
                response.Data = mapper.Map<ConcertResponseDto>(data);
                response.Success = true; // data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ConcertRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            Concert entity = new();
            try
            {
                entity = mapper.Map<Concert>(request);
                if (request.Image is not null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.Image.CopyToAsync(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(request.Image.FileName);
                        entity.ImageUrl = await fileStorage.SaveFile(content,
                            extension, container, request.Image.ContentType);
                    }
                }
                response.Data = await repository.AddAsync(entity); ;
                response.Success = true;
            }
            catch (Exception ex)
            {
                await fileStorage.DeleteFile(entity.ImageUrl ?? string.Empty, container);//si ocurre algún error borrar la imagen (si existe)
                response.ErrorMessage = "Ocurrió un error al añadir la información.";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, ConcertRequestDto request)
        {
            var response = new BaseResponse();
            try
            {
                var data = await repository.GetAsync(id);
                if(data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                mapper.Map(request, data); //sobre escribir data nueva al objeto obtenido en la db
                if (request.Image is not null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.Image.CopyToAsync(memoryStream);
                        var content = memoryStream.ToArray();
                        var extension = Path.GetExtension(request.Image.FileName);
                        data.ImageUrl = await fileStorage.EditFile(content,
                            extension, container, data.ImageUrl ?? string.Empty,
                            request.Image.ContentType);
                    }
                }
                else
                {
                    data.ImageUrl = string.Empty;
                }

                await repository.UpdateAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar.";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await repository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"El registro con id {id} no fue encontrado";
                    return response;
                }

                await repository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> FinalizeAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                await repository.FinalizeAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
    }
}
