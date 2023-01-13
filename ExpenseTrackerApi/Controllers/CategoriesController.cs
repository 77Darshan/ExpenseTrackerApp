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
    public class CategoriesController : ApiController
    {
        private DbExpenseTrackerEntities db = new DbExpenseTrackerEntities();

        // GET: api/Categories
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            

            if (id != category.categoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {

            category.CategoryExpense = 0;
            db.Categories.Add(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.categoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = category.categoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        //UPDATE CATEGORYEXPENSE: api/Categories/UpdateCategoryExpense
        [Route("api/Categories/UpdateCategoryExpense")]
        public IHttpActionResult PutUpdateCategoryExpense(Expenses expense)
        {
            
            var category = db.Categories.FirstOrDefault(c => c.categoryId == expense.categoryId);
            if (category != null)
            {
                //category.CategoryExpense += expense.ExpenseAmount; take the control
                var expenses = db.Expenses.Where(e => e.categoryId == expense.categoryId).Sum(e => e.Amount);
                category.CategoryExpense = expenses;
                db.SaveChanges();
            }
            return Ok(category.CategoryExpense);
        }

        [Route("api/Categories/TotalCategoryLimit/{id}")]
        public IHttpActionResult GetTotalCategoryLimit(int id)
        {
            var cat = db.Categories.Where(c => c.categoryId == id).Select(u => u.limit).ToList();
            return Ok(cat);
        }

        [Route("api/Categories/TotalCategoryExpense/{id}")]
        public IHttpActionResult GetTotalCategoryExpense(int id)
        {
            var cat = db.Categories.Where(c => c.categoryId == id).Select(u => u.CategoryExpense).ToList();
            return Ok(cat);
        }

        //CHECK DUPLICATE CATEGORY: api/Categories/isDuplicate
        [Route("api/Categories/IsDuplicate")]
        public bool PostIsDuplicate(Category category)
        {
            var cat = db.Categories.FirstOrDefault(c => c.Name == category.Name);
            if (cat == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.categoryId == id) > 0;
        }
    }
}