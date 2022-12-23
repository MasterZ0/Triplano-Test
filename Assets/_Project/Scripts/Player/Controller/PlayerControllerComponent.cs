namespace TriplanoTest.Player
{
    public abstract class PlayerControllerComponent
    { 
        protected PlayerController Controller { get; private set; }
        public virtual void Init(PlayerController controller)
        {
            Controller = controller;
        }
    }

}