using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Room
{
    public class RoomAttributeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomNameText;
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private Image _backgroundImage;
        private Color _defaultColor;
        private void Awake()
        {
            _defaultColor = _backgroundImage.color;
        }
        public void SetText(string roomName, int currentCount,int maxCount)
        {
            roomNameText.text = roomName;
            playerCountText.text = $"{currentCount} / {maxCount}";
            _backgroundImage.color = _defaultColor;
        }
        public void ClearUI()
        {
            roomNameText.text = string.Empty;
            playerCountText.text = $"0 / 0";
            _backgroundImage.color = Color.clear;
        }
    }
}
