using PartnerPortal.Domain.Entities.Models;

namespace PartnerPortal.WebApi.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
