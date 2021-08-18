public enum EAbilityUsageState
{
    ready,
    active,
    cooldown
    //ready, // allow start // frame where ability is ready to be called
    //finish, // the final frame where the ability finishes without interruption
    //started, // the first frame where the ability starts
    //updating, // Ability Update each frame 
    //interrupted, // the frame where the ability gets interrupted
}