using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MailKit;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Microsoft.Extensions.Options;


    public abstract class BaseEmailSenderDetail
    {

        public List<string> toList { get; set; }

        public BaseEmailSenderDetail()
        {
            toList = new List<string>();
        }
    }

    public abstract class BaseEmailSender
    {

        protected IConfiguration _config { get; set; }
        private readonly MailSettings _mailSettings;

        public BaseEmailSender(IConfiguration config, IOptions<MailSettings> mailSettings)
        {
            _config = config;
            _mailSettings = mailSettings.Value;
        }

        protected async Task<bool> SubmitEmail(IList<string> toList, IList<string>? ccList, IList<string>? bccList, StringBuilder template)
        {
        if (_mailSettings.Port == null)
        {
            throw new Exception("Malconfigured Mail Settings");
        }
            var ret = true;

            if ((toList == null) || !toList.Any(e => e.Any()))
            {
                return false;
            }


            foreach (var address in toList.Select(e => e.ToLower()).Distinct())
            {

                var message = new MimeMessage();
                if (!address.Any())
                {
                    continue;
                }

                message.To.Add(new MailboxAddress(address, address));
                message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.FromEmail));

                message.Subject = GetSubject(template);
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = template.ToString()
                };
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    try
                    {
                        smtp.Connect(_mailSettings.Host, int.Parse(_mailSettings.Port), SecureSocketOptions.StartTls);
                        smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        await smtp.SendAsync(message);
                        smtp.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        var exmessage = ex.Message;
                        var eString = ex.ToString();
                        ret = false;

                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
            return ret;
        }


        protected bool ReadEmailTemplate(string emailType, out StringBuilder template)
        {
            try
            {
                template = new StringBuilder(GetTemplate("EmailWrapper"));
                template.Replace("[{EmailBody}]", GetTemplate(emailType));

                PerformURLSubstitutions(template);
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
                throw;
            }
            return true;
        }

        protected string GetTemplate(string templateFileName)
        {
            try
            {
                var assembly = typeof(BaseEmailSender).GetTypeInfo().Assembly;
                // string[] resourceNames = assembly.GetManifestResourceNames();
                // foreach (string resourceName in resourceNames)
                // {
                //     Console.WriteLine("Resource:");
                //     Console.WriteLine(resourceName);
                // }

                using (var resource = assembly.GetManifestResourceStream($"Infrastructure.EmailSenders.Templates.{templateFileName}.html"))
                {
                    if (resource != null)
                    {
                        using (var reader = new StreamReader(resource, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        throw new Exception("Resource not found");
                    }

                }
            }
            catch (Exception ex)
            {
                var debugMesssage = ex.Message;
                var debugString = ex.ToString();
                Console.WriteLine(debugMesssage);
                Console.WriteLine(debugString);

                return "";
            }
        }

        protected void PerformURLSubstitutions(StringBuilder template)
        {
            //template.Replace("[{ApiUrl}]", _config["ApiUrl"]);
            //template.Replace("[{ClientUrl}]", _config["ClientUrl"]);
            template.Replace("[{SupportEmail}]", _config["MailSettings:SupportEmail"]);
            //template.Replace("[{OverviewVideo}]", InfrastructureSettings.Instance.MailSettings.OverviewVideo);
            // template.Replace("[{SRSWebsiteUrl}]", InfrastructureSettings.Instance.MailSettings.SRSWebsiteUrl);
            // template.Replace("[{InvoiceToolUrl}]", InfrastructureSettings.Instance.MailSettings.InvoiceToolUrl);
        }

        protected string GetSubject(StringBuilder template)
        {
            string pattern = Regex.Escape("<subject>") + "(.*?)" + Regex.Escape("</subject>");
            var matching = Regex.Match(template.ToString(), pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (matching.Success)
            {
                return matching.Groups[1].ToString();
            }

            return "Coordinatus Email";
        }
    }
