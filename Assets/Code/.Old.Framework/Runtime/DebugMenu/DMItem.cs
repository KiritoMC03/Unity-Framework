using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General.DebugMenu
{
    public class DMItem : MonoBehaviour
    {
        public Text labelText;
        public bool switchInputField;
        [ShowIf("switchInputField")]
        public InputField inputField;
        [Header("Vector3")] 
        public bool switchInputFieldVector;
        [ShowIf("switchInputFieldVector")]
        public InputField inputFieldX;
        [ShowIf("switchInputFieldVector")]
        public InputField inputFieldY;
        [ShowIf("switchInputFieldVector")]
        public InputField inputFieldZ;
        [Header("Toggle")] 
        public bool switchToggle;
        [ShowIf("switchToggle")]
        public Toggle toggle;
        [Header("Button")]
        public Button button;
        public Text buttonLabel;
        [Header("Panel")] 
        public Image panelColor;

        
        
        public void Init(string label, Action<int> action, string buttonLabel = null, Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() =>
            {
                int.TryParse(inputField.text, out int result);
                action?.Invoke(result);
            }));
        }
        
        public void Init(string label, Action<float> action, string buttonLabel = null, Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() =>
            {
                float.TryParse(inputField.text, out float result);
                action?.Invoke(result);
            }));
            
        }

        public void Init(string label, Action<string> action, string buttonLabel = null, Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() => action?.Invoke(inputField.text)));
        }
        
        public void Init(string label, Action<bool> action, string buttonLabel = null, Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() => action?.Invoke(toggle.isOn)));
        }
        
        public void Init(string label, Action<Vector3> action, string buttonLabel = null, Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() =>
            {
                float.TryParse(inputFieldX.text, out float X);
                float.TryParse(inputFieldY.text, out float Y);
                float.TryParse(inputFieldZ.text, out float Z);
                action?.Invoke(new Vector3(X, Y, Z));
            }));
        }
        
        public void Init(string label, Action action, string buttonLabel = null, in Color color = default)
        {
            InitInternal(label,buttonLabel,color);
            
            button.onClick.AddListener((() => action?.Invoke()));
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void InitInternal(in string label,in string buttonLabel = null, in Color color = default)
        {
            labelText.text = label;
            if (!string.IsNullOrEmpty(buttonLabel)) this.buttonLabel.text = buttonLabel;

            if(color != default) panelColor.color = color;
        }
        
    }
    
}
