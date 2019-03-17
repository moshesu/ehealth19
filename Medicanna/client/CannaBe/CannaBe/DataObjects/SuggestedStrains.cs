
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class SuggestedStrains : ViewModel
    {
        private int status;

        [JsonProperty("status")]
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private List<Strain> suggestedStrainList;

        [JsonProperty("suggestedStrains")]
        public List<Strain> SuggestedStrainList
        {
            get
            {
                return suggestedStrainList;
            }
            set
            {
                suggestedStrainList = value;
                OnPropertyChanged("SuggestedStrainList");
            }
        }

        [JsonConstructor]
        public SuggestedStrains(int status, List<Strain> suggestedStrains)
        {
            this.Status = status;
            this.SuggestedStrainList = suggestedStrains;
        }
    }
}
