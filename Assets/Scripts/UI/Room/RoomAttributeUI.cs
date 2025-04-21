using Scripts.Network;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Room
{
    public class RoomAttributeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomNameText;
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private Image backgroundImage;
        private Color _defaultColor;
        private void Awake()
        {
            _defaultColor = backgroundImage.color;
        }
        public void SetText(string roomName, int currentCount,int maxCount)
        {
            roomNameText.text = roomName;
            playerCountText.text = $"{currentCount} / {maxCount}";
            backgroundImage.color = _defaultColor;
        }
        public void ClearUI()
        {
            roomNameText.text = string.Empty;
            playerCountText.text = string.Empty;
            backgroundImage.color = Color.clear;
        }
    }
}
