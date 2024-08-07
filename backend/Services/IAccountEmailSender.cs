namespace backend.Services
{
    public interface IAccountEmailSender
    {
        Task<bool> SubmitResetPasswordEmail(AccountEmailDetail detail);

        Task<bool> SubmitNewUserValidateEmail(AccountEmailDetail detail);

        // Uncomment and add other method signatures as needed
        // Task<bool> SubmitEmailValidatedEmail(AccountEmailDetail detail);

        // Task<bool> SubmitAdminInvoiceEmail(AccountEmailDetail detail, Address billingAddress, Contact billingContact, string institutionName, List<ContentPurchaseSummary> purchases, double price);

        // Task<bool> SubmitCardPurchaseConfirmationEmail(AccountEmailDetail detail);

        // Task<bool> SubmitInvoicePurchaseConfirmationEmail(AccountEmailDetail detail);

        // Task<bool> SubmitInvoicePurchaseRequestEmail(AccountEmailDetail detail);

        // string formatMetricGoal(MetricGoal metricGoal);
    }
}

