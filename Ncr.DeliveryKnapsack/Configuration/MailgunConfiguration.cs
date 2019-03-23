namespace Ncr.DeliveryKnapsack.Configuration
{
    public class MailgunConfiguration
    {
        public string Key { get; set; }

        public string ReceiptTo { get; set; }

        public string WelcomeReplyTo { get; set; }

        public string Server { get; set; } = "mg.ncrlab.cz";

        public string Sender { get; set; } = "NCR Prague <mailer@mg.ncrlab.cz>";

        public string BccLogger { get; set; } = "ncrcontestlogger@gmail.com";

        public string SolutionAcceptedToBcc { get; set; }
    }
}
