using Shop.Application.Interfaces;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class SmsService : ISmsService
    {
        public string apiKey = "";
        public async Task SendVirificationCode(string mobile, string activeCode)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);

            await api.VerifyLookup(mobile, activeCode, "Verfy");
        }
    }
}
