using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utils
{
    public class UniqueRandom
    {
        private List<int> listOfRandoms = new List<int>();
        private readonly int minimum;
        private readonly int maximum;

        public UniqueRandom(int min, int max)
        {
            minimum = min;
            maximum = max;
            
            ResetList();
        }
        
        public int GetRandomInt()
        {
            if (listOfRandoms.Count == 0)
                ResetList();
            
            int randomIndex = Random.Range(0, listOfRandoms.Count);
            int randomNumber = listOfRandoms[randomIndex];
            listOfRandoms.RemoveAt(randomIndex);

            return randomNumber;
        }

        private void ResetList()
        {
            for (int i = minimum; i < maximum; i++)
            {
                listOfRandoms.Add(i);
            }
        }
    }
}