
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyScripts.SInt.Character.CharComp.AI
{
    [ExecuteInEditMode]
    public class CharCompAITargetingSystem : MonoBehaviour
    {
        public float memorySpan = 3.0f;
        public float distanceWeight = 1.0f;
        public float angleWeight = 1.0f;
        public float ageWeight = 1.0f;
        public CharCompAISensoryMemory memory = new CharCompAISensoryMemory(10);
        public CharCompAISensor sensor;
        [SerializeField] private AIMemory bestMemory; // the memory obj with highest score will be your best target obj

        public GameObject selfCharRootObj;
        public UnityEvent OnHasTargetEvent;
        public UnityEvent OnLoseTargetEvent;

        public bool HasTarget
        {
            get { return bestMemory != null; }
        }

        public GameObject Target
        {
            get { return bestMemory.gameObject; }
        }

        public Vector3 TargetPosition
        {
            get { return bestMemory.gameObject.transform.position; }
        }

        public bool TargetInSight
        {
            get
            {
                return bestMemory.Age < 0.5f; //seconds} 
            }
        }

        public float TargetDistance
        {
            get { return bestMemory.distance; }
        }

        private void Start()
        {
            sensor = GetComponent<CharCompAISensor>();
        }

        private void Update()
        {
            if (memory != null)
            {
                // Line Of Sight Field Of View Check
                memory.UpdateSenses(sensor, sensor.layers);
                // forget targets that have left  line of sight for than 3 seconds
                memory.ForgetMemories(memorySpan);

                EvaluateScores();
            }
        }

        private bool isAllowOnLoseTargetEventInvoke = false;
        public RaycastHit raycastHit;
        //  Evaluate Score Priority,
        void EvaluateScores()
        {
            //bestMemory = null;
            bool isBestMemoryFound = false;
            foreach (var memory in memory.memories.ToArray())
            {
                memory.score = CalculateScore(memory);
                if (isBestMemoryFound == false || memory.score > bestMemory.score)
                {
                    if (memory.gameObject != selfCharRootObj)
                    {

                        bestMemory = memory;
                        isBestMemoryFound = true;
                    }
                }
            }
            if (isBestMemoryFound == false && memory.memories.Count != 1)
            {
                bestMemory = null;
            }
            if (HasTarget == true)
            {
                OnHasTargetEvent.Invoke();
                isAllowOnLoseTargetEventInvoke = true;
            }
            else if (isAllowOnLoseTargetEventInvoke == true)
            {
                OnLoseTargetEvent.Invoke();
                isAllowOnLoseTargetEventInvoke = false;
            }
        }

        float Normalize(float value, float maxValue)
        {
            return 1.0f - (value / maxValue);
        }

        float CalculateScore(AIMemory memory)
        {
            // distance to obj detected that is still stored in memory
            memory.distanceScore = Normalize(memory.distance, sensor.distance) * distanceWeight;
            memory.angleScore =  (Normalize(memory.angle, sensor.angle)) * angleWeight;
            memory.ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
            return memory.distanceScore + memory.angleScore + memory.ageScore;
        }

        private void OnDrawGizmos()
        {
            //https://youtu.be/BYr6sAGpG3Y?list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy&t=770
            float maxScore = float.MinValue;
            if (memory != null)
            {
                foreach (var memory in memory.memories)
                {
                    maxScore = Mathf.Max(maxScore, memory.score);
                }

                foreach (var memory in memory.memories)
                {
                    Color color = Color.red;
                    if (memory == bestMemory)
                    {
                        color = Color.yellow;
                    }

                    color.a = memory.score / maxScore;
                    Gizmos.color = color;
                    Gizmos.DrawSphere(memory.position, 0.4f);
                }
            }
        }
    }
}