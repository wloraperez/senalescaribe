using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DataSenalesCaribe;
using SenalesDelCaribe.Models;

namespace SenalesDelCaribe.Controllers
{
    public class GeneralPageController : Controller
    {
        //
        // GET: /GeneralPage/

        public ActionResult Index(int id)
        {
            var tmpSb = new StringBuilder();

            var db = new ContentDataContext(Comun.GetConnString());
            var tmpCategory = db.Categories.Where(c => c.IDCategory == id).FirstOrDefault();
            var tmpContent = db.Content_Categories
                            .Where(c => c.IDCategory == id)
                            .Select(c => c.Content)
                            .FirstOrDefault();

            ViewBag.PhotoId = tmpCategory != null ? tmpCategory.IDPhoto : 0;
            ViewBag.Slide = tmpContent != null && tmpContent.ContentPhotos != null 
                         && tmpContent.ContentPhotos.Any() ? true : false;

            var tmpCategoryTypeEnum = (DataSenalesCaribe.Comun.Enums.CategoryType)tmpCategory.pCategoryType;
            ViewBag.CategoryType = tmpCategoryTypeEnum;
            

            return View(tmpContent);
        }

        public ActionResult SlideContainer(int vContentId)
        {
            var db = new ContentDataContext(Comun.GetConnString());
            var tmpPhoto = db.ContentPhotos.Where(c => c.IDContent == vContentId)
                            .Where(c => c.IsDeleted == false)
                            .Select(c => new CPhoto
                            {
                                IDPhoto = c.IDPhoto,
                                Description = c.Photo.Description 
                            })
                            .ToList();
   
            return PartialView("_SlideContainer", tmpPhoto);
        }

        public FileContentResult GetPhoto(int id)
        {
            var db = new ContentDataContext(Comun.GetConnString());
            var tmpPhoto = db.Photos.Where(c => c.IDPhoto == id).FirstOrDefault();

            if (tmpPhoto != null && tmpPhoto.PhotoLarge != null && tmpPhoto.PhotoLarge.Length > 0)
            {
                return File(tmpPhoto.PhotoLarge.ToArray(), tmpPhoto.ContentType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult ContactPage()
        {
            return PartialView("_ContactPage");
        }

        [HttpPost]
        public ActionResult ContactPage(Models.CContact contactVM)
        {
            if (!ModelState.IsValid)
            {
                return View(contactVM);
            }

            var contact = new CContact
            {
                From = contactVM.From,
                Subject = contactVM.Subject,
                Message = contactVM.Message
            };

            new CEmail().Send(contact);

            return RedirectToAction("ContactConfirm");
        }
    }
}


