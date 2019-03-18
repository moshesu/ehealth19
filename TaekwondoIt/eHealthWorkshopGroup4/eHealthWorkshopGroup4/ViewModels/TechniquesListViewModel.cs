using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace eHealthWorkshopGroup4.ViewModels
{
    class TechniquesListViewModel
    {
        public ObservableCollection<Technique> TechniquesList { get; set; }

        public TechniquesListViewModel()
        {
            string str = "";
                
            TechniquesList = new ObservableCollection<Technique>();
            TechniquesList.Add(new Technique("apkubi", "stances", Rank.White, 3, "apkubi.jpg", str));
            TechniquesList.Add(new Technique("beom sugi", "stances", Rank.Blue, 3, "beom_sugi.jpg", str));
            TechniquesList.Add(new Technique("juchum sugi", "stances", Rank.Red, 5, "juchum_sugi.jpg", str));
            TechniquesList.Add(new Technique("yop chagi", "kicks", Rank.Blue, 10, "yop_chagi.jpg", str));
            TechniquesList.Add(new Technique("an chagi", "kicks", Rank.Red, 8, "an_chagi.jpg", str));
            TechniquesList.Add(new Technique("santeul makki", "blocks", Rank.Red, 5, "santeul_makki.jpg", str));
            TechniquesList.Add(new Technique("yop jireugi", "strikes", Rank.Red, 5, "yop_jireugi.jpg", str));
            TechniquesList.Add(new Technique("gawi makki", "blocks", Rank.Blue, 8, "gawi_makki.jpg", str));
            TechniquesList.Add(new Technique("jebipoom sonal mok chigi", "blocks", Rank.Blue, 10, "jebipoom_sonal_mok_chigi.jpg", str));
            TechniquesList.Add(new Technique("hwangso makki", "blocks", Rank.Black5, 15, "hwangso_makki.jpg", str));
        }
    }
}
