using Commons;
using Game.Car;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Game.UI
{
    public class GameplayOverlap : BaseOverlap
    {
        public MobileJoystick joystick = null;
        private CarController _carMovementController = null;

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
            Tuple<CarController, CarController> controllers = data as Tuple<CarController, CarController>;
            _carMovementController = controllers.Item1;
            if (!_carMovementController) return;
            LogUtility.Info("GameplayOverlap", "_carMovementController.onGetInputValue = GetJoystickInput");
            _carMovementController.onGetInputValue = GetJoystickInput;
        }

        private Vector2 GetJoystickInput()
        {
            return joystick.JoystickInput;
        }
    }
}

