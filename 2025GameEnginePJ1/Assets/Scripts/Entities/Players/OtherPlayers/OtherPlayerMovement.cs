using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Players.OtherPlayers;
using Scripts.Core;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

namespace Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerMovement : PlayerMovement
    {
        public long InterpolationBackTime = 5; // 200ms �������� ����

        public List<SnapshotPacket> _snapshots = new List<SnapshotPacket>();
        private Vector3 _serverPos;
        private Vector3 _prevInterpPos;

        private int _currentAnimHash;
        private OtherPlayerAttackCompo _attackCompo;
        public override void Initialize(NetworkEntity entity)
        {
            base.Initialize(entity);
            _attackCompo = entity.GetCompo<OtherPlayerAttackCompo>();
        }
        public void Synchronize(LocationInfoPacket packet)
        {
            IsAiming = packet.isAiming;
            SetPosition(packet.position.ToVector3());
        }
        public void AddSnapshot(SnapshotPacket pak)
        {
            _snapshots.Add(pak);
            // ������ ������ ����
            if (_snapshots.Count > 10)
                _snapshots.RemoveAt(0);
        }
        private void Update()
        {
            long interpTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - InterpolationBackTime;
            // �ʿ��� �������� 2�� �̻� �־�� ���� ����
            if (_snapshots.Count < 2)
                return;
            // ���� ������ �� ���� snapshot ã��
            for (int i = 0; i < _snapshots.Count - 1; i++)
            {
                if (_snapshots[i].timestamp < interpTime && _snapshots[i + 1].timestamp > interpTime)
                {
                    SnapshotPacket older = _snapshots[i];
                    SnapshotPacket newer = _snapshots[i + 1];

                    float t = (interpTime - older.timestamp) / (float)(newer.timestamp - older.timestamp);
                    //Debug.Log($"old: {older.timestamp}, new: {newer.timestamp}, client:{interpTime}");
                    //Debug.Log(t);


                    Vector3 interpPos = Vector3.Lerp(older.position.ToVector3(), newer.position.ToVector3(), t);
                    SetAnimation(interpPos, newer.animHash);
                    _prevInterpPos = interpPos;
                    Quaternion interpRot = Quaternion.Slerp(older.rotation.ToQuaternion(), newer.rotation.ToQuaternion(), t);
                    Quaternion interpGunRot = Quaternion.Slerp(older.gunRotation.ToQuaternion(), newer.gunRotation.ToQuaternion(), t);
                    _player.transform.position = interpPos;
                    _player.transform.rotation = interpRot;
                    _attackCompo.currentGun.transform.rotation = interpGunRot;
                    return;
                }
            }
        }
        private void SetAnimation(Vector3 interpPos, int animHash)
        {
            Vector3 direction = (interpPos - _prevInterpPos).normalized;
            if (_currentAnimHash != animHash)
            {
                _animator.SetParam(_currentAnimHash, false);
                _currentAnimHash = animHash;
                _animator.SetParam(_currentAnimHash, true);
            }
            if (direction.magnitude > 0f)
            {
                float forwardDot = Vector3.Dot(_player.transform.forward, direction); // ��/��
                float rightDot = Vector3.Dot(_player.transform.right, direction);     // ��/��
                _animator.SetParam(_xHash, rightDot);
                _animator.SetParam(_zHash, forwardDot);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_serverPos, 0.3f);
        }

        public override void SetPosition(Vector3 position)
        {
            _player.transform.position = position;
        }
    }
}
