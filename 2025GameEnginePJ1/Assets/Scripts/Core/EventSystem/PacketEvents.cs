using Core.EventSystem;
using UnityEngine;

namespace Scripts.Core.EventSystem
{
    public class PacketEvents
    {
        public static readonly HandleFirstEnterRoom HandleFirstEnterRoom = new HandleFirstEnterRoom();
        public static readonly HandleRoomList HandleRoomList= new HandleRoomList();
    }
    public class HandleFirstEnterRoom : GameEvent
    {
        public S_EnterRoomFirst packet;
    }
    public class HandleRoomList : GameEvent
    {
        public S_RoomList packet;
    }
}
