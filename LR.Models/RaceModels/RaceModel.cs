using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LR.Models.RaceModels
{
    public class RaceModel
    {
        private int racersCount = 5;

        public Guid RaceId { get; set; }

        public int Length { get; set; }

        public int RacerMaximimCount { get { return racersCount; } }

        public List<RacerModel> Racers { get; set; }

        public List<RacerModel> Places { get; set; }

        public DateTime FinishTime { get; set; }

        public bool IsFinished { get; set; }

        public bool IsStarted { get; set; }

        public int Version { get; set; }

        public RaceModel()
        {
            RaceId = Guid.NewGuid();
            Racers = new List<RacerModel>();
            Places = new List<RacerModel>();
            Length = 1000;
        }
    }
}