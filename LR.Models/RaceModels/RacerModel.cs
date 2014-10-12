using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LR.Models.RaceModels
{
    public class RacerModel
    {
        public UserProfile Racer { get; set; }

        public int Speed { get; set; }

        public Car RaceCar {get; set;}

        public int Length { get; set; }

        public bool IsReady { get; set; }

        public bool IsFinished { get; set; }

        public int ReadyTime { get; set; }

        public bool HasGotResults { get; set; }

        public int TrysCountToGetResult { get; set; }

        public int Score { get; set; }

        public string Message { get; set; }

        public Result RaceResult = new Result();

        #region Constructors

        public RacerModel(UserProfile user, Car car)
        {
            Racer = user;
            RaceCar = car;
            ReadyTime = 5;
            Speed = 5;
        }

        #endregion

        #region Methods

        public void MoveRacer()
        {
            Length += Speed;
            if (Speed > RaceCar.AvrSpeed)
            {
                Speed -= RaceCar.DSpeed;
            }
            if (Speed < RaceCar.AvrSpeed)
            {
                Speed += RaceCar.DSpeed;
            }
        }

        public void ChangeSpeed(bool increase)
        {
            if (increase)
            {
                Speed += RaceCar.Accseleration;
                if (Speed > RaceCar.MaxSpeed) Speed = RaceCar.MaxSpeed;
            }
            else
            {
                Speed -= RaceCar.Breaks;
                if (Speed < 1) Speed = 1;
            }
            Score += increase ? 2 : -1;
        }

        #endregion

        #region internalClasses

        public class Result
        {
            public int RacersCount;
            public int RacerPlace;
        }

        #endregion
    }
}