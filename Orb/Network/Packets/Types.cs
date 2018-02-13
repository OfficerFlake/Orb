using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Network
    {
        public static partial class Packets
        {
            public static class Types
            {
                public const int Login = 1;
                public const int Error = 3;
                public const int Map = 4;
                public const int CreateVehicle = 5;
                public const int AcknowledgeSetting = 6;
                public const int SmokeCol = 7;
                public const int JoinRequest = 8;
                public const int JoinApproved = 9;
                public const int JoinDenied = 10;
                public const int FlightData = 11;
                public const int LeaveFlight = 12;
                public const int DestoryAircraft = 13;
                public const int EndAircraftList = 16;
                public const int Heartbeat = 17;
                public const int LockedTarget = 18;
                public const int DestoryGround = 19;
                public const int OrdinanceDropped = 20;
                public const int AcknowledgeUnknown1 = 21;
                public const int Damage = 22;
                public const int ServerVersion = 29;
                public const int AircraftConfiguration = 30;
                public const int MissilesOption = 31;
                public const int Chat = 32;
                public const int Weather = 33;
                public const int AircraftLoading = 36;
                public const int UserList = 37;
                public const int Airstate = 38;
                public const int WeaponsOption = 39;
                public const int TurrentData = 40;
                public const int UsernameDistance = 41;
                public const int MiscCommand = 43;
                public const int AircraftList = 44;
                public const int KillConfirmation = 46;
            }
        }
    }
}
