using ATM.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATM;

public class ATMLogic
{
    private static string Number { get; set; }
    private bool IsAuth { get; set; }

    public decimal TotalAmount { get; set; } = 10000;

    private const int LimitVisa = 200;
    private const int LimitMasterCard = 300;

    private static readonly IReadOnlyCollection<Card> cards = new List<Card>
{
    new ("4444333322221111", "Troy Mcfarland","edyDfd5A", CardBrands.Visa, 800),
    new ("5200000000001005", "Levi Downs", "teEAxnqg", CardBrands.MasterCard, 400)
};

    private static readonly IReadOnlyCollection<CardBrandLimit> WithdrawLimits = new List<CardBrandLimit>
{
    new (CardBrands.Visa, LimitVisa),
    new (CardBrands.MasterCard, LimitMasterCard)
};
    public sealed record CardBrandLimit(
    CardBrands CardBrand,
    decimal Amount);

    private readonly LogLogic _logLogic;
    public ATMLogic(LogLogic logLogic)
    {
        _logLogic = logLogic;
    }
    public bool Auth()
    {
        _logLogic.LogMethodName();
        return (IsAuth && Number != null);
    }

    public bool InitATM(string number)
    {
        _logLogic.LogMethodName();
        if (GetCard(number) != null)
        {
            Number = number;
            return true;
        }
            return false;
    }
    public bool Authorize(string pin)
    {
        _logLogic.LogMethodName();
        if (Number == null)
        {
            throw new ArgumentException("Balance is lower than amount");
        }
        var authorizedCard = cards.FirstOrDefault(c =>
            c.Number == Number && c.Pin == pin);

        if (authorizedCard != null)
        {
            IsAuth = true;
            return true;
        }

        throw new ArgumentException("Balance is lower than amount");
    }
    public bool Withdraw(decimal amount)
    {
        _logLogic.LogMethodName();
        if (!Auth())
        {
            throw new ArgumentException("Balance is lower than amount");
        }

        var authCard = GetCard(Number);

        if (authCard.Balance >= amount)
        {
            CheckLimit(amount, authCard.Brand);
            authCard.Balance -= amount;
            return true;
        }
        else
        {
            throw new ArgumentException("Balance is lower than amount");
        }
    }


    public decimal GetBalance()
    {
        _logLogic.LogMethodName();
        if (!Auth())
        {
            throw new ArgumentException("Balance is lower than amount");
        }
        return GetCard(Number).Balance;
    }

    public bool Transfer(TransferRequest transferRequest)
    {
        _logLogic.LogMethodName();
        var sourceCard = GetCard(Number);
        var destinationCard = GetCard(transferRequest.destinationNumber);

        if (sourceCard.Balance < transferRequest.Amount)
        {
            throw new ArgumentException("Balance is lower than amount");
        }
        sourceCard.Balance -= transferRequest.Amount;
        destinationCard.Balance += transferRequest.Amount;

        return true;
    }

    private void CheckLimit(decimal amount, CardBrands cardBrand)
    {
        decimal limit = 0;
        if(cardBrand == CardBrands.MasterCard)
        {
            limit = 300;
        }
        if(cardBrand == CardBrands.Visa)
        {
            limit = 200;
        }

        if (cardBrand == CardBrands.MasterCard && amount > 300)
        {
            throw new InvalidOperationException($"One time {cardBrand} withdraw limit is {limit}");
        }
        if (cardBrand == CardBrands.Visa && amount > 200)
        {
            throw new InvalidOperationException($"One time {cardBrand} withdraw limit is {limit}");
        }
    }
    private Card GetCard(string number)
    {
         return cards.FirstOrDefault(x => x.Number == number);
    }
}