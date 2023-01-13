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
    public class BudgetsController : ApiController
    {
        private DbExpenseTrackerEntities db = new DbExpenseTrackerEntities();

        // GET: api/Budgets
        public IQueryable<Budget> GetBudgets()
        {
            return db.Budget;
        }

        //GET: api/Budgets/BudgetLimit
        [Route("api/Budgets/BudgetLimit")]
        public IHttpActionResult GetBudgetLimit()
        {
            var budget = db.Budget.Select(u => u.TotalLimit).ToList();
            return Ok(budget);
        }

        //GET: api/Budgets/BudgetExpense
        [Route("api/Budgets/BudgetExpense")]
        public IHttpActionResult GetBudgetExpense()
        {
            var budget = db.Budget.Select(u => u.TotalExpense).ToList();
            return Ok(budget);
        }

        //SEARCH BUDGET: api/Budgets/FindBudget
        [Route("api/Budgets/FindBudget")]
        public bool GetFindBudget()
        {
            Budget budget = db.Budget.Find(1);
            if (budget == null)
                return false;
            else
                return true;
        }

        //UPDATE TOTALEXPENSE: api/Budgets/UpdateBudgetTotalExpense
        [Route("api/Budgets/UpdateBudgetTotalExpense/{id}")]
        public IHttpActionResult GetUpdateBudgetTotalExpense(int id)
        {
            var budget = db.Budget.FirstOrDefault(b => b.budgetid == id);
            budget.TotalExpense = db.Categories.Sum(c => c.CategoryExpense);
            db.SaveChanges();
            return Ok(budget.TotalExpense);
        }
        // GET: api/Budgets/5
        [ResponseType(typeof(Budget))]
        public IHttpActionResult GetBudgets(int id)
        {
            Budget budget = db.Budget.Find(id);
            if (budget == null)
            {
                return NotFound();
            }

            return Ok(budget);
        }

        // PUT: api/Budgets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBudgets(int id, Budget budget)
        {
            

            if (id != budget.budgetid)
            {
                return BadRequest();
            }

            db.Entry(budget).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
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

        // POST: api/Budgets
        [ResponseType(typeof(Budget))]
        public IHttpActionResult PostBudgets(Budget budget)
        {
            budget.budgetid = 1;
           

            db.Budget.Add(budget);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BudgetExists(budget.budgetid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = budget.budgetid }, budget);
        }

        // DELETE: api/Budgets/5
        [ResponseType(typeof(Budget))]
        public IHttpActionResult DeleteBudget(int id)
        {
            Budget budget = db.Budget.Find(id);
            if (budget == null)
            {
                return NotFound();
            }

            db.Budget.Remove(budget);
            db.SaveChanges();

            return Ok(budget);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BudgetExists(int id)
        {
            return db.Budget.Count(e => e.budgetid == id) > 0;
        }
    }
}