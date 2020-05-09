
using UnityEngine;

namespace Code
{
    public interface IMovableListener
    {
        void Stopped();
        void Started(Vector3 destination);
    }
}
