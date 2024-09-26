namespace GorBilet_example.Data
{
    public interface IData<T> where T : class
    {
        Task<T?> Create(T post);

        Task<IEnumerable<T?>?> GetAll();

        Task<T?> GetById(dynamic id);

        Task<T?> Update(T post);

        Task<bool?> Delete(dynamic id);
    }
}
