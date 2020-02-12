using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PoolManager : Singleton<PoolManager>
    {
        private Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

                //Criação do parent para os objetos inseridos na pool
                GameObject poolHolder = new GameObject(prefab.name + "_Pool");
                poolHolder.transform.parent = transform;

                for (int i = 0; i < poolSize; i++)
                {
                    ObjectInstance newObject = new ObjectInstance(Instantiate(prefab));
                    poolDictionary[poolKey].Enqueue(newObject);
                    newObject.SetParent(poolHolder.transform);
                }
            }
        }

        public ObjectInstance ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation);
                
                return objectToReuse;
            }

            return null;
        }

        public class ObjectInstance
        {
            public GameObject GameObject;
            
            private Transform transform;
            private bool hasPoolObjectComponent;
            private PoolObject poolObject;

            public ObjectInstance(GameObject objectInstance)
            {
                GameObject = objectInstance;
                transform = GameObject.transform;
                GameObject.SetActive(false);

                if (GameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectComponent = true;
                    poolObject = GameObject.GetComponent<PoolObject>();
                }
            }

            public void Reuse(Vector3 position, Quaternion rotation)
            {
                GameObject.SetActive(true);
                transform.position = position;
                transform.rotation = rotation;

                if (hasPoolObjectComponent)
                {
                    poolObject.OnObjectReuse();
                }
            }

            public void SetParent(Transform parent)
            {
                transform.parent = parent;
            }
        }
    }
}