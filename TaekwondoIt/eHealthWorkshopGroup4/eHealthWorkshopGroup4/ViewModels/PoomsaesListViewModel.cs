using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace eHealthWorkshopGroup4.ViewModels
{
    class PoomsaesListViewModel
    {

        public ObservableCollection<Poomsae> PoomsaesList { get; set; }

        public PoomsaesListViewModel()
        {
            PoomsaesList = new ObservableCollection<Poomsae>();

            foreach (Poomsae p in Poomsae.Values)
            {
                PoomsaesList.Add(p);
            }  
        }
    }
}
