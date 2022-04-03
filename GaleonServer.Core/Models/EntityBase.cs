namespace GaleonServer.Core.Models
{
	public class EntityBase<T>
	{
		public T Id { get; set; } = default!;
	}
}