using Commons;
using Game.Car;
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
            _context.PubSubRegister(EventID.FinishCounting, OnStartGameplay);
            Tuple<CarController, CarController> data = Tuple.Create(_context.CarController, _context.OpponentCarController);
            UIManager.Instance.ShowOverlap<GameplayOverlap>(data: data, forceShowData: true);
            _context.PubSubBroadcast(EventID.OnGameStartCounting);
        }

        public override void Exit()
        {
            base.Exit();
            //_context.Unregister(EventID.FinishCounting, OnStartGameplay);
            _context.PubSubUnregister(EventID.FinishCounting, OnStartGameplay);
        }

        private void OnStartGameplay(object obj)
        {
            _context.ChangeToGameplayState();
        }
    }
}

