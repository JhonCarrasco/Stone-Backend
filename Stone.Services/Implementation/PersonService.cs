using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository repository;
        private readonly ILogger<PersonService> logger;
        private readonly IMapper mapper;

        public PersonService(IPersonRepository repository, ILogger<PersonService> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(Person person)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                response.Data = await repository.AddAsync(person);
                response.Success = true;

                //TODO: borrar - solo usado para poblar
                //// Agregando 20 personas (Nombres y RUTs ficticios)
                //Dictionary<string, string> personas = new Dictionary<string, string>()
                //{
                //    { "17786044-1", "Giovanni Donoso" },
                //    { "16887941-5", "Eric Aguilera" },
                //    { "6667346-4", "Fidel Montecinos" },
                //    { "12870631-3", "Ismael Santibañez" },
                //    { "24156862-8", "Isaias Varas" },
                //    { "12983018-2", "Christian Valdes" },
                //    { "24882585-5", "Eduardo Ortega" },
                //    { "15518258-k", "Arturo Bustamante" },
                //    { "6629779-9", "Bernardo Venegas" },
                //    { "21115084-k", "Camilo Gutierrez" },
                //    { "18504907-8", "Benito Ramirez" },
                //    { "25691171-k", "Segundo Torres" },
                //    { "36386631-k", "Daniel Moreno" },
                //    { "46892737-3", "Ramon Avendaño" },
                //    { "9416571-7", "Augusto Saavedra" },
                //    { "16582135-1", "Benito Montecinos" },
                //    { "29839564-9", "Dagoberto Zamora" },
                //    { "34503224-k", "Cesar Fernandez" },
                //    { "49696791-7", "Salvador Mardones" },
                //    { "8879036-7", "Miguel Riveros" }
                //};

                //foreach (var p in personas)
                //{
                //    await repository.AddAsync(new Person
                //    {
                //        Rut = p.Key,
                //        DisplayName = p.Value,
                //        TypePerson = 1,//persona natural
                //        CreateAt = DateTime.UtcNow,
                //        UpdatedAt = null,
                //        Active = true
                //    });
                //};                ;
                //response.Data = 9999;
                //response.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
