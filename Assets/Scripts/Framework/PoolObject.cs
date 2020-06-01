using System;
using UnityEngine;

namespace Framework
{
    public class PoolObject : MonoBehaviour
    {
        public virtual void OnObjectReuse()
        {
            
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
