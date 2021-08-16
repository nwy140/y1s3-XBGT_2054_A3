using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCompHitboxInflictorRigidHandler : MonoBehaviour, ISupportComp
{
    #region Interface Related
    public UnitRefs _ownerUnitRefs;
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    #endregion Interface Related
    private List<Vector3> _collisionPoints = new List<Vector3>();

    public PropCompHitboxInflictorRigid hrModel;
    private PropCompHitboxInflictorRigid[] _hittableRigidsPool;
    public GameObject tempHitFX_Prefab;
    public void CollectCollisionPoint(Vector3 point)
    {
        _collisionPoints.Add(point);
        OnHitBoxCollisionPointDetected(point);  
    }

    public void OnHitBoxCollisionPointDetected(Vector3 point)
    {
        Instantiate(tempHitFX_Prefab, point, Quaternion.identity);
    }

    public void ClearCollisionPointList()
    {
        _collisionPoints.Clear();
    }

    private void Start()
    {
        hrModel.hitboxRigidHandler = this;
    }

    public void InitializePool(int poolSize)
    {
        _hittableRigidsPool = new PropCompHitboxInflictorRigid[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            _hittableRigidsPool[i] = Instantiate(hrModel, hrModel.transform.position, hrModel.transform.rotation);
            _hittableRigidsPool[i].transform.localScale = hrModel.transform.lossyScale;
            //_hittableRigidsPool[i] = Instantiate(hrModel,hrModel.transform.parent);
            //_hittableRigidsPool[i].transform.position = Vector3.zero;
            //_hittableRigidsPool[i].transform.localRotation = Quaternion.identity;
            //_hittableRigidsPool[i].transform.localScale = Vector3.one;

            _hittableRigidsPool[i].hitboxRigidHandler = this;
            _hittableRigidsPool[i].gameObject.SetActive(false);
        }
    }
    public void ActivateHittableRigid(Vector3 position, Quaternion rotation)
    {
        foreach (PropCompHitboxInflictorRigid hittableRigid in _hittableRigidsPool)
        {
            if (hittableRigid.isActiveAndEnabled == false)
            {
                hittableRigid.gameObject.SetActive(true);
                hittableRigid.transform.position = position;
                hittableRigid.transform.rotation = rotation;
                break;
            }
        }
    }

    public void DeActivateHittableRigid(Vector3 position, Quaternion rotation)
    {
        foreach (PropCompHitboxInflictorRigid hittableRigid in _hittableRigidsPool)
        {
            if (hittableRigid.isActiveAndEnabled == true)
            {
                hittableRigid.gameObject.SetActive(false);
                hittableRigid.transform.position = position;
                hittableRigid.transform.rotation = rotation;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in _collisionPoints)
        {
            Gizmos.DrawWireSphere(point, 0.05f);
        }
    }
}
