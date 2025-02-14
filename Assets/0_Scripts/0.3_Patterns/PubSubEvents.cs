using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public enum EventID
    {
        #region Gameplay
        OnInitGame,
        OnStartInitGame,
        OnSpawnedCars,
        OnGameStartCounting,
        OnGameEndCounting,
        OnStartGameplay,
        FinishCounting,
        OnHitFinishLine,
        OnFinishGame,
        OnReplayBtnClicked,
        #endregion

        #region UI


        #endregion
    }
}
