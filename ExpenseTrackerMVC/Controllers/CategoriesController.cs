using ExpenseTrackerMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTrackerMVC.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult AddorEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Category());
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Category>().Result);
            }
        }

        [HttpPost]

        public ActionResult AddorEdit(Category category)
        {
            if (category.categoryId == 0)
            {
                //check validation
                if (isDuplicateCategory(category))
                {
                    if (checkCategoryLimit(category))
                    {
                        //inserting
                        HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories", category).Result;
                        TempData["successMessage"] = "Saved Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["failureMessage"] = "Overflow Budget";
                        return View();
                    }

                }
                else
                {
                    TempData["failureMessage"] = "Category already Exists";
                    return View();
                }

            }
            else
            {
               
                if (checkCategoryLimit(category))
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Categories/" + category.categoryId, category).Result;
                    TempData["successMessage"] = "Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["failureMessage"] = "Overflow Budget";
                    return View();
                }
            }

        }

       

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Categories/" + id.ToString()).Result;
            TempData["successMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        public bool checkCategoryLimit(Category category)
        {
            HttpResponseMessage responseMessage1 = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetExpense").Result;
            List<decimal> limit = responseMessage1.Content.ReadAsAsync<List<decimal>>().Result;
            var limitList = limit.ToList();
            var limitChecker = limitList.ElementAt(0);

            var tot = limitChecker + category.limit;

            HttpResponseMessage responseMessage2 = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetLimit").Result;
            List<decimal> budgetLimit = responseMessage2.Content.ReadAsAsync<List<decimal>>().Result;
            var budgetlimitList = budgetLimit.ToList();
            var budgetlimitChecker = budgetlimitList.ElementAt(0);
            

            if (tot > budgetlimitChecker)
            {
                return false;
            }
            else
            {
                return true;
            }
            //arre ha bol aane ss send kari de ne
        }

        public bool isDuplicateCategory(Category category)
        {
            HttpResponseMessage cat = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/IsDuplicate", category).Result;
            var isDuplicate = cat.Content.ReadAsAsync<bool>().Result;
            if (isDuplicate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult Index()
        {
            IEnumerable<Category> categoriesList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories").Result;
            categoriesList = response.Content.ReadAsAsync<IEnumerable<Category>>().Result;
            return View(categoriesList);
        }
    }
}