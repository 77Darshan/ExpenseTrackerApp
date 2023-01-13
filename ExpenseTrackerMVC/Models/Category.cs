using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTrackerMVC.Models
{
    public class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Expenses = new HashSet<Expenses>();
        }

        public int categoryId { get; set; }
        public string Name { get; set; }
        public decimal limit { get; set; }
        public decimal CategoryExpense { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expenses> Expenses { get; set; }
    }
}