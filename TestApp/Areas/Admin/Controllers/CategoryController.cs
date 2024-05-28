using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        public readonly CategoryService categoryService;

        private readonly IMapper mapper;

        public CategoryController() { 

            categoryService = new CategoryService();
            mapper = AutoMapperConfig.Mapper;
        }


        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = categoryService.ReadAll();


            var categoriesList = mapper.Map<List<CategoryModel>>(categories);


            /* foreach (var item in categories) {
                categoriesList.Add(new CategoryModel
                {
                    Id = item.ID,
                    Name = item.Name,
                    ParentName = item.Category2?.Name
                });
            }*/
            return View(categoriesList);
        }


        public ActionResult Create()
        {
            var categoryModel = new CategoryModel();

            InitMainCategories(null, ref categoryModel);

            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult Create(CategoryModel data)
        {

               var newCategroy = mapper.Map<Category>(data);
               newCategroy.Category2 = null;

                int creationResult = categoryService.Create(newCategroy);

                if (creationResult == -2)
                {
                InitMainCategories(null, ref data);


                ViewBag.Message = "Category name already exist!";
                    return View(data);
                }

                return RedirectToAction("Index");
            }


        public ActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return RedirectToAction("index", "Home");
            }

            var currentCategory = categoryService.ReadById(id.Value);

            if (currentCategory == null)
            {
                return HttpNotFound($"This category ({id}) not found");
            }

            var categoryModel = mapper.Map<CategoryModel>(currentCategory);

            /*var categoryModel = new CategoryModel
            {
                Id = currentCategory.ID,
                Name = currentCategory.Name,
                ParentId = currentCategory.Parent_Id
            };*/

            InitMainCategories(currentCategory.ID, ref categoryModel);

            return View(categoryModel);


        }


        [HttpPost]
        public ActionResult Edit(CategoryModel data)
        {


            var updatedCategory = mapper.Map<Category>(data);
            updatedCategory.Category2 = null;

            var result = categoryService.Update(updatedCategory);


                if (result == -2)
                {

                    ViewBag.Message = "Category name already exist!";
                     InitMainCategories(data.Id, ref data);
                     return View(data);
                }

                else if (result > 0)
                {
                    ViewBag.Success = true;
                    ViewBag.Message = $"Category {data.Name} updated successfully.";
                }
                else
                    ViewBag.Message = $"An error occurred!";

            InitMainCategories(data.Id, ref data);
            return View(data);

        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int? Id)
        {
 
                var deleted = categoryService.Delete(Id.Value);

            if (deleted)
            {
                return RedirectToAction("index", "Category");
            }
            return RedirectToAction("index", "Category");

        }


        private void InitMainCategories(int? categoryToExclude, ref CategoryModel categoryModel)
        {

            var categoriesList = categoryService.ReadAll();

            if(categoryToExclude!=null)
            {
                var currentCategory = categoriesList.Where(c => c.ID == categoryToExclude).FirstOrDefault();
                categoriesList.Remove(currentCategory);
            }


            categoryModel.MainCategories = new SelectList(categoriesList, "ID", "Name");

        }
    }
}