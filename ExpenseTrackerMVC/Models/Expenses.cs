using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTrackerMVC.Models
{
    public class Expenses
    {
        public int expenseId { get; set; }
        public int categoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public virtual Category Category { get; set; }
    }
}