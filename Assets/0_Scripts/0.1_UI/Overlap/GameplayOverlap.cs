using Game.Car;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Game.UI
{
    public class GameplayOverlap : BaseOverlap
    {
        public Joystick joystick;
        private CarController _controller;

        public override void Hide()
        {
            base.Hide();
            if (!_controller) return;
            _controller.onGetInputValue = null;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            if (data is not CarController controller) return;
            _controller = controller;
            if (!_controller) return;
            _controller.onGetInputValue = () => new Vector2(joystick.Horizontal, joystick.Vertical);
        }
    }
}

