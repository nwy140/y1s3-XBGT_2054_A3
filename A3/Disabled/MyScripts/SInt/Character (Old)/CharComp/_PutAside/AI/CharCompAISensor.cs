using System;
using System.Collections.Generic;
using UnityEngine;


// Ref: https://www.youtube.com/watch?app=desktop&v=znZXmmyBF-o&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy&index=8
namespace MyScripts.SInt.Character.CharComp.AI
{
    //[ExecuteInEditMode]
    public class CharCompAISensor : MonoBehaviour
    {
        public float distance = 10;
        public float angle = 30;
        public float height = 1.0f;
        public Color meshColor = Color.red;

        public int scanFrequency = 30;
        public LayerMask layers;
        public Color sensedRaycastColor;

        public LayerMask occlusionLayers; // layers that block line of sight i.e walls
        public float occlusionCastOffsetHeight;
        public Color occlusionCastColor;

        //[Header("SphereCastDetect - Extra")]
        //public bool isPerformExtraSphereCast;
        //public float extraSphereCastRadius = 1.0f; // extra sphere cast to detect what's right in front of the character as a failsafe if this AISensor is inaccurate

        public bool isUseAllowedOnlyTags;

        public List<String> allowedOnlyTags;
        public List<String> ignoreTags;
        public List<GameObject> ignoreObjs;

        [Header("EyeSight Transforms")]
        public Transform originPoint;  // where the eye is , where the raycast begins shooting 
        public Transform desPoint; // where the destination target is , where the raycast ends 
        public Transform fieldOfViewPivot; // the pivot of the sight field of view cone shape detection
        public bool isAllowDebug;

        public List<GameObject> Objects
        {
            get
            {
                objects.RemoveAll(obj => !obj);
                return objects;
            }
        }

        private List<GameObject> objects = new List<GameObject>();

        public Collider[] colliders = new Collider[50];
        private Mesh mesh;
        private int count;
        private float scanInterval;
        private float scanTimer;

        public RaycastHit raycastHit;
        void Start()
        {
            scanInterval = 1.0f / scanFrequency;
        }

