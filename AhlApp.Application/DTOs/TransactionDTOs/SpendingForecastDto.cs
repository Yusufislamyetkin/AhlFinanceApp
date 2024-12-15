using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhlApp.Application.DTOs.TransactionDTOs
{
    public class SpendingForecastDto
    {
        public decimal PredictedExpense { get; set; }
        public decimal PredictedIncome { get; set; }
    }
}
