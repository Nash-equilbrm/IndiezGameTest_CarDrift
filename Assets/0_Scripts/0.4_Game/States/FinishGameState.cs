using Game.UI;
using Patterns;
using System;
using UI;

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
            UIManager.Instance.ShowPopup<ReplayPopup>(forceShowData: true);
            _context.Register(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
            _context.Broadcast(EventID.OnFinishGame);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
        }

        private void OnReplayBtnClicked(object obj)
        {
            _context.ChangeToInitGameState();
        }
    }
}
