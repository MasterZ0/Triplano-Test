using UnityEngine;

namespace TriplanoTest.Shared
{
    [CreateAssetMenu(menuName = MenuPath.ScriptableObjects + "Events/Game Event Bool", fileName = "NewGameEventBool")]
    public class GameEventBool : GameEvent<bool> { }
}