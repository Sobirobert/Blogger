using Domain.Enums;

namespace Application.Interfaces;

public interface IEmailSenderService
{
    Task<bool> (string to, string subject, EmailTemplate template, object model);
}
