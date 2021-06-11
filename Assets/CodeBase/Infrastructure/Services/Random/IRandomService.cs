namespace CodeBase.Infrastructure.Services.Random
{
    public interface IRandomService : IService
    {
        int Next(int min, int max);
    }
}