using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ExpenseTrackerApi.Models;

namespace ExpenseTrackerApi.Controllers
{
    public class ExpensesController : ApiController
    {
        private DbExpenseTrackerEntities db = new DbExpenseTrackerEntities();

        // GET: api/Expenses
        /* public IQueryable<Expenses> GetExpenses1()
         {
             return db.Expenses;
         }
 */

        public IHttpActionResult GetExpense()
        {
            var expenses = db.Expenses.Join(db.Categories, e => e.categoryId, c => c.categoryId, (e, c) => new { e, c })
                .Select(ec => new ExpenseViewModel()
                {
                    expenseId = ec.e.expenseId,
                    CategoryName = ec.c.Name,
                    Title = ec.e.Title,
                    Description = ec.e.Description,
                    Date = ec.e.Date,
                    Amount = ec.e.Amount
                }).ToList();
            return Ok(expenses);
        }

        //GET EXPENSE BY CATEGORY: api/Expenses/ExpenseByCategory
        [Route("api/Expenses/ExpenseByCategory/{id}")]
        public IHttpActionResult GetExpenseByCategory(int id)
        {
            var expenses = db.Expenses.Join(db.Categories, e => e.categoryId, c => c.categoryId, (e, c) => new { e, c })
                .Where(ec => ec.e.categoryId == id)
                .Select(ec => new ExpenseViewModel()
                {
                    expenseId = ec.e.expenseId,
                    CategoryName = ec.c.Name,
                    Title = ec.e.Title,
                    Description = ec.e.Description,
                    Date = ec.e.Date,
                    Amount = ec.e.Amount
                }).ToList();
            return Ok(expenses);
        }

        // GET: api/Expenses/5
        [ResponseType(typeof(Expenses))]
        public IHttpActionResult GetExpenses(int id)
        {
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                return NotFound();
            }

            return Ok(expenses);
        }

        // PUT: api/Expenses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExpenses(int id, Expenses expenses)
        {

            if (expenses.Description == null)
                expenses.Description = "NONE";
            expenses.Date = DateTime.Now;

            if (id != expenses.expenseId)
            {
                return BadRequest();
            }

            db.Entry(expenses).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpensesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Expenses
        [ResponseType(typeof(Expenses))]
        public IHttpActionResult PostExpenses(Expenses expenses)
        {
            if (expenses.Description == null)
                expenses.Description = "NONE";
            expenses.Date = DateTime.Now;

            db.Expenses.Add(expenses);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expenses.expenseId }, expenses);
        }

        // DELETE: api/Expenses/5
        [ResponseType(typeof(Expenses))]
        public IHttpActionResult DeleteExpenses(int id)
        {
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                return NotFound();
            }

            db.Expenses.Remove(expenses);
            db.SaveChanges();

            return Ok(expenses);
        }

        //CATEGORY AMOUNT: api/Expenses/TotalCategoryAmount
        [Route("api/Expenses/TotalCategoryAmount")]
        public decimal PostTotalCategoryAmount(Expenses expense)
        {
            var totCatExp = db.Expenses.Where(e => e.categoryId == expense.categoryId).Sum(e => e.Amount);
            return totCatExp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpensesExists(int id)
        {
            return db.Expenses.Count(e => e.expenseId == id) > 0;
        }
    }
}