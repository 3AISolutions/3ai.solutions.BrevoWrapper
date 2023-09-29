using Microsoft.Extensions.Logging;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace _3ai.solutions.BrevoWrapper;

public class BrevoClient //: IEmailSender
{
    private readonly ILogger<BrevoClient> _logger;

    public BrevoClient(ILogger<BrevoClient> logger)
    {
        _logger = logger;
    }

    public async Task<bool> ImportContactsAsync(long listId, string fileBody)
    {
        try
        {
            ContactsApi trans = new();
            RequestContactImport importRQ = new()
            {
                ListIds = new() { listId },
                FileBody = fileBody
            };
            await trans.ImportContactsAsync(importRQ);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import Contacts Error");
        }
        return false;
    }

    public async Task<bool> SendEmailByTemplateAsync(string email, long templateId, bool skipUserCheck = true, string? bccAddress = null, Dictionary<string, string>? attributes = null)
    {
        try
        {
            var isValidUser = skipUserCheck || await CheckUserAsync(email);
            if (!isValidUser)
            {
                _logger.LogError("User not found or blacklisted: {email}", email);
            }
            else
            {
                TransactionalEmailsApi trans = new();
                SendSmtpEmail msg = new()
                {
                    To = new() { new(email) },
                    TemplateId = templateId,
                    Params = attributes
                };
                if (!string.IsNullOrEmpty(bccAddress))
                    msg.Bcc = new() { new(bccAddress) };
                await trans.SendTransacEmailAsync(msg);
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Send Email By Template Error");
        }
        return false;
    }

    private async Task<bool> CheckUserAsync(string emailAddress)
    {
        try
        {
            ContactsApi contactsAPI = new();
            var resp = await contactsAPI.GetContactInfoAsync(emailAddress);
            return emailAddress == resp.Email && !(resp.EmailBlacklisted ?? false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user");
            return false;
        }
    }
}
