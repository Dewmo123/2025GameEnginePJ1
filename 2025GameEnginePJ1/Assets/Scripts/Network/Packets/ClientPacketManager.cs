using Core.EventSystem;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PacketManager
{
    private PacketHandler _packetHandler;
    public PacketManager(EventChannelSO packetChannel)
    {
        _packetHandler = new PacketHandler(packetChannel);
        Register();
    }

    Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> _onRecv = new();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
        RegisterHandler<S_RoomEnter>(PacketID.S_RoomEnter, _packetHandler.S_RoomEnterHandler);
        RegisterHandler<S_RoomExit>(PacketID.S_RoomExit, _packetHandler.S_RoomExitHandler);
        RegisterHandler<S_RoomList>(PacketID.S_RoomList, _packetHandler.S_RoomListHandler);
        RegisterHandler<S_EnterRoomFirst>(PacketID.S_EnterRoomFirst, _packetHandler.S_EnterRoomFirstHandler);
        RegisterHandler<S_UpdateInfos>(PacketID.S_UpdateInfos, _packetHandler.S_UpdateInfosHandler);
        RegisterHandler<S_TeamInfos>(PacketID.S_TeamInfos, _packetHandler.S_TeamInfosHandler);
    }

    private void RegisterHandler<T>(PacketID id, Action<PacketSession, IPacket> handler) where T : IPacket, new()
    {
        _onRecv.Add((ushort)id, PacketUtility.CreatePacket<T>);
        _handler.Add((ushort)id, handler);
    }

    public IPacket OnRecvPacket(ArraySegment<byte> buffer)
    {
        ushort packetId = PacketUtility.ReadPacketID(buffer);
        Func<ArraySegment<byte>, IPacket> func = null;
        if (_onRecv.TryGetValue(packetId, out func))
            return func.Invoke(buffer);
        return default;
    }
    public void HandlePacket(PacketSession session, IPacket packet)
    {
        if (_handler.ContainsKey(packet.Protocol))
            _handler[packet.Protocol].Invoke(session, packet);
        else
        {
            Debug.Log("Fail: "+packet.Protocol);
            throw new NullReferenceException();
        }
    }
}