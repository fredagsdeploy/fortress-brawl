using UnityEngine;

namespace Utils
{
    public static class DynamicObjectsUtil
    {
        public static Transform DynamicRoot { get; } = GameObject.FindWithTag("DynamicObjectsRoot").transform;
    }
}