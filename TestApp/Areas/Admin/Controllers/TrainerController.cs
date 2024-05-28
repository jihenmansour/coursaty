using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly IMapper mapper;

        private readonly TrainerService trainerService;

        private readonly UserManager<MyIdentityUser> userManager;
        public TrainerController() 
        {
            mapper = AutoMapperConfig.Mapper;

            trainerService = new TrainerService();

            var db = new CoursesIdentityContext();
            var userStore = new UserStore<MyIdentityUser>(db);
            userManager = new UserManager<MyIdentityUser>(userStore);
        }
        // GET: Admin/Trainer
        public ActionResult Index()
        {
            var trainersList = trainerService.ReadAll();
            var mappedTrainersList = mapper.Map<IEnumerable<TrainerModel>>(trainersList);


            return View(mappedTrainersList);
        }

        // GET: Admin/Trainer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Trainer/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TrainerModel userInfo)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    var identityUser = new MyIdentityUser
                    {
                        Email = userInfo.Email,
                        UserName = userInfo.Email

                    };


                    var creationResult = await userManager.CreateAsync(identityUser, userInfo.Password);


                    //User Created
                    if (creationResult.Succeeded)
                    {
                        var userId = identityUser.Id;
                        creationResult = userManager.AddToRole(userId, "Trainer");


                        //Role Assigned
                        if (creationResult.Succeeded)
                        {
                            //Save to Trainee table

                            var savedTrainee = trainerService.Create(new Trainer
                            {
                                Email = userInfo.Email,
                                Name = userInfo.Name,
                                Website = userInfo.Website,
                                Creation_Date = DateTime.Now,
                            }); ;

                            if (savedTrainee == 0)
                            {
                                return View(userInfo);
                            }
                            return RedirectToAction("Index", "Trainer");
                        }
                    }
                    var message = creationResult.Errors.FirstOrDefault();
                }
                else
                {
                    ViewBag.Error = "The email already exists!";
                }
            }
            return View(userInfo);
        }

        // GET: Admin/Trainer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("index", "Home");
            }

            var currentTrainer = trainerService.ReadById(id.Value);

            if (currentTrainer == null)
            {
                return HttpNotFound($"This trainer ({id}) not found");
            }

            var TrainerModel = mapper.Map<TrainerModel>(currentTrainer);


            return View(TrainerModel);
        }

        // POST: Admin/Trainer/Edit/5
        [HttpPost]
        public ActionResult Edit(TrainerModel trainerData)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var updatedTrainer = mapper.Map<Trainer>(trainerData);

                    var result = trainerService.Update(updatedTrainer);

                    ViewBag.Success = true;
                    ViewBag.Message = $"Trainer {trainerData.Name} updated successfully.";

                }

                return View(trainerData);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(trainerData);
            }
        }

        // POST: Admin/Trainer/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int? Id, string Email)
        {

                var deleted = trainerService.Delete(Id.Value);

                var user = await userManager.FindByEmailAsync(Email);


                 var creationResult = await userManager.DeleteAsync(user);

                if (deleted)
                {
                return RedirectToAction("index", "Trainer");
            }
            return RedirectToAction("index", "Trainer");
        }
    }
}
