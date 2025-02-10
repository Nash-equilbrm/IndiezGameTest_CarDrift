using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public enum EventID
    {
        #region Gameplay
        InitBlockSpanwer,   // For Jump Dash
        PlayerFinishMovement, // For Jump Dash
        GainPoint,
        HitBlock, // For Jump Dash
        BackToMainMenu,
        FinishLoadingScene,
        ExitMiniGame, // Notify that minigame is finish
        UnloadMiniGame, // Actually unload minigame and free memory

        PlayerFinishLaunch, // For Knife Hit
        HitObstacle, // For Knife Hit
        InitTarget, // For Knife Hit
        HitTarget,
        #endregion

        #region UI
        OnPlayBtnClicked,
        OnBackToMainMenuBtnClicked,
        OnContinueClicked,
        BtnClicked,
        ThrowKnife,
        Jump,
        LoseGame,
        ContinueGame,
        TargetExplode,

        #endregion
    }
}
