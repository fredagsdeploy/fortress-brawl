﻿using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Races
{
    public abstract class Race: MonoBehaviour
    {
        
        public abstract string Name { get; }
        public abstract List<BuildingInfo> GetBuildingInfos();
    }
}