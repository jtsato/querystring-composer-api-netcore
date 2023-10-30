namespace Core.Commons;

public interface IServiceResolver
{
    public T Resolve<T>();
}