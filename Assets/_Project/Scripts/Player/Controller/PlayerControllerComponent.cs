using TriplanoTest.Data;

namespace TriplanoTest.Player
{
    public abstract class PlayerControllerComponent
    { 
        protected PlayerController Controller { get; private set; }
        protected PlayerData Data => GameData.Player;

        public virtual void Init(PlayerController controller)
        {
            Controller = controller;
        }
    }

}