using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTrackerMVC.Models
{
    public class Budget
    {
        public int budgetid { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal TotalExpense { get; set; }
    }
}