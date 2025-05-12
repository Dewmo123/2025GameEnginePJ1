using Assets.Scripts.Entities.Players.OtherPlayers;
using Core.EventSystem;
using Scripts.Core;
using Scripts.Core.EventSystem;
using Scripts.Core.Managers;
using Scripts.Entities.Players;
using Scripts.Entities.Players.MyPlayers;
using Scripts.Entities.Players.OtherPlayers;
using Scripts.UI.Room;
using ServerCore;
using System;
using UnityEngine;

class PacketHandler
{
    private EventChannelSO _packetChannel;
    public PacketHandler(EventChannelSO packetChannel)
    {
        _packetChannel = packetChannel;
    }

    internal void S_EnterRoomFirstHandler(PacketSession session, IPacket packet)
    {
        Debug.Log("RoomFIrst");
        var evt = PacketEvents.HandleFirstEnterRoom;
        var firstPacket = packet as S_EnterRoomFirst;
        evt.packet = firstPacket;
        _packetChannel.InvokeEvent(evt);
        PlayerManager.Instance.FirstInitPlayer(firstPacket);
    }

    internal void S_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var roomEnter = packet as S_RoomEnter;
        PlayerManager.Instance.InitOtherPlayer(roomEnter.newPlayer);
    }

    internal void S_RoomExitHandler(PacketSession session, IPacket packet)
    {
        var roomExit = packet as S_RoomExit;
        PlayerManager.Instance.ExitOtherPlayer(roomExit.Index);
    }

    internal void S_RoomListHandler(PacketSession session, IPacket packet)
    {
        var evt = PacketEvents.HandleRoomList;
        evt.packet = packet as S_RoomList;
        _packetChannel.InvokeEvent(evt);
    }

    internal void S_TeamInfosHandler(PacketSession session, IPacket packet)
    {
        var infos = packet as S_TeamInfos;
        foreach(var item in infos.teamInfos)
        {
            var player = PlayerManager.Instance.GetPlayerById(item.index);
            player.SetTeam(item.team);
            Debug.Log($"index:{item.index}, Team:{item.team}");
        }
    }

    internal void S_TestTextHandler(PacketSession session, IPacket packet)
    {
        var test = packet as S_TestText;
    }

    internal void S_UpdateInfosHandler(PacketSession session, IPacket packet)
    {
        var p = packet as S_UpdateInfos;
        foreach (var item in p.playerInfos)
        {
            var player = PlayerManager.Instance.GetPlayerById(item.index);
            if (player == default)
                continue;
            if (player.Index == PlayerManager.Instance.MyIndex)
            {
                var myMovement = player.GetCompo<MyPlayerMovement>();
                myMovement.SetPosition(item.position.ToVector3());
                continue;
            }
            var movement = player.GetCompo<OtherPlayerMovement>();
            movement.Synchronize(item);
        }
        foreach (var item in p.snapshots)
        {
            var player = PlayerManager.Instance.GetPlayerById(item.index);
            if (player == default)
                continue;
            if (player.Index == PlayerManager.Instance.MyIndex)
                continue;
            var movement = player.GetCompo<OtherPlayerMovement>();
            movement.AddSnapshot(item);
        }
        foreach(var item in p.attacks)
        {
            if (item.attackerIndex == PlayerManager.Instance.MyIndex)
                continue;
            var player = PlayerManager.Instance.GetPlayerById(item.attackerIndex);
            player.GetCompo<OtherPlayerAttackCompo>().Shoot(item.firePos.ToVector3(),item.direction.ToVector3());
        }
    }
}
