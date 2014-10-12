using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LR.Models.RaceModels
{
    public class RaceModel
    {
        private int racersCount = 4;

        public Guid RaceId { get; set; }

        public int Length { get; set; }

        public int RacerMaximimCount { get { return racersCount; } }

        public List<RacerModel> Racers { get; set; }

        public List<RacerModel> Places { get; set; }

        public DateTime FinishTime { get; set; }

        public bool IsFinished { get; set; }

        public bool IsStarted { get; set; }

        public int Version { get; set; }

        public Guid CategoryId { get; set; }

        public RaceModel(Guid categoryId)
        {
            RaceId = Guid.NewGuid();
            Racers = new List<RacerModel>();
            Places = new List<RacerModel>();
            Length = 1000;
            CategoryId = categoryId;
        }
    }
}