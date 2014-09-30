using System;
using System.Collections.Generic;
using DAL.Models;
using DAL.Models.Contacts;

namespace DAL.Repositories
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void InsertUser(User item);
        void UpdateUser(User item);
        void DeleteUser(int id);

        void InsertUserContact(int userId, Contact contact);

        void Save();
    }
}