

// Similar to the movement mode Enum In Unreal Engine:
// https://docs.unrealengine.com/4.26/en-US/API/Runtime/Engine/GameFramework/UCharacterMovementComponent/MovementMode/
public enum EUnitMovementMode
{
    movingOnGround = 0, //: Walking on a surface, under the effects of friction, and able to "step up" barriers. Vertical velocity is zero.
    fallingOnAir = 1, //: Falling under the effects of gravity, after jumping or walking off the edge of a surface.
    flying = 2, //: Flying, ignoring the effects of gravity.
    swimming = 3, //: Swimming through a fluid volume, under the effects of gravity and buoyancy.
    custom = 4, // : User-defined custom movement mode, including many possible sub-modes. This is automatically replicated through the Character owner and for client-server movement functions.
}



// Similar to Unreal's possesion : https://docs.unrealengine.com/4.26/en-US/InteractiveExperiences/HowTo/PossessPawns/
// Enum that represents who's currently possesing the unit, AI or Plauer
public enum EUnitPossesionType
{
    ai, 
    player,
    //p2,
    //p3,
    //p4,
}

/*
using BehaviorDesigner.Runtime;
public class SharedEUnitPossesionType: SharedVariable<EUnitPossesionType>
{
    public static implicit operator SharedEUnitPossesionType(EUnitPossesionType value) { return new SharedEUnitPossesionType { Value = value }; }
}
*/

public enum EUnitAnimParamNames
{
    // boolean
    CanAttack,
    isActionLocked,
    // specific Actions
    isEvading,
    // Variables
    isInstigateDamageOn,
    // trigger
    Attack,
    // integer
    AttackType,
    movementMode,
}