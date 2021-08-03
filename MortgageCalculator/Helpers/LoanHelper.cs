using System;

public class LoanHelper
{
    private decimal CalculateMonthlyRate(decimal loanRate) => loanRate / 1200;
    private decimal CalculateMonthlyInterest( decimal balance, decimal monthlyRate ) => balance * monthlyRate;
    
    public Loan GetPayments(Loan loan)
    {
        
        loan.Payment = CalculateMonthlyPayment(loan.Amount, loan.Rate, loan.Term);
        
        var balance = loan.Amount;
        var totalInterest = 0.0m;
        var monthlyInterest = 0.0m;
        var monthlyPrincipal = 0.0m;
        var monthlyRate = CalculateMonthlyRate(loan.Rate);

        for (int month = 1; month <= loan.Term; month++)
        {
            monthlyInterest = CalculateMonthlyInterest(balance, monthlyRate);
            totalInterest += monthlyInterest;
            monthlyPrincipal = loan.Payment - monthlyInterest;
            balance -= monthlyPrincipal;

            LoanPayment loanPayment = new()
            {
                Month = month,
                Payment = loan.Payment,
                MonthlyPrincipal = monthlyPrincipal,
                MonthlyInterest = monthlyInterest,
                TotalInterest = totalInterest,
                Balance = balance
            };

            loan.Payments.Add( loanPayment );
        }

        loan.TotalInterest = totalInterest;
        loan.TotalCost = loan.Amount + totalInterest;

        return loan;
    }

    private decimal CalculateMonthlyPayment(decimal loanAmount, decimal loanRate, int loanTerm)
    {
        var monthlyRate = CalculateMonthlyRate(loanRate);
        
        var rateDouble = Convert.ToDouble(monthlyRate);
        var amountDouble = Convert.ToDouble(loanAmount);

        var paymentDouble = (amountDouble * rateDouble )/ ( 1-Math.Pow( 1+rateDouble, -loanTerm ));
        
        return Convert.ToDecimal( paymentDouble );
    }


}