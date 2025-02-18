using Commons;
using Patterns;
using System;
using Game.Car;


namespace Game.States
{
    public class GameplayState : State<GameManager>
    {
        public GameplayState(GameManager context) : base(context)
        {
        }

        public GameplayState(GameManager context, string name) : base(context, name)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.PubSubRegister(EventID.OnHitFinishLine, OnHitFinishLine);
            _context.PubSubBroadcast(EventID.OnStartGameplay);
        }

        public override void Exit()
        {
            base.Exit();
            _context.PubSubUnregister(EventID.OnHitFinishLine, OnHitFinishLine);
        }

        private void OnHitFinishLine(object obj)
        {
            CarCollision winner = obj as CarCollision;
            if (winner != null)
            {
                _context.PlayerWin = winner.gameObject == _context.CarController.gameObject;
            }
            _context.ChangeToFinishGameState();
        }
    }
}

