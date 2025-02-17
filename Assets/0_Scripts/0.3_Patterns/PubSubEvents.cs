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
        OnSpawnedGameobjects,
        OnGameStartCounting,
        OnGameEndCounting,
        OnStartGameplay,
        FinishCounting,
        OnHitFinishLine,
        OnFinishGame,
        OnReplayBtnClicked,
        OnHitCheckpoint,
        #endregion

        #region UI


        #endregion
    }
}
