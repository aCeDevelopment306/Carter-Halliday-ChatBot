/**
 * *********************************************************************
 * © 2023 ThangChiba. All rights reserved.
 * 
 * This code is licensed under the MIT License.
 * 
 * Homepage: https://thangchiba.com
 * Email: thangchiba@gmail.com
 * *********************************************************************
 */

using UnityEngine;
using UnityEngine.UI;

namespace ThangChibaGPT
{
    [RequireComponent(typeof(Button))]
    public class ButtonChatMode : MonoBehaviour
    {
        [SerializeField] private ChatMode chatMode;
        private Button button;

        public void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ChatManager.Instance.OnSetChatMode(chatMode);
        }
    }
}
