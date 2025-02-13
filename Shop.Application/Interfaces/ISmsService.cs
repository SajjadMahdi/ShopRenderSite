using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface ISmsService
    {
        Task SendVirificationCode(string mobile, string activeCode);
    }
}
