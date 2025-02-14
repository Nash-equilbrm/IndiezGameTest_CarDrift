using Game.UI;
using Patterns;
using System;
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
            _context.Register(EventID.FinishCounting, OnStartGameplay);
            UIManager.Instance.ShowOverlap<GameplayOverlap>(data: _context.CarController, forceShowData: true);

            _context.Broadcast(EventID.OnGameStartCounting);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.FinishCounting, OnStartGameplay);
        }

        private void OnStartGameplay(object obj)
        {
            _context.ChangeToGameplayState();
        }
    }
}

