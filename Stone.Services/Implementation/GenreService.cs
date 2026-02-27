using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<IGenreService> logger;
        private readonly IMapper mapper;

        public GenreService(IGenreRepository genreRepository, ILogger<IGenreService> logger, IMapper mapper)
        {
            this.genreRepository = genreRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<ICollection<GenreResponseDto>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<GenreResponseDto>>();
            try
            {
                response.Data = mapper.Map<ICollection<GenreResponseDto>>(await genreRepository.GetAsync());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<GenreResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<GenreResponseDto>();
            try
            {
                var data = await genreRepository.GetAsync(id);
                response.Data = mapper.Map<GenreResponseDto>(data);
                response.Success = response.Data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(GenreRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var entity = mapper.Map<Genre>(request);
                response.Data = await genreRepository.AddAsync(entity); ;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al guardar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }
        public async Task<BaseResponse> UpdateAsync(int id, GenreRequestDto request)
        {
            var response = new BaseResponse();
            try
            {
                var data = await genreRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                }
                var entity = mapper.Map(request, data); //sobre escribir data nueva al objeto obtenido en la db
                await genreRepository.UpdateAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar la información.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await genreRepository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No se ha encontrado registro con el id {id}";
                    return response;
                }

                await genreRepository.DeleteAsync(id);
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
