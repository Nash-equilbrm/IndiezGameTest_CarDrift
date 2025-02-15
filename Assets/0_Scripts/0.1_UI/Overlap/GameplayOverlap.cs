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
        private CarMovement _carMovementController;

        public override void Hide()
        {
            base.Hide();
            if (!_carMovementController) return;
            _carMovementController.onGetInputValue = null;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            CarController controller = (CarController)data;
            _carMovementController = controller.MovementController;
            if (!_carMovementController) return;
            _carMovementController.onGetInputValue = () => new Vector2(joystick.Horizontal, joystick.Vertical);
        }
    }
}

