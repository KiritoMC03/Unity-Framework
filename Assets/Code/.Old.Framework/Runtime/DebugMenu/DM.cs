using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General.DebugMenu
{
    public class DM : MonoBehaviour
    {
        private const string DMNotFound = "DM not found.";

        #region Fields

        private static DM instance;

#if !Il2CPP_DEBUG && UNITY_EDITOR
        private const string DebugMenuDeactivatedMessage =
            "DM does not work with this configuration. To work you must enable compilation mode <b> IL2CPP - DEBUG </b>";
#endif

        [SerializeField]
        private GameObject button;
        [SerializeField]
        private GameObject inputField;
        [SerializeField]
        private GameObject inputFieldVector;
        [SerializeField]
        private GameObject toggle;

        [SerializeField,Space] 
        private Transform content;


        [Header("Exit")]
        [SerializeField]
        private GameObject[] debugMenu;
        [SerializeField] 
        private Button exit;
        [Header("Enter")]
        [SerializeField]
        private GameObject enterMenu;
        [SerializeField] 
        private Button enter;

        private Dictionary<int, DMItem> items = new Dictionary<int, DMItem>();

        private static bool availableDebugMenu;

        [Space]
        public bool dontDestroyOnLoadScene = true;

        #endregion

        #region Properties

        public static DM Instance
        {
            get
            {
                if (instance == null)
                {
                    Init();
                }

                return instance;
            }
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
#if Il2CPP_DEBUG
            Init();
            if (dontDestroyOnLoadScene) DontDestroyOnLoad(this);
            exit.onClick.AddListener(ExitMenuState);
            enter.onClick.AddListener(EnterMenuState);
            RegisterDebugMenu("Change button transparency", ChangeTransparency,"Change",Color.yellow);
#else
            DeactivateMenu();
#if UNITY_EDITOR
            Debug.LogWarning(DebugMenuDeactivatedMessage);
#endif
#endif
        }

        #endregion

        #region Methods

        public int RegisterDebugMenu(string label, Action<Vector3> action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(inputFieldVector, content).GetComponent<DMItem>();
            dmItem.Init(label,action,buttonLabel,color);
            return RegisterDebugMenuInternal(dmItem);
        }
        
        public int RegisterDebugMenu(string label, Action<float> action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(inputField, content).GetComponent<DMItem>();
            dmItem.Init(label,action,buttonLabel,color);
            return RegisterDebugMenuInternal(dmItem);
        }
        public int RegisterDebugMenu(string label, Action<int> action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(inputField, content).GetComponent<DMItem>();
            dmItem.Init(label,action,buttonLabel,color);
            return RegisterDebugMenuInternal(dmItem);
        }
        
        public int RegisterDebugMenu(string label, Action<string> action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(inputField, content).GetComponent<DMItem>();
            dmItem.Init(label,action,buttonLabel,color);
            return RegisterDebugMenuInternal(dmItem);
        }
        
        public int RegisterDebugMenu(string label, Action<bool> action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(toggle, content).GetComponent<DMItem>();
            dmItem.Init(label, action, buttonLabel, color);
            return RegisterDebugMenuInternal(dmItem);
        }
        
        public int RegisterDebugMenu(string label, Action action, string buttonLabel = null, Color color = default)
        {
            if (!availableDebugMenu) return 0;

            var dmItem = Instantiate(button, content).GetComponent<DMItem>();
            dmItem.Init(label, action, buttonLabel, color);
            return RegisterDebugMenuInternal(dmItem);
        }

        public DMItem GetDMItem(in int id)
        {
            items.TryGetValue(id, out DMItem dmItem);
            if(dmItem is {}) Debug.Log($"Item not found in DM id:{id}");
            return dmItem;
        }

        public void ClearAll()
        {
            if (!availableDebugMenu) return;

            foreach (var item in items.Values)
            {
                item.Destroy();
            }
            items.Clear();
        }

        public void Clear(in int id)
        {
            if (!availableDebugMenu) return;

            if (items.TryGetValue(id, out DMItem dmItem))
            {
                items[id].Destroy();
                items.Remove(id);
            }
        }

        private void ExitMenuState()
        {
            enterMenu.SetActive(true);
            foreach (var item in debugMenu)
                item.SetActive(false);
        }

        private void EnterMenuState()
        {
            enterMenu.SetActive(false);
            foreach (var item in debugMenu)
                item.SetActive(true);
        }

        private void DeactivateMenu()
        {
            enterMenu.SetActive(false);
            foreach (var item in debugMenu)
                item.SetActive(false);
        }

        private int RegisterDebugMenuInternal(in DMItem dmItem)
        {
            var id = dmItem.gameObject.GetInstanceID();
            items.Add(id, dmItem);
            return id;
        }

        private void ChangeTransparency()
        {
            var image = enterMenu.GetComponent<Image>();
            image.color = !Mathf.Approximately(image.color.a, 1) ? 
                new Color(image.color.r, image.color.g, image.color.b, 1):
                new Color(image.color.r, image.color.g, image.color.b, 0);
        }

        private static void Init()
        {
            instance = FindObjectOfType<DM>();
            if (instance is null) Debug.LogWarning(DMNotFound);
#if Il2CPP_DEBUG
            availableDebugMenu = true;
#else
            availableDebugMenu = false; 
#endif
        }

        #endregion
    }
}
