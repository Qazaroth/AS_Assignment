using AS_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_Assignment.Services
{
    public class UserService
    {
        private DBContext _ctx;

        public UserService(DBContext _ctx)
        {
            this._ctx = _ctx;
        }

        public Boolean addUser(User user)
        {
            List<User> allUsers = _ctx.Users.ToList();

            if (allUsers.Contains(user))
                return false;

            foreach (User u in allUsers)
            {
                if (u.email == user.email)
                    return false;
            }

            allUsers.Add(user);
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            return true;
        }

        public Boolean userEquals(User u1, User u2)
        {
            return (u1.email == u2.email &&
                u1.NRIC == u2.NRIC);
        }

        public Boolean userEqualsAll(User u1, User u2)
        {
            return (u1.email == u2.email &&
                u1.NRIC == u2.NRIC &&
                u1.firstName == u2.firstName &&
                u1.lastName == u2.lastName);
        }

        public Boolean updateUser(User newUser)
        {
            List<User> users = _ctx.Users.ToList();
            User oldUser = getUserByID(newUser.id);

            int index = users.IndexOf(oldUser);
            oldUser.email = (newUser.email == null) ? oldUser.email : newUser.email;
            oldUser.firstName = (newUser.firstName == null) ? oldUser.firstName : newUser.firstName;
            oldUser.lastName = (newUser.lastName == null) ? oldUser.lastName : newUser.lastName;
            _ctx.SaveChanges();
            return true;


            /**if (index >= 0 && index < users.Count)
            {
                User oldUser = users[index];

                users[index] = user;

                return userEquals(users[index], oldUser);
            }
            return false;**/
        }

        public int getUserTries(string email)
        {
            User u = getUserByEmail(email);

            return u.tries;
        }

        public User getUserByID(int id)
        {
            User u = null;
            List<User> allUsers = _ctx.Users.ToList();

            foreach (User a in allUsers)
            {
                if (a.id == id)
                {
                    u = a;
                    break;
                }
            }

            return u;
        }

        public User getUserByEmail(string email)
        {
            User u = null;
            List<User> allUsers = _ctx.Users.ToList();

            foreach (User a in allUsers)
            {
                if (a.email == email)
                {
                    u = a;
                    break;
                }
            }

            return u;
        }

        public User getUserByNRIC(string nric)
        {
            User u = null;
            List<User> allUsers = _ctx.Users.ToList();

            foreach (User a in allUsers)
            {
                if (a.NRIC == nric)
                {
                    u = a;
                    break;
                }
            }

            return u;
        }

        public List<User> getAllUsers()
        {
            return _ctx.Users.ToList();
        }
    }
}
