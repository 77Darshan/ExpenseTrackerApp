//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseTrackerApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Budget
    {
        public int budgetid { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal TotalExpense { get; set; }
    }
}