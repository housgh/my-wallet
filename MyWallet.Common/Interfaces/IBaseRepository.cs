namespace MyWallet.Common.Interfaces;

public interface IBaseRepository<TDto>
{
    Task<TDto> GetAsync(int id);
    Task AddAsync(TDto dto);
    void Update(TDto dto);
    void Delete(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
