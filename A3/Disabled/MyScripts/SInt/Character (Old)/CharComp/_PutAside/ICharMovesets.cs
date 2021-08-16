
using Rewired;

namespace MyScripts.SInt.Character
{
    public interface ICharMovesets
    {
        // Please see link for the list of moves
        // https://docs.google.com/document/d/1HGl2VWe2WO3Wv1X3tET8ymVM_4GU11_Q-2J0vo-QfyY/edit#heading=h.1i8r60md8wmy
        #region Rewired Input System Use

        public void OnRegularAttack(InputActionEventData data);
        public void OnChargedAttack(InputActionEventData data);

        public void OnGeneralLockOn(InputActionEventData data);

        #endregion Rewired Input System Use

        #region Regular Use

        public void OnGeneralLockOn();
        #endregion Regular Use
    }
}