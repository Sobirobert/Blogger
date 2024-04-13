using Application.Interfaces;
using Domain.Enums;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Application.Services.Emails;

public class EmailSenderService : IEmailSenderService
{
    private readonly IFluentEmail _email;
    private readonly ILogger _logger;
    private const string TemplatePath = "Application.Services.Emails.Templates.{0}.cshtml";

    public EmailSenderService(IFluentEmail email, ILogger<EmailSenderService> logger)
    {
        _email = email;
        _logger = logger;
    }

    public async Task<bool> Send(string to, string subject, EmailTemplate template, object model)
    {
        var result = await _email.To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(string.Format(TemplatePath, template), ToExpando(model), GetType().Assembly)
            .SendAsync();

        if (!result.Successful)
        {
            _logger.LogError("Faild to send email. \n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));
        }

        return result.Successful;
    }

    #region Helper methods;

    private static ExpandoObject ToExpando(object model)
    {
        if (model is ExpandoObject exp)
        {
            return exp;
        }

        IDictionary<string, object?> expando = new ExpandoObject();
        foreach (var propertyDescriptor in model.GetType().GetTypeInfo().GetProperties())
        {
            var obj = propertyDescriptor.GetValue(model);

            if (obj != null && IsAnonymousType(obj.GetType()))
            {
                obj = ToExpando(obj);
            }

            expando.Add(propertyDescriptor.Name, obj);
        }

        return (ExpandoObject)expando;
    }

    private static bool IsAnonymousType(Type type)
    {
        bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
            .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
            .Any();

        bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
        bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

        return isAnonymousType;
    }

    #endregion Helper methods;
}