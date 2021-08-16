using System.Collections.Generic;
using UnityEngine;

namespace MyScripts.SInt.Character.CharComp.AI
{
    // Ref: https://youtu.be/BYr6sAGpG3Y?list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
    [System.Serializable]
    public class AIMemory
    {
        public float Age
        {
            get { return Time.time - lastSeen; }
        }

        public GameObject gameObject;
        public Vector3 position;
        public Vector3 direction;
        public float distance;
        public float angle;
        public float lastSeen;
        public float score;


        // scores
        [Header("Scores Preview Only")]
        public float distanceScore;
        public float angleScore;
        public float ageScore;
    }
    [System.Serializable]
    public class CharCompAISensoryMemory
    {
        public List<AIMemory> memories = new List<AIMemory>();
        private GameObject[] characters;

        public CharCompAISensoryMemory(int maxDetected)
        {
            characters = new GameObject[maxDetected];
        }


        public void UpdateSenses(CharCompAISensor sensor, LayerMask layermask)
        {
            int targets = sensor.Filter(characters, layermask);

            for (int i = 0; i < targets; ++i)
            {
                GameObject target = characters[i];
                RefreshMemory(sensor.originPoint.gameObject, target);
            }
        }

        public void UpdateSenses(CharCompAISensor sensor)
        {
            int targets = sensor.Filter(characters, "Character")
                ;
            for (int i = 0; i < targets; ++i)
            {
                GameObject target = characters[i];
                RefreshMemory(sensor.originPoint.gameObject, target);
            }
        }

        public void RefreshMemory(GameObject agent, GameObject target)
        {
            AIMemory memory = FetchMemory(target);
            memory.gameObject = target;
            memory.position = target.transform.position;
            memory.direction = target.transform.position - agent.transform.position;
            memory.distance = memory.direction.magnitude;
            memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
            memory.lastSeen = Time.time;
        }

        public AIMemory FetchMemory(GameObject gameObject)
        {
            AIMemory memory = memories.Find(x => x.gameObject == gameObject);
            if (memory == null)
            {
                memory = new AIMemory();
                memories.Add(memory);
            }

            return memory;
        }

        public void ForgetMemories(float olderThan)
        {
            memories.RemoveAll(m => m.Age > olderThan);
            memories.RemoveAll(m => !m.gameObject); // remove objects that is null
            // memories.RemoveAll(m => m.gameObject.GetComponent<HP>().isDead == true);
        }
    }
}