using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace eHealthWorkshopGroup4
{
    public class Technique
    {
        public string name { get; set; }
        public string category { get; set; }
        public Rank minRank { get; set; }
        public int xp { get; set; }
        public string imageSorce { get; set; }
        public string note { get; set; }

        public Technique(string name, string category, Rank minRank, int xp, string imageSorce, string note)
        {
            this.name = name;
            this.category = category;
            this.minRank = minRank;
            this.xp = xp;
            this.imageSorce = imageSorce;
            this.note = note;
        }

    }
}
