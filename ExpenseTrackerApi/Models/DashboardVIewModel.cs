﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTrackerApi.Models
{
    public class DashboardVIewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ExpenseViewModel> Expenses { get; set; }

        public decimal Limit { get; set; }
        public decimal ExpenseAmt { get; set; }
        public int usage { get; set; }
    }
}