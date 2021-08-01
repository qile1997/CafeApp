﻿using CafeApp.DomainEntity;
using CafeApp.DomainEntity.Interfaces;
using CafeApp.Persistance;
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
        private iFoodRepository FoodRepository { get; set; }
        private CafeWebApp _context { get; set; }

        // GET: Foods
        public ActionResult Index(CafeWebApp context)
        {
            _context = context;
            InitializeData();
            return View(FoodRepository.ReadAllFoods());
        }

        public void InitializeData()
        {
            FoodRepository = new FoodRepository(_context);
        }
        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food foods = FoodRepository.GetFoodById(id);
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
        public ActionResult Create(Food foods, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                UploadPhoto(foods, Photo);
                if (ViewBag.FailMessage != null)
                {
                    return View();
                }
                FoodRepository.CreateFood(foods);
                return RedirectToAction("Index");
            }

            return View(foods);
        }
        public void UploadPhoto(Food foods, HttpPostedFileBase Photo)
        {
            if (Photo != null)
            {
                string fileExtension = Path.GetExtension(Photo.FileName);

                if (!fileExtension.ToLower().Equals(".jpg") || !fileExtension.ToLower().Equals(".png"))
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
            Food foods = FoodRepository.GetFoodById(id);
            ViewBag.FoodPicture = foods.PhotoFile;
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
        public ActionResult Edit([Bind(Include = "FoodsId,FoodCategory,FoodName,Price,Remarks")] Food foods, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                FoodRepository.UpdateFood(foods);
                UploadPhoto(foods, Photo);
                FoodRepository.SaveChanges();
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
            Food foods = FoodRepository.GetFoodById(id);
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
            Food foods = FoodRepository.GetFoodById(id);
            FoodRepository.DeleteFood(foods);
            return RedirectToAction("Index");
        }

    }
}
