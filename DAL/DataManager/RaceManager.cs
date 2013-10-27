using Entities.RaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DataManager
{
    public class RaceManager : MainContext
    {
        public List<RaceModel> GetUserRaces(int userId)
        {
            List<RaceModel> result = RaceDataContext.Races.Where(r => RaceDataContext.RaceUsers.Where(ru => ru.UserId == userId).Select(ru => ru.RaceId).Contains(r.Id))
                .Select(r => new RaceModel()
                {
                    RaceId = r.Id,
                    FinishTime = r.FinishTime.Value,
                    Places = getPlaces(r)
                }).ToList();
            return result;
        }

        public Guid CreateRace(RaceModel race, Guid categoryId)
        {
            DAL.Race raceToStore = new DAL.Race()
            {
                Id = race.RaceId,
                CategoryId = categoryId
            };
            RaceDataContext.Races.InsertOnSubmit(raceToStore);
            RaceDataContext.SubmitChanges();
            return raceToStore.Id;
        }

        public void AddUserToRace(int userId, Guid raceId)
        {
            DAL.RaceUser userRace = new DAL.RaceUser()
            {
                RaceId = raceId,
                UserId = userId
            };
            RaceDataContext.RaceUsers.InsertOnSubmit(userRace);
            RaceDataContext.SubmitChanges();
        }

        public void UpdateUserInRace(int userId, byte? place, Guid raceId)
        {
            DAL.RaceUser userRace = RaceDataContext.RaceUsers.FirstOrDefault(ur => ur.RaceId == raceId && ur.UserId == userId);
            userRace.Place = place;
            DAL.Race race = RaceDataContext.Races.FirstOrDefault(r => r.Id == raceId);
            if (place == race.RaceUsers.Count)
            {
                race.FinishTime = DateTime.Now;
            }
            
            RaceDataContext.SubmitChanges();
        }

        #region Private_methods

        private List<RacerModel> getPlaces(DAL.Race race)
        {
            List<RacerModel> result = new List<RacerModel>();
            result.AddRange(RaceDataContext.UserProfiles
                .Where(u => RaceDataContext.RaceUsers.Where(ru => ru.RaceId == race.Id).Select(ur => ur.UserId).Contains(u.UserId))
                .Select(u => new RacerModel()
                {
                    RacerId = u.UserId,
                    RacerName = u.UserName 
                }));
            return result;
        }

        #endregion
    }
}
