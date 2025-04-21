using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI.Room
{
    public class RoomListUI : MonoBehaviour,INetworkUI
    {
        [SerializeField] private GameObject attribute;
        [SerializeField] private int countPerPage = 5;

        [SerializeField]private List<RoomAttributeUI> _attributes;
        private void Awake()
        {
            _attributes = new List<RoomAttributeUI>();
            for (int i = 0; i < countPerPage; i++)
            {
                _attributes.Add(Instantiate(attribute,transform).GetComponent<RoomAttributeUI>());
            }
            ResetAttributes();
        }
        public void ResetAttributes()
            => _attributes.ForEach(att => att.ClearUI());
        public void SetList(List<RoomInfoPacket> roomInfos)
        {
            Debug.Log("asdsad");
            ResetAttributes();
            for (int i = 0; i < roomInfos.Count; i++)
                _attributes[i].SetText(roomInfos[i].roomName, roomInfos[i].currentCount, roomInfos[i].maxCount);
        }
    }
}
