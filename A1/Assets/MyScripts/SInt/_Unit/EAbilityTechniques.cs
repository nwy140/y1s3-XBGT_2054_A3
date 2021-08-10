
// An Enum to store and access ability names
public enum EAbilityTechniques
{

    // Desc: Special Techniques

    // Character Class 001
     
    // General Abilties Set // Named the same as the Enums in EBtns
    #region General Abilities Set

    #region 0 : Default

    Empty,
    MoveHorizontal,
    MoveVertical,

    #endregion 0 : Default

    #region 1 : Motion

    GroundSprint,
    GroundJump,
    GroundEvade,
    //AirSprint,
    AirJump,
    AirEvade,

    #endregion 1 : Motion

    Interact,
    GroundRegularAttack,
    GroundChargedAttack,
    AirRegularAttack,

    #region 2 : Activity

    #endregion 2 : Activity

    #region 3 : Techniques

    #endregion 3 : Techniques

    #region 2 : Assist

    Pause,

    #endregion 2 : Assist

    #region 5 : Camera

    LockOn,
    LockOnCycle,
    Strafe,
    mouseLookX,
    mouseLookY,
    gamepadLookX,
    gamepadLookY,
    Recentre,

    #endregion 5 : Camera
    #endregion General Abilities Set


    #region 2D
    RegularAtkRange2D,
    RegularAtkAim,
    #endregion 2D

}


/*
using BehaviorDesigner.Runtime;
//Expose Shared Enum For Behavior Designer: https://www.opsive.com/forum/index.php?threads/enum-drop-down-list.896/
[System.Serializable]
public class SharedEAbilityTechniques : SharedVariable<EAbilityTechniques>
{
    public static implicit operator SharedEAbilityTechniques(EAbilityTechniques value) { return new SharedEAbilityTechniques { Value = value }; }
}
*/