        void FixedUpdate()
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0)
            {
                scanTimer += scanInterval;
                Scan();
            }
        }

        public GameObject currObjToScan;
        private void Scan()
        {
            count = Physics.OverlapSphereNonAlloc(fieldOfViewPivot.transform.position, distance, colliders, layers
                , QueryTriggerInteraction.UseGlobal);

            objects.Clear();
            for (int i = 0; i < count; ++i)
            {
                //GameObject obj = colliders[i].gameObject;
                currObjToScan = colliders[i].gameObject;
                GameObject checkSightPoint = currObjToScan;



                if (IsInSight(currObjToScan) == true)
                {
                    objects.Add(currObjToScan);
                }
            }
        }

        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = originPoint.position;
            Vector3 dest = obj.transform.position;
            if (obj.layer == LayerMask.NameToLayer("Character"))
            {
                if (obj.transform.childCount > 0)
                {
                    // TODO optional, for loop iterate over each bone in character mesh via final ik // if require accuracy and realism
                    if (obj.transform.GetChild(0).CompareTag("TargetOffsetPoint"))
                    {
                        dest = obj.transform.GetChild(0).position; // Get Target Offset
                    }
                }
            }
            Vector3 directionToFOVPivot = dest - fieldOfViewPivot.transform.position;
            if (directionToFOVPivot.y < 0 || directionToFOVPivot.y > height)
            {
                return false;
            }
            Vector3 directionToOrigin = dest - origin;
            directionToFOVPivot.y = 0;
            float deltaAngle = Vector3.Angle(directionToOrigin, originPoint.transform.forward);
            if (deltaAngle > angle)
            {
                return false;
            }

            //origin.y += height / 2;
            //dest.y = origin.y;
            origin.y = origin.y + occlusionCastOffsetHeight;
            //Debug.Log("origin: " + origin + "destL: " + dest);
            // Ignore any with condition
            if (ignoreTags.Contains(obj.tag))
            {
                return false;
            }
            if (ignoreObjs.Contains(obj))
            {
                return false;
            }

            // Allow Only with specific tag
            if (isUseAllowedOnlyTags)
            {
                if (allowedOnlyTags.Contains(obj.tag))
                {
                    return true;
                }
            }

            // Perform linecast to see if a wall/occlusion layer is blocking our sight
            if (Physics.Linecast(origin, dest, occlusionLayers))
            {
                if (isAllowDebug)
                {
                    Debug.DrawRay(origin, Vector3Common.GetDirectionFrom2Pos(origin, dest), occlusionCastColor, 2, false);

                }

                //print("Hit");
                return false;
            }
            else
            {
                if (isAllowDebug)
                {
                    Debug.DrawLine(origin, dest, sensedRaycastColor, 2, false);

                }
                desPoint.transform.position = obj.transform.position;
            }


            // Perform an extra spherecast in case our AI Sensor is inaccurate
            //if (isPerformExtraSphereCast == true)
            //{
            //    if (Physics.SphereCast(originPoint.transform.position, extraSphereCastRadius,originPoint.transform.forward, //Vector3Common.GetDirectionFrom2Pos(originPoint.transform.position, dest),
            //        out raycastHit, distance, layers, QueryTriggerInteraction.UseGlobal))
            //    {
            //        if (isAllowDebug)
            //        {
            //            //Debug.DrawRay(originPoint.transform.position, direction, Color.cyan, 2, false);
            //            RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugCapsule(originPoint.transform.position, dest, Color.black, extraSphereCastRadius);
            //        }
            //        desPoint.transform.position = raycastHit.point;
            //        if (raycastHit.collider.gameObject != obj)
            //        {
            //            currObjToScan = raycastHit.collider.gameObject;
            //            obj = currObjToScan;
            //        }
            //    }
            //}


            return true;
        }

        Mesh CreateWedgeMesh()
        {
            Mesh wedgeMesh = new Mesh();

            int segments = 10;
            int numTriangles = (segments * 4) + 2 + 2;
            // int numTriangles = 8;
            int numVertices = numTriangles * 3;

            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;

            int vert = 0;
            // left side 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            // right side 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;
            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segments;
            for (int i = 0; i < segments; ++i)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

                topRight = bottomRight + Vector3.up * height;
                topLeft = bottomLeft + Vector3.up * height;

                // far side 
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                // top  
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;


                // bottom 
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;


                currentAngle += deltaAngle;
            }

            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            wedgeMesh.vertices = vertices;
            wedgeMesh.triangles = triangles;
            wedgeMesh.RecalculateNormals();

            return wedgeMesh;
        }

        // Monobehaviour,
        // this event is called when the script is loaded or a value is changed in the Inspector (Called in the editor only).
        private void OnValidate()
        {
            mesh = CreateWedgeMesh();
            scanInterval = 1.0f / scanFrequency;
        }

        private void OnDrawGizmos()
        {
            if (isAllowDebug == false)
            {
                return;
            }
            if (mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, fieldOfViewPivot.transform.position, fieldOfViewPivot.transform.rotation);
            }

            Gizmos.DrawWireSphere(fieldOfViewPivot.transform.position, distance);
            for (int i = 0; i < count; ++i)
            {
                Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
            }

            Gizmos.color = Color.green;
            foreach (var obj in objects)
            {
                Gizmos.DrawSphere(obj.transform.position, 0.2f);
            }
        }

        public int Filter(GameObject[] buffer, LayerMask layermask)
        {
            int count = 0;
            foreach (var obj in Objects)
            {
                // check if specific layer is in Layermask: https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
                if (layermask == (layermask | (1 << obj.layer)))
                {
                    buffer[count++] = obj;
                }

                if (buffer.Length == count)
                {
                    break;
                }
            }

            return count;
        }

        public int Filter(GameObject[] buffer, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            int count = 0;
            foreach (var obj in Objects)
            {
                // check if specific layer is in Layermask: https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
                if (obj.layer == layer)
                {
                    buffer[count++] = obj;
                }

                if (buffer.Length == count)
                {
                    break;
                }
            }

            return count;
        }

    }
}
