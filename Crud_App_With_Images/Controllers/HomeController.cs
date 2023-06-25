using Crud_App_With_Images.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_App_With_Images.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        EmployeeEntities db = new EmployeeEntities();
        public ActionResult Index()
        {
            var data = db.employee2.ToList();
            return View(data);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public ActionResult Create(employee2 e)
        {
            if(ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);
                HttpPostedFileBase postedFile = e.ImageFile;
                int length = postedFile.ContentLength;

                if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".ppg")
                {
                    if(length<= 1000000)
                    {
                        fileName = fileName + extension;
                        e.image_path = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        e.ImageFile.SaveAs(fileName);
                        db.employee2.Add(e);
                        int a = db.SaveChanges();
                        if(a > 0)
                        {
                            TempData["Message"] = "<script>alert('Data Inserted..')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            TempData["Message"] = "<script>alert('Data Not Inserted!!!!!!!')</script>";
                        }
                    }
                    else
                    {
                        TempData["sizeMessage"] = "<script>alert('Image should be less than 1 MB..')</script>";
                    }
                }
                else
                {
                    TempData["extentionMessage"] = "<script>alert('Image extention not supported')</script>";
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var employeeRow = db.employee2.Where(model => model.id == id).FirstOrDefault();

            Session["Image"] = employeeRow.image_path;

            return View(employeeRow);
        }

        [HttpPost]
        public ActionResult Edit(employee2 e)
        {
            if (ModelState.IsValid == true)
            {
                if(e.ImageFile!=null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".ppg")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.image_path = "~/Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            e.ImageFile.SaveAs(fileName);
                            db.Entry(e).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {

                                string ImagePath = Request.MapPath(Session["Image"].ToString());
                                if (System.IO.File.Exists(ImagePath))
                                {
                                    System.IO.File.Delete(ImagePath);
                                }
                                TempData["UMessage"] = "<script>alert('Data updated..')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UMessage"] = "<script>alert('Data Not updated!!!!!!!')</script>";
                            }
                        }
                        else
                        {
                            TempData["sizeMessage"] = "<script>alert('Image should be less than 1 MB..')</script>";
                        }
                    }
                    else
                    {
                        TempData["extentionMessage"] = "<script>alert('Image extention not supported')</script>";
                    }

                }
                else
                {
                    e.image_path = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UMessage"] = "<script>alert('Data updated..')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UMessage"] = "<script>alert('Data Not updated!!!!!!!')</script>";
                    }
                }
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            if(id> 0)
            {
                var EmployeeRow = db.employee2.Where(model => model.id == id).FirstOrDefault();

                if (EmployeeRow != null)
                {
                    db.Entry(EmployeeRow).State = EntityState.Deleted;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UMessage"] = "<script>alert('Data Deleted..')</script>";
                        string ImagePath = Request.MapPath(EmployeeRow.image_path.ToString());
                        if(System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                       
                    }
                    else
                    {
                        TempData["UMessage"] = "<script>alert('Data Not Deleted!!!!!!!')</script>";
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id)
        {
            var EmployeeRow = db.employee2.Where(model => model.id == id).FirstOrDefault();
            Session["Image2"] = EmployeeRow.image_path.ToString();
            return View(EmployeeRow);
        }
    }
}