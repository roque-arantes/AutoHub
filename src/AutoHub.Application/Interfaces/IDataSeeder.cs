namespace AutoHub.Application.Interfaces;

public interface IDataSeeder
{
    Task<bool> SeedAsync();
}
