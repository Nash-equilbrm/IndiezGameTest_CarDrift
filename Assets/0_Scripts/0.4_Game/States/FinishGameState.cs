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
            UIManager.Instance.ShowPopup<ReplayPopup>(data: _context.PlayerWin, forceShowData: true);
            _context.PubSubRegister(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
            _context.PubSubBroadcast(EventID.OnFinishGame);
        }

        public override void Exit()
        {
            base.Exit();
            _context.PubSubUnregister(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
        }

        private void OnReplayBtnClicked(object obj)
        {
            _context.ChangeToInitGameState();
        }
    }
}
