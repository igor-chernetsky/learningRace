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

        public int AvrSpeed { get; set; }

        public int DSpeed { get; set; }

        public int Length { get; set; }

        public bool IsReady { get; set; }

        public bool IsFinished { get; set; }

        public int ReadyTime { get; set; }

        public bool HasGotResults { get; set; }

        public int TrysCountToGetResult { get; set; }

        public int Score { get; set; }

        public RacerModel()
        {
            DSpeed = 1;
        }
    }
}