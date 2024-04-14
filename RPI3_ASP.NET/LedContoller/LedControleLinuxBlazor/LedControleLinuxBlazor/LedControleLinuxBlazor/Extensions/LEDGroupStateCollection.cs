using LedControleLinuxBlazor.Model;
using System.Collections.ObjectModel;

namespace LedControleLinuxBlazor.Extensions
{
    public class LEDGroupStateCollection: ObservableCollection<LedGroup>
    {
        
        /// <summary>
        /// Updates the collection based on a provided list of LedGroups.
        /// Removes existing elements and then fills with new ones from the list.
        /// </summary>
        /// <param name="newGroups">The new list of LedGroup items to use.</param>
        public void UpdateFromList(List<LedGroup> newGroups)
        {
            int numborOfLedGroup = 0;
            this.Clear(); 
            foreach (var group in newGroups)
            {
                group.GroupState.LedNumber = numborOfLedGroup;
                this.Add(group);
                numborOfLedGroup++;
            }
        }

        /// <summary>
        /// Converts the ObservableCollection back to a standard List.
        /// </summary>
        /// <returns>A List containing all elements from the ObservableCollection.</returns>
        public List<LedGroup> ToList()
        {
            return new List<LedGroup>(this);
        }
    }
}
