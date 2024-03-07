namespace ATM.Requests
{
    public class TransferRequest
    {
        public string destinationNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
