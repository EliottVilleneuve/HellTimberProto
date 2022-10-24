///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace HellLumber {

    public class AutoSubmitButton : MonoBehaviour {

        public Button button;

        private void OnValidate()
        {
            if(button == null) button = GetComponent<Button>();
        }

        private void Update()
        {
            //if (Controller.GetButtonDown("Submit")) button.onClick.Invoke();
        }
    }
}