using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using DAL;
using DAL.Models;
using DAL.Models.Contacts;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: /User/
        public ActionResult Index()
        {
            return View(unitOfWork.UserRepository.GetAllUsers());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = unitOfWork.UserRepository.GetUserById(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                Include =
                    "ID,FirstName,LastName,BirthDate,Sex,Married,Sallary,PrimaryContact,SecondaryContact,AdministrativeContact"
                )] User user)
        {
            //Required PrimaryContact data
            if (user.PrimaryContact.Name.IsEmpty() || user.PrimaryContact.Phone.IsEmpty())
            {
                return RedirectToAction("Create");
            }
            if (user.SecondaryContact != null && user.SecondaryContact.Name.IsEmpty())
            {
                user.SecondaryContact = null;
            }
            if (user.AdministrativeContact != null && user.AdministrativeContact.Name.IsEmpty())
            {
                user.AdministrativeContact = null;
            }
            unitOfWork.UserRepository.InsertUser(user);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = unitOfWork.UserRepository.GetUserById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                Include =
                    "ID,FirstName,LastName,BirthDate,Sex,Married,Sallary,PrimaryContact,SecondaryContact,AdministrativeContact"
                )] User user)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.UserRepository.UpdateUser(user);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = unitOfWork.UserRepository.GetUserById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.UserRepository.DeleteUser(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string DeleteUserContact(int userId, int contactId, int contactType)
        {
            unitOfWork.UserRepository.DeleteUserContact(userId, contactId, contactType);
            unitOfWork.Save();
            return "Success";
        }

        [HttpPost]
        public string CreateUserContact(int userId, [Bind(Include = "ID,Name,Phone,Fax,Email,Note")] Contact contact,
            int contactType)
        {
            contact.ContactType = (ContactTypes) contactType;
            unitOfWork.UserRepository.InsertUserContact(userId, contact);
            unitOfWork.Save();
            return "Success";
        }

        [HttpPost]
        public string UpdateUserContact([Bind(Include = "ID,Name,Phone,Fax,Email,Note")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.UserRepository.UpdateUserContact(contact);
                unitOfWork.Save();
            }
            return "Success";
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}