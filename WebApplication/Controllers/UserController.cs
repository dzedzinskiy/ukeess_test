using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using BLL.DataAccess;
using BLL.UnitOfWork;
using Models;
using Models.Contacts;
using Models.DataContexts;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        
        private readonly IUserRepositoryProxy repository;

        public UserController(IUserRepositoryProxy repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        //public UserController()
        //{
        //    this.appContext = new MyAppContext();
        //    this.repository = new UserRepositoryProxy(appContext);
        //    this.unitOfWork = new UnitOfWork(appContext);
        //}

        // GET: /User/
        public ActionResult Index()
        {
            return View(repository.GetAllUsers());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = repository.GetUserById(id.Value);

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
            using (unitOfWork)
            {
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
                repository.InsertUser(user);
                unitOfWork.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.GetUserById(id.Value);
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
            [Bind(Include = "ID,FirstName,LastName,BirthDate,Sex,Married,Sallary,PrimaryContact,SecondaryContact,AdministrativeContact")] User user)
        {
            if (ModelState.IsValid)
            {
                using (unitOfWork)
                {
                    repository.UpdateUser(user);
                    unitOfWork.SaveChanges();
                }
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
            User user = repository.GetUserById(id.Value);
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
            using (unitOfWork)
            {
                repository.DeleteUser(id);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string DeleteUserContact(int userId, int contactId, int contactType)
        {
            using (unitOfWork)
            {
                repository.DeleteUserContact(userId, contactId, contactType);
                unitOfWork.SaveChanges();
            }
            return "Success";
        }

        [HttpPost]
        public string CreateUserContact(int userId, [Bind(Include = "ID,Name,Phone,Fax,Email,Note")] Contact contact,
            int contactType)
        {
            contact.ContactType = (ContactTypes) contactType;
            repository.InsertUserContact(userId, contact);
            unitOfWork.SaveChanges();
            return "Success";
        }

        [HttpPost]
        public string UpdateUserContact([Bind(Include = "ID,Name,Phone,Fax,Email,Note")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (unitOfWork)
                {
                    repository.UpdateUserContact(contact);
                    unitOfWork.SaveChanges(); 
                }
            }
            return "Success";
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}