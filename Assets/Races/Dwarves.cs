using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Races
{
    public class Dwarves: Race
    {
        protected static Dwarves instance;
        public static Dwarves Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = (Dwarves) FindObjectOfType(typeof(Dwarves));
 
                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(Dwarves) + 
                                       " is needed in the scene, but there is none.");
                    }
                }
 
                return instance;
            }
        }

        public List<ConstructionInfo> constructionInfos;
        public override string getName()
        {
            return "Dwarves";
        }

        public override List<ConstructionInfo> GetConstructionInfos()
        {
            return constructionInfos;
        }
    }
}