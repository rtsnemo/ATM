namespace ATM
{
    public class Card
    {
        public string Number { get; set; }
        public string HolderName { get; set; }
        public string Pin { get; set; }
        public CardBrands Brand { get; set; }
        public decimal Balance { get; set; }

        public Card(string number, string holderName, string pin, CardBrands brand, decimal balance)
        {
            Number = number;
            HolderName = holderName;
            Pin = pin;
            Brand = brand;
            Balance = balance;
        }
    }

    public enum CardBrands
    {
        Visa,
        MasterCard
    }
}
