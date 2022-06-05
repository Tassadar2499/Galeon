using Microsoft.EntityFrameworkCore;

namespace GaleonServer.Infrastructure.Database
{
	//TODO: Будет ссылаться на readonly реплику
	public class GaleonReadonlyContext : GaleonContext
	{
		public GaleonReadonlyContext(DbContextOptions<GaleonReadonlyContext> options) : base(options)
		{
		}
	}
}