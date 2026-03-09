using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stone.Dto.Request;
using Stone.Dto.Response;
using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Interface;
using Stone.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Stone.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<UserService> logger;
        private readonly IOptions<AppSettings> options;
        private readonly ICustomerRepository customerRepository;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;
        private readonly IPersonService personService;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger,
            IOptions<AppSettings> options, ICustomerRepository customerRepository,
            SignInManager<User> signInManager
            , IEmailService emailService, IPersonService personService
            )
        {
            this.userManager = userManager;
            this.logger = logger;
            this.options = options;
            this.customerRepository = customerRepository;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.personService = personService;
        }

        public async Task<BaseResponseGeneric<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var response = new BaseResponseGeneric<RegisterResponseDto>();
            try
            {
                var person = new Person
                {
                    Rut = request.Rut,
                    DisplayName = request.DisplayName
                };

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email,                    
                    FirstName = request.DisplayName,
                    EmailConfirmed = true
                };
                var responsePerson = await personService.AddAsync(person);
                if (responsePerson.Success)
                {
                    logger.LogWarning(response.ErrorMessage); //:TODO - Definir mesaje "Fallo al guardar persona"
                }

                user.PersonId = responsePerson.Data;
                var resultado = await userManager.CreateAsync(user, request.ConfirmPassword);

                if (resultado.Succeeded)
                {
                    user = await userManager.FindByEmailAsync(request.Email);

                    if (user is not null)
                    {
                        await userManager.AddToRoleAsync(user, Constants.RoleCustomer);

                        var customer = new Customer()
                        {
                            Email = request.Email,
                            DisplayName = request.DisplayName
                        };

                        await customerRepository.AddAsync(customer);

                        // TODO: Enviar un email

                        response.Success = true;

                        var tokenResponse = await ConstruirToken(user);//returning jwt
                        response.Data = new RegisterResponseDto
                        {
                            UserId = user.Id,
                            Token = tokenResponse.Token,
                            ExpirationDate = tokenResponse.ExpirationDate
                        };
                    }
                }
                else
                {
                    //response.Success = false;
                    response.ErrorMessage = String.Join(" ", resultado.Errors.Select(x => x.Description).ToArray());
                    logger.LogWarning(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al registrar el usuario.";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var response = new BaseResponseGeneric<LoginResponseDto>();
            try
            {
                var resultado = await signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(request.Username);
                    response.Success = true;
                    response.Data = await ConstruirToken(user);
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "Credenciales incorrectas.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error.";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        private async Task<LoginResponseDto> ConstruirToken(User user)
        {
            //creamos los claims, que son informaciones emitidas por una fuente confiable, pueden contener cualquier key/value que definamos y que son añadidas al TOKEN
            var claims = new List<Claim>()
           {
               new Claim(ClaimTypes.Email,user.Email), //Nunca enviar data sensible en un claim, ya que es leído por el cliente
               new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
           };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //firmando el JWT
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Jwt.JWTKey)); //nos valemos del proveedor de configuracion appsettings.Development.json para guardar una llaveJWT
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddSeconds(options.Value.Jwt.LifetimeInSeconds);//se puede configurar cualquier espacio de tiempo de validez de un token según las reglas de negocio

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, signingCredentials: credenciales, expires: expiracion);
            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                ExpirationDate = expiracion
            };
        }

        public async Task<BaseResponse> RequestTokenToResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(userIdentity);

                // Enviar un email con el token para reestablecer la contraseña
                await emailService.SendEmailAsync(request.Email, "Reestablecer clave",
                    @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Para reestablecer su clave, por favor copie el siguiente codigo</p>
                    <p> <strong> {token} </strong> </p>
                    <hr />
                    Atte. <br />
                    Music Store © 2025
                ");

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al solicitar el token para resetear la clave";
                logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> ResetPasswordAsync(NewPasswordRequestDto request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result = await userManager.ResetPasswordAsync(userIdentity, request.Token, request.ConfirmNewPassword);
                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    response.ErrorMessage = String.Join(" ", result.Errors.Select(x => x.Description).ToArray());
                }
                else
                {
                    // Enviar un email de confirmacion de clave cambiada
                    await emailService.SendEmailAsync(request.Email, "Confirmacion de cambio de clave",
                    @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Music Store © 2024");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al resetear la clave";
                logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequestDto request)
        {
            var response = new BaseResponse();

            try
            {
                var userIdentity = await userManager.FindByEmailAsync(email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result = await userManager.ChangePasswordAsync(userIdentity, request.OldPassword, request.NewPassword);

                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    response.ErrorMessage = String.Join(" ", result.Errors.Select(x => x.Description).ToArray());
                }
                else
                {
                    logger.LogInformation("Se cambio la clave para {email}", userIdentity.Email);

                    // Enviar un email de confirmacion de clave cambiada
                    await emailService.SendEmailAsync(email, "Confirmacion de cambio de clave",
                    @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Music Store © 2024");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al cambiar la clave";
                logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
