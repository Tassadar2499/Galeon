namespace GaleonServer.Core.Models
{
	public interface IEntityBase<T>
	{
		public T Id { get; set; }
	}
}