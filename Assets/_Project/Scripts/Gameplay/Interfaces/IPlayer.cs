using UnityEngine;

namespace TriplanoTest.Gameplay
{
    public interface IPlayer
    {
        void SetActiveInput(bool active);
        void SetPosition(Transform point);
    }
}