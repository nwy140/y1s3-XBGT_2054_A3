using System.Collections;
using UnityEngine;

public class AbilityCompMotionGroundEvade2D : AbilityBaseComp
{
    [Header("AbilityComp")]
    public float evadeAcelSpeed = 1000f;
    public AnimationCurve evadeAnimCurve;
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.GroundEvade2D;
        base.Awake();
        desc = "Evade";
        devComment = "Evade";
        rejectedAnimBoolParamStateNames.Add(nameof(EUnitAnimParamNames.isActionLocked));
    }

    public float originalVelocitySize;
    public Quaternion originalRot;
    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        originalVelocitySize = _ownerUnitRefs._unitCharacterController._rigidbody2D.velocity.magnitude;
        originalRot = _ownerUnitRefs.transform.rotation;
        _ownerUnitRefs._unitCharacterController._rigidbody2D.velocity = Vector2.zero;
        // same for both Ai and Player
        //if (buttonDown)
        //{
        //    _ownerUnitRefs._unitCharacterController._rigidbody2D.AddRelativeForce(evadeAcelSpeed * Vector2.up);
        //}
    }
    public override void OnAbilityActiveStay()
    {
        base.OnAbilityActiveStay();
        if (originalRot != Quaternion.identity)
        {
            _ownerUnitRefs.transform.rotation = originalRot;
        }

        _ownerUnitRefs._unitCharacterController._rigidbody2D.AddRelativeForce(evadeAcelSpeed * Vector2.up * Time.deltaTime);
    }
    public override void OnAbilityActiveExit()
    {
        base.OnAbilityActiveExit();
        if (isEvadeAnimCurveActive == false)
        {
            isEvadeAnimCurveActive = true;
            StartCoroutine(AbilityWithAnimCurve());
        }
        originalRot = Quaternion.identity;
    }

    public float elpasedTime = 0f;
    public bool isEvadeAnimCurveActive;
    IEnumerator AbilityWithAnimCurve()
    {
        var duration = activeDuration;
        while (elpasedTime < duration)
        {
            elpasedTime += Time.deltaTime;
            float strength = evadeAnimCurve.Evaluate(elpasedTime / duration);
            _ownerUnitRefs._unitCharacterController._rigidbody2D.AddRelativeForce(evadeAcelSpeed * Vector2.up * strength);

            isEvadeAnimCurveActive = false;
            yield return null;
        }
    }
    //public override IEnumerator OnAbilityStarted()
    //{
    //    AbilityFunctionality();
    //    return base.OnAbilityStarted();
    //}
    //public override IEnumerator OnAbilityUpdating()
    //{
    //    AbilityFunctionality();

    //    return base.OnAbilityUpdating();
    //}
}