namespace DataMapper
{
    public interface IMapper<T>
    {
        T GetByID(int id);
        T GetAll();
        void Save(T t);
        void Delete(T t);
    }
}
