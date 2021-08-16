
    public enum ECharMovesetState
    {
        None,
        regularMotion,
        // Isolated Moves are moves that will prevent other moves from executing until the isolated move have ended.
        isolatedMotion,
        isolatedAttack
    }

    public enum ECharMovesetName
    {
        /// Redundant
        //    Move,
        // GroundJump,
        // GroundSprint,
        /// General
        /// Ground
        None,

        #region Motion

        groundEvade,

        #endregion Motion

        #region Attack

        groundAtkComboA1,
        groundAtkComboA2,
        groundAtkComboA3,

        #endregion Attack

        /// Air

        #region Motion

        airJump,
        airEvade,

        #endregion Motion

        #region Attack

        airAtkComboA1,
        airAtkComboA2,
        airAtkComboA3,

        #endregion Attack
    }

    public enum ECharAnimParam
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
    } 