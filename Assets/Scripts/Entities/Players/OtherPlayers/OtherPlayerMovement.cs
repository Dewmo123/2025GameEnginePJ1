using Scripts.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerMovement : MonoBehaviour, IEntityComponent
    {
        public float InterpolationBackTime = 5F; // 200ms �������� ����

        public List<SnapshotPacket> _snapshots = new List<SnapshotPacket>();
        [SerializeField] private float syncOffset;
        private Vector3 _mousePos;
        private Player _player;
        public bool IsAiming { get; private set; }
        public void Initialize(NetworkEntity entity)
        {
            _player = entity as Player;
        }
        private float _serverToClientOffset;
        public void Synchronize(PlayerInfoPacket packet)
        {
            IsAiming = packet.isAiming;
            _mousePos = packet.mouse.ToVector3();
        }
        public void AddSnapshot(SnapshotPacket pak)
        {
            _serverToClientOffset = Time.time - (pak.timestamp / 1000f); // �� ����
            _snapshots.Add(pak);
            // ������ ������ ����
            if (_snapshots.Count > 50)
                _snapshots.RemoveAt(0);
        }
        private void Update()
        {
            float interpTime = Time.time - InterpolationBackTime - _serverToClientOffset;

            // �ʿ��� �������� 2�� �̻� �־�� ���� ����
            if (_snapshots.Count < 2)
                return;
            // ���� ������ �� ���� snapshot ã��
            for (int i = 0; i < _snapshots.Count - 1; i++)
            {
                if (_snapshots[i].timestamp / 1000f <= interpTime && _snapshots[i + 1].timestamp / 1000f >= interpTime)
                {
                    SnapshotPacket older = _snapshots[i];
                    SnapshotPacket newer = _snapshots[i + 1];
                    //Debug.Log($"Server: {older.timestamp / 1000F},Clinet:{interpTime},Next:{newer.timestamp / 1000f}");

                    float t = Mathf.InverseLerp(older.timestamp, newer.timestamp, interpTime);

                    Vector3 interpPos = Vector3.Lerp(older.position.ToVector3(), newer.position.ToVector3(), t);
                    Quaternion interpRot = Quaternion.Slerp(older.rotation.ToQuaternion(), newer.rotation.ToQuaternion(), t);

                    _player.transform.position = interpPos;
                    _player.transform.rotation = interpRot;
                    return;
                }
            }
        }
    }
}
