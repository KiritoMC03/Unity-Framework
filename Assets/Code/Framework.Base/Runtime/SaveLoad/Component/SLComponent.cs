using UnityEngine;

namespace Framework.Base.SaveLoad
{
    public class SLComponent : MonoBehaviour
    {
        #region Fields

        private static ISaveLoadSystem instance;

        #endregion


        #region Properties

        public static ISaveLoadSystem Instance
        {
            get
            {
                if (instance == null) Init();

                return instance;
            }
        }

        #endregion


        #region Methods

        private static void Init()
        {
            instance = new SaveLoadSystem();
        }

        #endregion
    }
}