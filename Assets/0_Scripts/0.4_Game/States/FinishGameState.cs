using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class FinishGameState : State<GameManager>
    {
        public FinishGameState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.Broadcast(EventID.OnFinishGame);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
