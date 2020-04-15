using CafeApp.DomainEntity;
using CafeApp.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CafeApp.Controllers
{
    public class FoodsController : Controller
    {
       private FoodRepository foodRepo = new FoodRepository();
        // GET: Foods
        public ActionResult Index()
        {
           return View(foodRepo.GetFoods());
        }

        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            if (foods == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodPicture = foods.PhotoFile;
            return View(foods);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Foods foods, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                UploadPhoto(foods, Photo);
                if (ViewBag.FailMessage != null)
                {
                    return View();
                }
                foodRepo.AddFood(foods);
                return RedirectToAction("Index");
            }

            return View(foods);
        }
        public void UploadPhoto(Foods foods, HttpPostedFileBase Photo)
        {
            if (Photo != null)
            {
                string fileExtension = Path.GetExtension(Photo.FileName);

                if (!fileExtension.ToLower().Equals(".jpg"))
                {
                    ViewBag.FailMessage = "Photo invalid file type. Please use only .jpg file.";
                }
                else if (Photo.ContentLength > 1000000)
                {
                    ViewBag.FailMessage = "Photo size cannot be more than 1 MB ";
                }
                else if (Photo.ContentLength > 0 && Photo.ContentLength < 1000000)
                {
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), Photo.FileName);
                    Photo.SaveAs(_path);
                    foods.PhotoFile = Photo.FileName;
                }
                else
                {
                    ViewBag.FailMessage = "Invalid. Please check the photo file";
                }
            }
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            //ViewBag.FoodPicture = foods.PhotoFile;
            if (foods == null)
            {
                return HttpNotFound();
            }
            return View(foods);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Foods foods, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                foodRepo.UpdateFood(foods);
                UploadPhoto(foods,Photo);
                foodRepo.Save();
                return RedirectToAction("Index");
            }
            return View(foods);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foods foods = foodRepo.food(id);
            if (foods == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodPicture = foods.PhotoFile;
            return View(foods);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Foods foods = foodRepo.food(id);
            foodRepo.DeleteFood(foods);
            return RedirectToAction("Index");
        }

    }
}
