using Scripts.Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Room
{
    public class RoomListUI : MonoBehaviour, INetworkUI
    {
        [SerializeField] private GameObject attribute;
        [SerializeField] private int countPerPage = 5;

        private List<RoomAttributeUI> _attributes;
        private List<RoomInfoPacket> _roomInfos;

        private int _currentCount = 0;
        private int _currentAttCount = 0;
        private int _currentRoomId;

        private void Awake()
        {
            _attributes = new List<RoomAttributeUI>();
            for (int i = 0; i < countPerPage; i++)
            {
                _attributes.Add(Instantiate(attribute, transform).GetComponent<RoomAttributeUI>());
                int k = i;
                _attributes[i].GetComponent<Button>().onClick.AddListener(() => SetRoomId(k + 1));
            }
            ResetAttributes();
        }

        private void SetRoomId(int roomId)
        {
            Debug.Log(roomId);
            _currentRoomId = roomId;
        }
        private void OnDestroy()
        {
            foreach (var item in _attributes)
                item.GetComponent<Button>().onClick.RemoveAllListeners();
;        }

        public void ResetAttributes()
            => _attributes.ForEach(att => att.ClearUI());
        public void SetList(List<RoomInfoPacket> roomInfos)
        {
            _roomInfos = roomInfos;
            _currentCount = 0;
            SetNextAttributes();
        }
        public void SetNextAttributes()
        {
            ResetAttributes();
            int i;
            for (i = _currentCount; i < _currentCount + countPerPage; i++)
            {
                if (i >= _roomInfos.Count)
                    break;
                _attributes[i - _currentCount]
                    .SetText(_roomInfos[i].roomName, _roomInfos[i].currentCount, _roomInfos[i].maxCount);
            }
            if (i == _currentCount)
            {
                _currentCount -= _currentAttCount;
                SetNextAttributes();
                return;
            }
            _currentAttCount = i;
            _currentCount += i;
        }
        public void SetPrevAttributes()
        {
            ResetAttributes();
            _currentCount -= (_currentAttCount + 5);
            if (_currentCount < 0)
                _currentCount = 0;
            SetNextAttributes();
        }
        public void EnterRoom()
        {
            C_RoomEnter roomEnter = new C_RoomEnter() { roomId = _currentRoomId };
            NetworkManager.Instance.SendPacket(roomEnter);
        }
    }
}
