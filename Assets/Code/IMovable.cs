
using UnityEngine;

namespace Code
{
    public interface IMovable
    {
        void SetDestination(Vector3 destination);
        void Stop();
    }
}
