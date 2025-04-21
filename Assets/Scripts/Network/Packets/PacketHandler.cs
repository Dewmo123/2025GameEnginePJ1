using Scripts.Core.Managers;
using Scripts.UI.Room;
using ServerCore;
using System;
using UnityEngine;

class PacketHandler
{
    internal static void S_EnterRoomFirstHandler(PacketSession session, IPacket packet)
    {
        var players = packet as S_EnterRoomFirst;
        Debug.Log($"myIndex : {players.myIndex}");
    }

    internal static void S_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var roomEnter = packet as S_RoomEnter;
        Debug.Log($"newPlayer: {roomEnter.newPlayer.index}");
    }

    internal static void S_RoomExitHandler(PacketSession session, IPacket packet)
    {
    }

    internal static void S_RoomListHandler(PacketSession session, IPacket packet)
    {
        var listPacket = packet as S_RoomList;
        UIManager.Instance.GetCompo<RoomListUI>().SetList(listPacket.roomInfos);
    }

    internal static void S_TestTextHandler(PacketSession session, IPacket packet)
    {
        var test = packet as S_TestText;
    }

    internal static void S_UpdateInfosHandler(PacketSession session, IPacket packet)
    {
        var p = packet as S_UpdateInfos;
        foreach (var item in p.playerInfos)
        {
            Debug.Log($"{item.index}");
        }
    }
}
