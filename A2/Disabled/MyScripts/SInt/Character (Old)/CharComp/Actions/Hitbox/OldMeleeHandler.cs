using MyScripts.SInt.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

//ref: https://www.youtube.com/watch?v=r3UsE7PfzOI&list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&index=3
public class OldMeleeHandler : MonoBehaviour
{ // Old Melee Handler script
    public struct BufferObj
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 lossyScale;
        public Vector3 size; // box collider size
    }

    public LayerMask hitLayers;
    public bool isDebugTrail = false;
    LinkedList<BufferObj> _trailList = new LinkedList<BufferObj>();
    LinkedList<BufferObj> _trailFillerList = new LinkedList<BufferObj>();
    public CharRefs _cr;
    public GameObject weaponobj;
    public BoxCollider _weaponCol;
    public float _minWeaponLengthRange;
    int maxFrameBuffer = 2;

    int attackId = 0; // unique id for every attack call

    private SupportCompHitboxInflictorRigidHandler _hittableRigidHandler;

    private void Start()
    {
        if (_weaponCol == null)
        {
            _weaponCol = (BoxCollider)weaponobj.GetComponent<Collider>();
        }
        _hittableRigidHandler = GetComponent<SupportCompHitboxInflictorRigidHandler>();
        //_hittableRigidHandler.InitializePool(maxFrameBuffer + 1);
        _hittableRigidHandler.InitializePool(8);
        //Ref: https://youtu.be/OMau8bhENp4?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&t=710
    }
    private void LateUpdate()
    {
        //if (isDebugTrail)
        if (_cr._anim.GetBool(nameof(ECharAnimParam.isInstigateDamageOn)))
        {
            CheckTrail();
        }

    }
    private void CheckTrail()
    {
        BufferObj bo = new BufferObj();
        bo.size = _weaponCol.size;
        bo.rotation = _weaponCol.transform.rotation;
        bo.position = _weaponCol.transform.position + _weaponCol.transform.TransformVector(_weaponCol.center); // add collider offset
        bo.lossyScale = weaponobj.transform.lossyScale;
        _trailList.AddFirst(bo);
        if (_trailList.Count > maxFrameBuffer)
        {
            _trailList.RemoveLast();
        }
        if (_trailList.Count > 1)
        {
            _trailFillerList = FillTrail(_trailList.First.Value, _trailList.Last.Value);
        }

        //Check Colliders Hit
        Collider[] hits = Physics.OverlapBox(bo.position, bo.size / 2, bo.rotation, hitLayers, QueryTriggerInteraction.Ignore);
        Dictionary<long, Collider> colliderList = new Dictionary<long, Collider>();
        CollectColliders(bo, hits, colliderList);

        //if (hits.Length > 0)
        //{
        //    Debug.Log(hits[0].tag);
        //}
        foreach (BufferObj cbo in _trailFillerList)
        {
            hits = Physics.OverlapBox(cbo.position, cbo.size / 2, cbo.rotation, hitLayers, QueryTriggerInteraction.Ignore);
            CollectColliders(cbo, hits, colliderList);
            //if (hits.Length > 0)
            //{
            //    Debug.Log("Trial Hits" + hits[0].tag);
            //}
        }
        foreach (Collider col in colliderList.Values)
        {
            // send message 
            HitData hd = new HitData();
            hd.id = attackId;
            HitboxReceiver hittable = col.GetComponent<HitboxReceiver>();
            if (hittable != null)
            {
                hittable.Hit(hd);
                // Pass in instigator instead, better idea
            }
        }
    }

    // Ref: https://youtu.be/r3UsE7PfzOI?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&t=1013
    private void CollectColliders(BufferObj source, Collider[] hits, Dictionary<long, Collider> colliderList)
    {
        // if we hit, we active the rigidbody collider in the same position/rotation
        if (hits.Length > 0)
        {
            _hittableRigidHandler.ActivateHittableRigid(source.position, source.rotation);
        }

        for (int i = 0; i < hits.Length; i++)
        {
            if (colliderList.ContainsKey(hits[i].GetInstanceID()) == false) // prevemt duplicate dictionary jet errir
            {
                colliderList.Add(hits[i].GetInstanceID(), hits[i]);
            }
        }
    }

    private LinkedList<BufferObj> FillTrail(BufferObj from, BufferObj to)
    {
        LinkedList<BufferObj> fillerList = new LinkedList<BufferObj>();
        float distance = Mathf.Abs((from.position - to.position).magnitude);

        if (distance > _minWeaponLengthRange/*_weaponCol.size.z*/)
        {
            float steps = Mathf.Ceil(distance / _weaponCol.size.z);
            float stepsAmount = 1 / (steps + 1);
            float stepsValue = 0;
            for (int i = 0; i < (int)steps; i++)
            {
                stepsValue += stepsAmount;
                BufferObj tmpBo = new BufferObj();
                tmpBo.position = Vector3.Lerp(from.position, to.position, stepsValue);
                tmpBo.rotation = Quaternion.Lerp(from.rotation, to.rotation, stepsValue);
                tmpBo.lossyScale = _weaponCol.transform.lossyScale;
                tmpBo.size = _weaponCol.size;
                fillerList.AddFirst(tmpBo);
            }

        }
        return fillerList;
    }
    private void OnDrawGizmos()
    {
        if (isDebugTrail == false)
        {
            return;
        }

        //bool isFirstRound = true;
        //BufferObj lastBo = new BufferObj();
        foreach (BufferObj bo in _trailList)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(bo.position, bo.rotation, bo.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, bo.size); // relative to matrix above
            //if (isFirstRound == false)
            //{
            //    LinkedList<BufferObj> calculated = FillTrail(bo, lastBo);
            //    foreach (BufferObj cbo in _trailList)
            //    {

            //        Gizmos.color = Color.yellow;
            //        Gizmos.matrix = Matrix4x4.TRS(cbo.position, cbo.rotation, cbo.lossyScale);
            //        Gizmos.DrawWireCube(Vector3.zero, cbo.size); // relative to matrix above
            //    }
            //}
            //isFirstRound = false;
            //lastBo = bo;
        }
        foreach (BufferObj bo in _trailFillerList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(bo.position, bo.rotation, bo.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, bo.size); // relative to matrix above
        }
    }

    #region Action Wrapper
    // TODO: Refactor and Redesign interface architecture
    #region Regular Use
    public void OnAttack(int attackType)
    {
        // Validate movement mode, then decide whether to call by air or ground
        if (_cr.currMovementMode == EUnitMovementMode.movingOnGround)
        {
            OnGroundRegularAttack(attackType);
        }
        else if (_cr.currMovementMode == EUnitMovementMode.fallingOnAir)
        {
            OnAirRegularAttack(attackType);
        }
    }

    public void OnAttack_SetAnimParam(int attackType)
    {
        if (_cr._anim.GetBool(nameof(ECharAnimParam.CanAttack)))
        {
            attackId++;
            _cr._anim.SetTrigger(nameof(ECharAnimParam.Attack));
            _cr._anim.SetInteger(nameof(ECharAnimParam.AttackType), attackType);
            _hittableRigidHandler.ClearCollisionPointList();
        }
    }
    public void OnGroundRegularAttack(int attackType)
    {
        if (_cr._anim.GetBool(nameof(ECharAnimParam.isEvading)))
        {
            return;
        }
        OnAttack_SetAnimParam(attackType);
        //Debug.Log("Atk");
    }

    public void OnAirRegularAttack(int attackType)
    {

    }


    #endregion Regular Use
    #endregion Action Wrapper
}
