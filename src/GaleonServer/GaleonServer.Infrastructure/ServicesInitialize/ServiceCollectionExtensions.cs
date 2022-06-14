using System.Text;
using GaleonServer.Core;
using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Infrastructure.Gateways;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using IdentityOptions = GaleonServer.Models.Options.IdentityOptions;

namespace GaleonServer.Infrastructure.ServicesInitialize
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity(configuration);
			services.AddDatabase(configuration);
			services.AddGateways();

			return services;
		}

		private static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
		{
			services.ConfigureFromSection<IdentityOptions>(configuration, "Identity");
			services.ConfigureFromSection<EmailGatewayOptions>(configuration, "EmailService");

			var builder = services.AddIdentityCore<User>();
			var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
			identityBuilder.AddEntityFrameworkStores<GaleonContext>();
			identityBuilder.AddSignInManager<SignInManager<User>>();
			
			var tokenKeyParamName = configuration["Identity:TokenKey"];
			var tokenKeyParamBytes = Encoding.UTF8.GetBytes(tokenKeyParamName);
			var key = new SymmetricSecurityKey(tokenKeyParamBytes);
			
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
			opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = key,
					ValidateAudience = false,
					ValidateIssuer = false,
				};
			});
		}

		private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("GaleonServerConnection");

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<GaleonContext>(opt => opt.UseNpgsql(connectionString));

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<GaleonReadonlyContext>(opt => opt.UseNpgsql(connectionString));
		}

		private static void AddGateways(this IServiceCollection services)
		{
			services.AddTransient<IJwtTokenGateway, JwtTokenGateway>();
			services.AddTransient<IGameGateway, GameGateway>();
			services.AddTransient<IGameReadonlyGateway, GameReadonlyGateway>();
			services.AddTransient<IEmailGateway, EmailGateway>();
			services.AddTransient<IUserGateway, UserGateway>();
		}
	}
}