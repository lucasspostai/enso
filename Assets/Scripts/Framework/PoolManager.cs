using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PoolManager : MonoBehaviour
    {
        private Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        private static PoolManager instance;

        public static PoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PoolManager>();
                }

                return instance;
            }
        }

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

                //Criação do parent para os objetos inseridos na pool
                GameObject poolHolder = new GameObject((prefab.name + "_Pool"));
                poolHolder.transform.parent = transform;

                for (int i = 0; i < poolSize; i++)
                {
                    ObjectInstance newObject = new ObjectInstance(Instantiate(prefab));
                    poolDictionary[poolKey].Enqueue(newObject);
                    newObject.SetParent(poolHolder.transform);
                }
            }
        }

        public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation);
            }
        }

        public class ObjectInstance
        {
            private GameObject gameObject;
            private Transform transform;
            private bool hasPoolObjectComponent;
            private PoolObject poolObject;

            public ObjectInstance(GameObject objectInstance)
            {
                gameObject = objectInstance;
                transform = gameObject.transform;
                gameObject.SetActive(false);

                if (gameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectComponent = true;
                    poolObject = gameObject.GetComponent<PoolObject>();
                }
            }

            public void Reuse(Vector3 position, Quaternion rotation)
            {
                gameObject.SetActive(true);
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