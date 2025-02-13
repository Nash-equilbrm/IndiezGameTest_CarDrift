using Game.UI;
using Patterns;
using UI;


namespace Game.States
{
    public class StartGameState : State<GameManager>
    {
        public StartGameState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.ShowOverlap<GameplayOverlap>(data: _context.CarController, forceShowData: true);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

