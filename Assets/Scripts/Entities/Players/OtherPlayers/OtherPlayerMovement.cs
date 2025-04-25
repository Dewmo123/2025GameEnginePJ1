using Assets.Scripts.Entities;
using Scripts.Core;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

namespace Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerMovement : MonoBehaviour, IEntityComponent
    {
        public long InterpolationBackTime = 5; // 200ms 뒤쳐지게 보간

        public List<SnapshotPacket> _snapshots = new List<SnapshotPacket>();
        private Vector3 _mousePos;
        private Vector3 _prevInterpPos;
        private Player _player;
        private EntityAnimator _animator;
        public bool IsAiming { get; private set; }

        private long _serverToClientOffset;

        private int _currentAnimHash;
        private int _xHash;
        private int _zHash;
        public void Initialize(NetworkEntity entity)
        {
            _xHash = Animator.StringToHash("X");
            _zHash = Animator.StringToHash("Z");
            _animator = entity.GetCompo<PlayerAnimator>();
            _player = entity as Player;
        }
        public void Synchronize(PlayerInfoPacket packet)
        {
            IsAiming = packet.isAiming;
            _mousePos = packet.mouse.ToVector3();
        }
        public void AddSnapshot(SnapshotPacket pak)
        {
            _serverToClientOffset = (long)(Time.time * 1000) - (pak.timestamp); // 초 단위
            _snapshots.Add(pak);
            // 오래된 스냅샷 제거
            if (_snapshots.Count > 10)
                _snapshots.RemoveAt(0);
        }
        private void Update()
        {
            long interpTime = (long)(Time.time * 1000) - InterpolationBackTime - _serverToClientOffset;
            // 필요한 스냅샷이 2개 이상 있어야 보간 가능
            if (_snapshots.Count < 2)
                return;
            // 보간 가능한 두 개의 snapshot 찾기
            for (int i = 0; i < _snapshots.Count - 1; i++)
            {
                if (_snapshots[i].timestamp <= interpTime && _snapshots[i + 1].timestamp >= interpTime)
                {
                    SnapshotPacket older = _snapshots[i];
                    SnapshotPacket newer = _snapshots[i + 1];

                    float t = Mathf.InverseLerp(older.timestamp, newer.timestamp, interpTime);
                    //Debug.Log($"old: {older.timestamp}, new: {newer.timestamp}, client:{interpTime}");
                    //Debug.Log(t);


                    Vector3 interpPos = Vector3.Lerp(older.position.ToVector3(), newer.position.ToVector3(), t);
                    if (_prevInterpPos != interpPos)
                        SetAnimation((interpPos - _prevInterpPos).normalized, newer.animHash);
                    _prevInterpPos = interpPos;
                    Quaternion interpRot = Quaternion.Slerp(older.rotation.ToQuaternion(), newer.rotation.ToQuaternion(), t);

                    _player.transform.position = interpPos;
                    _player.transform.rotation = interpRot;
                    return;
                }
            }
        }
        private void SetAnimation(Vector3 direction, int animHash)
        {
            if (_currentAnimHash != animHash)
            {
                _animator.SetParam(_currentAnimHash, false);
                _currentAnimHash = animHash;
                _animator.SetParam(_currentAnimHash, true);
            }
            float forwardDot = Vector3.Dot(_player.transform.forward, direction); // 앞/뒤
            float rightDot = Vector3.Dot(_player.transform.right, direction);     // 좌/우
            Debug.Log($"{forwardDot},{rightDot}");
            _animator.SetParam(_xHash, rightDot);
            _animator.SetParam(_zHash, forwardDot);
        }
    }
}
