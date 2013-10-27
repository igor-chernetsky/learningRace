using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Data.Providers
{
    public class UserProvider : MainProvider
    {
        public bool AddUser(string username)
        {
            UserProfile user = Context.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            // Check if user already exists
            if (user == null)
            {
                // Insert name into the profile table
                Context.UserProfile.Add(new UserProfile { UserName = username });
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserProfile GetUserById(int id)
        {
            UserProfile result = Context.UserProfile.FirstOrDefault(u => u.UserId == id);
            return result;
        }

        public List<UserProfile> GetUsers()
        {
            List<UserProfile> result = Context.UserProfile.OrderBy(u => u.Score).ToList();
            return result;
        }

        public void DeleteUser(int id)
        {
            UserProfile user = GetUserById(id);
            Context.UserProfile.Remove(user);
            Context.SaveChanges();
        }

        public void AddScoreToUser(int score, UserProfile user)
        {
            UserProfile dbUser = Context.UserProfile.First(u => u.UserId == user.UserId);
            dbUser.Score = user.Score.HasValue ? user.Score.Value + score : score;
            Context.SaveChanges();
        }

    }
}
