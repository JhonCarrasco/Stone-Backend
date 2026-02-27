using AutoMapper;
using Microsoft.Extensions.Logging;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Repositories.Interface;
using Stone.Services.Interface;

namespace Stone.Services.Implementation
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository saleRepository;
        private readonly IConcertRepository concertRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<SaleService> logger;
        private readonly IMapper mapper;

        public SaleService(
            ISaleRepository saleRepository,
            IConcertRepository concertRepository,
            ICustomerRepository customerRepository,
            ILogger<SaleService> logger,
            IMapper mapper)
        {
            this.saleRepository = saleRepository;
            this.concertRepository = concertRepository;
            this.customerRepository = customerRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(string email, SaleRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                await saleRepository.CreateTransactionAsync();
                Sale entity = mapper.Map<Sale>(request);

                var customer = await customerRepository.GetByEmailAsync(email);

                if (customer is null)
                {
                    throw new InvalidOperationException($"La cuenta {email} no está registrada como cliente.");
                    //Caso de uso: Si el cliente no existe, lo creo
                    //customer = new Customer()
                    //{
                    //    Email = request.Email,
                    //    FullName = request.FullName
                    //};
                    //customer.Id = await customerRepository.AddAsync(customer);
                }

                entity.CustomerId = customer.Id;

                var concert = await concertRepository.GetAsync(request.ConcertId);
                if (concert is null)
                    throw new Exception($"El concierto con el Id {request.ConcertId} no existe.");

                //Se quiere comprar tickets para un concierto que ya empezó.
                if (DateTime.Today > concert.DateEvent)
                    throw new InvalidOperationException(
                        $"No se puede comprar tickets para el concierto {concert.Title} porque ya pasó.");

                //Se quiere comprar tickets para un concierto que ya finalizó
                if (concert.Finalized)
                    throw new InvalidOperationException($"El concierto con id {request.ConcertId} ya finalizó.");

                entity.Total = entity.Quantity
                               * (decimal)concert.UnitPrice;

                await saleRepository.AddAsync(entity);
                await saleRepository.UpdateAsync();

                response.Data = entity.Id;
                response.Success = true;
                logger.LogInformation($"Se creó correctamente la venta para {email}");
            } 
            catch (Exception ex)
            {
                await saleRepository.RollBackAsync();
                response.ErrorMessage = "Error al crear la venta";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<SaleResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<SaleResponseDto>();
            try
            {
                var sale = await saleRepository.GetAsync(id);
                response.Data = mapper.Map<SaleResponseDto>(sale);
                response.Success = response.Data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener la venta";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(SaleByDateSearchDto search, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<SaleResponseDto>>();
            try
            {
                var dateInit = Convert.ToDateTime(search.DateStart);
                var dateEnd = Convert.ToDateTime(search.DateEnd);

                var data = await saleRepository.GetAsync(
                    predicate: s => s.SaleDate >= dateInit && s.SaleDate <= dateEnd,
                    orderBy: x => x.OperationNumber,
                    pagination
                    );

                response.Data = mapper.Map<ICollection<SaleResponseDto>>(data);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al buscar las ventas por fecha.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(string email, string title, PaginationDto pagination)
        {
            var response = new BaseResponseGeneric<ICollection<SaleResponseDto>>();
            try
            {
                var data = await saleRepository.GetAsync(
                    predicate: s => s.Customer.Email == email && s.Concert.Title.Contains(title ?? string.Empty),
                    orderBy: x => x.SaleDate,
                    pagination
                    );

                response.Data = mapper.Map<ICollection<SaleResponseDto>>(data);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al buscar las ventas por titulo.";
                logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<SaleReportResponseDto>>> GetSaleReportAsync(DateTime dateStart, DateTime dateEnd)
        {
            var response = new BaseResponseGeneric<ICollection<SaleReportResponseDto>>();
            try
            {
                // Codigo
                var list = await saleRepository.GetSaleReportAsync(dateStart, dateEnd);
                response.Data = mapper.Map<ICollection<SaleReportResponseDto>>(list);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los datos del reporte";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
