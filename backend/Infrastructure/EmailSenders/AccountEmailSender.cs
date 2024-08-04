using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
public class AccountEmailDetail : BaseEmailSenderDetail
{
    public AccountEmailDetail() : base() { }

    public AccountEmailDetail(string email, Guid? r, ValidateUserType _userType)
    {
        toList = new List<string>() { email };

        if (r != null)
        {
            referenceId = r;
        }

        userType = _userType;
    }

    public AccountEmailDetail(string email, Guid? r)
    {
        toList = new List<string>() { email };

        if (r != null)
        {
            referenceId = r;
        }
    }

    public AccountEmailDetail(List<string> emails)
    {
        toList = emails;
    }

    public Guid? referenceId { get; set; }
    public ValidateUserType? userType { get; set; }
}

public enum ValidateUserType
{
    Admin = 0,
    User = 1
}

public class AccountEmailSender : BaseEmailSender
{
    public AccountEmailSender(IConfiguration config, IOptions<MailSettings> mailSettings) : base(config, mailSettings) { }

    public async Task<bool> SubmitResetPasswordEmail(AccountEmailDetail detail)
    {
        try
        {

            //Console.WriteLine("Account Email Sender ");
            var template = new StringBuilder();

            ReadEmailTemplate("ForgotPassword", out template);

            template.Replace("[{ResetID}]", detail.referenceId.ToString());

            return await SubmitEmail(detail.toList, null, null, template);
        }
        catch (Exception ex)
        {
            var debugMessage = ex.Message;
            var debugString = ex.ToString();
            Console.WriteLine("AccountEmailSender Broke");
            Console.WriteLine(debugMessage);
            System.Diagnostics.Debugger.Break();
        }
        return false;
    }

    public async Task<bool> SubmitNewUserValidateEmail(AccountEmailDetail detail)
    {
        try
        {
            var template = new StringBuilder();

            ReadEmailTemplate("ValidateUser", out template);

            // perform substitutions
            template.Replace("[{ValidateID}]", detail.referenceId.ToString());
            // if (detail.userType == ValidateUserType.Admin)
            //     template.Replace("[{ValidateUserEndpoint}]", "admin");
            // else if (detail.userType == ValidateUserType.Institution)
            //     template.Replace("[{ValidateUserEndpoint}]", "institution");

            return await SubmitEmail(detail.toList, null, null, template);
        }
        catch (Exception ex)
        {
            var debugMessage = ex.Message;
            var debugString = ex.ToString();
            System.Diagnostics.Debugger.Break();
        }

        return false;
    }

    // public async Task<bool> SubmitEmailValidatedEmail(AccountEmailDetail detail) {
    //     try {
    //         var template = new StringBuilder();

    //         ReadEmailTemplate("EmailValidated", out template);

    //         return await SubmitEmail(detail.toList, null, null, template);
    //     }
    //     catch (Exception)
    //     {
    //     }

    //     return false;
    // }

    // public async Task<bool> SubmitAdminInvoiceEmail(AccountEmailDetail detail, Address billingAddress, Contact billingContact, string institutionName, List<ContentPurchaseSummary> purchases, double price) {
    //     try {
    //         var template = new StringBuilder();

    //         ReadEmailTemplate("AdminInvoice", out template);

    //         // perform substitutions
    //         template.Replace("[{BillingName}]", billingContact.FirstName + " " + billingContact.LastName);
    //         template.Replace("[{BillingAddress}]", billingAddress.Address1 + " " + billingAddress.Address2 + " " + billingAddress.City + ", " + billingAddress.State + " " + billingAddress.ZipCode);
    //         template.Replace("[{InstitutionName}]", institutionName);
    //         template.Replace("[{Price}]", "Total: $" + price.ToString());

    //         var purchasablesString = "";

    //         foreach (var purchase in purchases)
    //         {
    //             purchasablesString += "<li>";
    //             if (purchase.purchasable == null) purchasablesString += "Topic Extension: \"" + purchase.topicInstitutionContainer.topic.name + "\" extended by " + purchase.extensionDays + " days";
    //             else if (purchase.purchasable.topic != null) purchasablesString += "Topic: " + purchase.purchasable.topic.topicPublished.name;
    //             else purchasablesString += "Collection: " + purchase.purchasable.collection.name;
    //             purchasablesString += "</li>";
    //         }


    //         template.Replace("[{PurchasableInfo}]", "<ul>" + purchasablesString + "</ul>");
    //         return await SubmitEmail(detail.toList, null, null, template);
    //     }
    //     catch (Exception ex)
    //     {
    //         var debugMessage = ex.Message;
    //         var debugString = ex.ToString();
    //         System.Diagnostics.Debugger.Break();
    //     }

    //     return false;
    // }

    // public async Task<bool> SubmitCardPurchaseConfirmationEmail(AccountEmailDetail detail) {
    //     try {
    //         var template = new StringBuilder();

    //         ReadEmailTemplate("CardPurchaseConfirmation", out template);

    //         return await SubmitEmail(detail.toList, null, null, template);
    //     }
    //     catch (Exception)
    //     {
    //     }

    //     return false;
    // }

    // public async Task<bool> SubmitInvoicePurchaseConfirmationEmail(AccountEmailDetail detail) {
    //     try {
    //         var template = new StringBuilder();

    //         ReadEmailTemplate("InvoicePurchaseConfirmation", out template);

    //         return await SubmitEmail(detail.toList, null, null, template);
    //     }
    //     catch (Exception)
    //     {
    //     }

    //     return false;
    // }

    // public async Task<bool> SubmitInvoicePurchaseRequestEmail(AccountEmailDetail detail) {
    //     try {
    //         var template = new StringBuilder();

    //         ReadEmailTemplate("InvoicePurchaseRequest", out template);

    //         return await SubmitEmail(detail.toList, null, null, template);
    //     }
    //     catch (Exception)
    //     {
    //     }

    //     return false;
    // 
    // public string formatMetricGoal(MetricGoal metricGoal)
    // {
    //     var ret = (metricGoal.MetricDirectionEnum == (int)MetricDirection.Increase ? "Increase" : "Decrease") + " " + metricGoal.Name + " from ";
    //     if (metricGoal.MetricTypeEnum == (int)MetricType.Dollar) ret += "$" + metricGoal.Baseline;
    //     else if (metricGoal.MetricTypeEnum == (int)MetricType.Percent) ret += metricGoal.Baseline + "%";
    //     else ret += metricGoal.Baseline;
    //     ret += " to ";
    //     if (metricGoal.MetricTypeEnum == (int)MetricType.Dollar) ret += "$" + metricGoal.Goal;
    //     else if (metricGoal.MetricTypeEnum == (int)MetricType.Percent) ret += metricGoal.Goal + "%";
    //     else ret += metricGoal.Goal;
    //     ret += " by " + metricGoal.ByDate.ToString("MM/dd/yyyy");
    //     return ret;
    // }
}
