using System.Threading.Tasks;

namespace mySimpleMessageService.Domain
{
    public interface IValidator<T>
    {
        string ValidatorName { get; }
        Task<bool> IsValid(T obj);
    }
}
