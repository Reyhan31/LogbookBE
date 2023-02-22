namespace LogBookAPI.Models
{
    public class Constant
    {
        public const int Length255 = 255;
        public const int Length50 = 50;
        public const int SqlServerDuplicateCode = 2601;
        public const int TokenExpiryDate = 7;

        public static readonly string DefaultStatusText = "Draft";
        public static readonly string Submitted = "Submitted";
        public static readonly string Review = "Review";
        public static readonly string Reject = "Reject";
        public static readonly string Revise = "Revise";
        public static readonly string Approve = "Approved";
    }
}