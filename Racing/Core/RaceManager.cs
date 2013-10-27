using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using LR.Models.RaceModels;
using LR.Data.Providers;

namespace Racing.Core
{
    public class RaceManager
    {
        #region Properties

        public static bool IsRaceStarted = false;

        public static Thread RaceThread { get; set; }
        //started races
        public static List<RaceModel> Races { get; set; }
        //not started races
        public static List<RaceModel> IdleRaces {get;set;}

        #endregion

        #region PublicMethods

        public static RaceModel AddRacer(int racerId, Guid categoryId)
        {
            InitLists();
            RaceModel race = Races.FirstOrDefault(r => r.Racers.FirstOrDefault(rc => rc.Racer.UserId == racerId) != null);
            if (race == null)
            {
                race = IdleRaces.FirstOrDefault(r => r.Racers.FirstOrDefault(rc => rc.Racer.UserId == racerId) != null);
            }
            if (race == null)
            {
                race = IdleRaces.FirstOrDefault(r => r.Racers.Count < r.RacerMaximimCount);
                if (race == null)
                {
                    race = new RaceModel();
                    IdleRaces.Add(race);
                }
                RacerModel racer = new RacerModel();

                racer.Racer = DataProvider.UserProfile.GetUserById(racerId);
                racer.AvrSpeed = 25;
                racer.ReadyTime = 5;
                racer.Speed = 5;

                race.Racers.Add(racer);
                race.Version++;
            }
            StartRace();
            return race;
        }

        public static RaceModel GetRaceById(Guid raceId)
        {
            RaceModel result = Races.FirstOrDefault(r => r.RaceId == raceId);
            if (result == null)
            {
                result = IdleRaces.FirstOrDefault(r => r.RaceId == raceId);
            }
            return result;
        }

        public static RacerModel GetRacerById(Guid raceid, int racerId)
        {
            RacerModel result = GetRaceById(raceid).Racers.FirstOrDefault(r => r.Racer.UserId == racerId);
            return result;
        }

        public static void ChangeRaceState(Guid raceid, int racerId, bool isReady)
        {
            RacerModel racer = GetRacerById(raceid, racerId);
            IncreaceVersion(raceid);
            racer.IsReady = isReady;
        }

        public static void ChangeSpeed(Guid raceId, int racerId, int delta)
        {
            RacerModel racer = GetRacerById(raceId, racerId);
            IncreaceVersion(raceId);
            racer.Speed += delta;
            if (racer.Speed < 0)
                racer.Speed = 1;
            racer.Score += delta > 0 ? 2 : -1;
        }

        public static void StartRace()
        {
            if (!IsRaceStarted)
            {
                RaceThread = new Thread(new ThreadStart(racingActivity));
                IsRaceStarted = true;
                RaceThread.Start();
            }
        }

        #endregion

        private static void racingActivity()
        {
            while (IdleRaces.Count > 0 || Races.Count>0)
            {
                IdleRaces.Where(r => r.Racers.Count == r.RacerMaximimCount).ToList().ForEach(r => r.Racers.ForEach(rc => rc.ReadyTime -= !rc.IsReady && rc.ReadyTime != 0 ? 1 : 0));
                List<RaceModel> readyRaces = IdleRaces.Where(r => r.Racers.FirstOrDefault(rc => rc.ReadyTime != 0 && !rc.IsReady) == null).ToList();
                readyRaces.ForEach(r => r.IsStarted = true);
                IdleRaces.RemoveAll(r => readyRaces.Contains(r));
                Races.AddRange(readyRaces);

                Races.Where(r=>r.IsFinished).ToList().ForEach(r=>r.Racers.ForEach(rc=>rc.TrysCountToGetResult+= rc.HasGotResults ? 0 : 1));
                Races.RemoveAll(r => r.IsFinished && r.Racers.FirstOrDefault(rc => !rc.HasGotResults && rc.TrysCountToGetResult < 5) == null);
                
                Races.Where(r => !r.IsFinished).ToList().ForEach(r => r.Racers.ForEach(rc => MoveRacer(rc, r)));
                Races.ForEach(r => r.IsFinished = r.Racers.FirstOrDefault(rc => !rc.IsFinished) == null);
                Thread.Sleep(1000);
            }
            IsRaceStarted = false;
        }

        /// <summary>
        /// Moves racer forward and returns true if ther racer has finished
        /// </summary>
        private static void MoveRacer(RacerModel racer, RaceModel race)
        {
            if (racer.Length < race.Length)
            {
                racer.Length += racer.Speed;
                if (racer.Speed > racer.AvrSpeed)
                {
                    racer.Speed -= racer.DSpeed;
                }
                if (racer.Speed < racer.AvrSpeed)
                {
                    racer.Speed += racer.DSpeed;
                }
            }
            else if (!racer.IsFinished)
            {
                if (race.Places.FirstOrDefault(r => r.Racer.UserId == racer.Racer.UserId) == null)
                {
                    race.Places.Add(new RacerModel()
                        {
                            Racer = racer.Racer
                        }
                    );
                    int totalCount = (racer.Score * (10 + (race.Racers.Count - race.Places.Count))) / 10;
                    DataProvider.UserProfile.AddScoreToUser(totalCount, racer.Racer);
                }
                racer.Speed = 0;
                racer.IsFinished = true;
            }
        }

        private static void InitLists()
        {
            if(Races == null) Races = new List<RaceModel>();
            if (IdleRaces == null) IdleRaces = new List<RaceModel>();
        }

        private static void IncreaceVersion(Guid raceId)
        {
            GetRaceById(raceId).Version++;
        }
    }
}