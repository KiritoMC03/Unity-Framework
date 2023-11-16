using UnityEngine;

namespace GameKit.CraftModule.Resource
{
    public class Resource : MonoBehaviour, IBaseResource
    {
        #region Fields

        [SerializeField]
        private GameKit.CraftModule.Resource.ResourceType type;

        private GameObject cacheGameObject;
        private Transform cacheTransform;

        #endregion
        
        #region Properties IBaseResource

        public GameKit.CraftModule.Resource.ResourceType Type => type;

        public GameObject CacheGameObject
        {
            get
            {
                if (cacheGameObject == null)
                {
                    cacheGameObject = gameObject;
                }

                return cacheGameObject;
            }
        }

        public Transform CacheTransform
        {
            get
            {
                if (cacheTransform == null)
                {
                    cacheTransform = transform;
                }

                return cacheTransform;
            }
        }

        #endregion
    }
}