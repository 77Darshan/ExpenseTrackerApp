using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTrackerApi.Models
{
    public class ExpenseViewModel
    {
        public int expenseId { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Amount { get; set; }

    }
}