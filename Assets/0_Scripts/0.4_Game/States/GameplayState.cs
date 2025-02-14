using Patterns;
using System;


namespace Game.States
{
    public class GameplayState : State<GameManager>
    {
        public GameplayState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.Register(EventID.OnHitFinishLine, OnHitFinishLine);
            _context.Broadcast(EventID.OnStartGameplay);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.OnHitFinishLine, OnHitFinishLine);
        }

        private void OnHitFinishLine(object obj)
        {
            _context.ChangeToFinishGameState();
        }
    }
}

