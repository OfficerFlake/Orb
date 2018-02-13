using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orb
{
    public static partial class Server
    {
        public partial class Vehicle
        {
            #region Orb Values
            public bool IsAircraft
            {
                get
                {
                    if (Type == 65537)
                    {
                        return false;
                    }
                    else return true;
                }
            }
            public uint FormTarget = 0;
            public float FormX = 0;
            public float FormY = 0;
            public float FormZ = 0;
            public int SmokeType = SmokeTypes.Normal;
            public static class SmokeColor
            {
                public static byte Alpha = 0;
                public static byte Red = 0;
                public static byte Green = 0;
                public static byte Blue = 0;
            }
            #endregion

            #region YSFlight Values
            public int State = 0; //state being "landed" ,"landed, stopped", etc.
            public string Name = "";
            public double TimeStamp = 0;
            public uint ID = 0;
            public uint IFF = 0;
            public uint Type = 0;
            public short Version = 0;
            public double PosX = 0;
            public double PosY = 0;
            public double PosZ = 0;
            public double HdgX = 0;
            public double HdgY = 0;
            public double HdgZ = 0;
            public double V_PosX = 0;
            public double V_PosY = 0;
            public double V_PosZ = 0;
            public double V_HdgX = 0;
            public double V_HdgY = 0;
            public double V_HdgZ = 0;
            public double SmokeOil = 0;      //Weight of loaded smoke.
            public double Fuel = 0;            //WEIGHT of fuel.
            public double Payload = 0;       //Weight of loaded payload.
            public byte FlightState = 0;    //Guns, Lights etc.
            public byte VGW = 0;
            public byte AirBrake = 0;
            public byte Flap = 0;
            public short Flags = 0;
            public short Ammo_GUN = 0;
            public short Ammo_RKT = 0;
            public byte Ammo_AAM = 0;
            public byte Ammo_AGM = 0;
            public byte Ammo_B500 = 0;
            public byte Health = 0;
            public byte gForce = 0;         //Signed: >127 = negative number!
            public byte Throttle = 0;
            public byte Elevator = 0;       //Signed: >127 = negative number!
            public byte Aileron = 0;        //Signed: >127 = negative number!
            public byte Rudder = 0;         //Signed: >127 = negative number!
            public byte ElevatorTrim = 0;   //Signed: >127 = negative number!

            public byte NozzleDirection = 0;
            public byte BombBay = 0;

            public byte ThrustReverse = 0;
            public byte LandingGear = 0;
            public byte Brake = 0;
            #endregion

            #region Orb Vehicle SubTypes
            public static class SmokeTypes
            {
                public static int Normal = 0;
                public static int Rainbow = 1;
            }
            #endregion

            #region Methods

            /// <summary>
            /// Translate an aircraft relative to it's own axis.
            /// </summary>
            /// <param name="x">X position (Left/Right)</param>
            /// <param name="y">Y position (Up/Down)</param>
            /// <param name="z">Z position (Forwards/Backwards)</param>
            public void Translate(double x, double y, double z)
            {
                //Rotation Matricies Care of http://en.wikipedia.org/wiki/Rotation_matrix
                //The ORDER of rotation is very VERY important!
                #region RotX
                x = (x *  1)                          + (y *  0)                           + (z *  0);
                y = (x *  0)                          + (y *  Math.Cos(HdgX.ToRadians()))  + (z *  Math.Sin(HdgX.ToRadians()));
                z = (x *  0)                          + (y * -Math.Sin(HdgX.ToRadians()))  + (z *  Math.Cos(HdgX.ToRadians()));
                #endregion
                #region RotY
                x = (x *  Math.Cos(HdgY.ToRadians())) + (y *  0)                           + (z * -Math.Sin(HdgY.ToRadians()));
                y = (x *  0)                          + (y *  1)                           + (z *  0);
                z = (x *  Math.Sin(HdgY.ToRadians())) + (y *  0)                           + (z *  Math.Cos(HdgY.ToRadians()));
                #endregion
                #region RotZ
                x = (x *  Math.Cos(HdgZ.ToRadians())) + (y *  Math.Sin(HdgZ.ToRadians()))  + (z *  0);
                y = (x * -Math.Sin(HdgZ.ToRadians())) + (y *  Math.Cos(HdgZ.ToRadians()))  + (z *  0);
                z = (x *  0)                          + (y *  0)                           + (z *  1);
                #endregion
                PosX += x;
                PosY += y;
                PosZ += z;
            }

            /// <summary>
            /// Rotates an aircraft relative to it's own axis.
            /// </summary>
            /// <param name="x">X Angle (Pitch)</param>
            /// <param name="y">Y Angle (Yaw)</param>
            /// <param name="z">Z Angle (Bank)</param>
            public void Rotate(double x, double y, double z)
            {
                //Rotation Matricies Care of http://en.wikipedia.org/wiki/Rotation_matrix
                //The ORDER of rotation is very VERY important!

                //HdgX = 0;
                HdgY = 0;
                HdgZ = 0;
                x = 0;
                y = 0;
                z = 0;

                double new_vx = 0;
                double new_vy = 0;
                double new_vz = 0;

                double Angle_X = 0;
                double Angle_Y = 0;
                double Angle_Z = 0;

                double[] Rotate = _Rotate(0, 0, 1, HdgX.ToDegreesFromShort() + x, HdgY.ToDegreesFromShort() + y, HdgZ.ToDegreesFromShort() + z);

                Angle_X = Math.Atan2(Rotate[2], Rotate[0]);
                Logger.Console.Debug("&cXANGLE: " + Angle_X.ToString());

                Rotate = _Rotate(0, 1, 0, HdgX.ToDegreesFromShort() + x, HdgY.ToDegreesFromShort() + y, HdgZ.ToDegreesFromShort() + z);

                Angle_Y = Math.Atan2(Rotate[2], Rotate[1]);

                Rotate = _Rotate(1, 0, 0, HdgX.ToDegreesFromShort() + x, HdgY.ToDegreesFromShort() + y, HdgZ.ToDegreesFromShort() + z);

                Angle_Z = Math.Atan2(Rotate[0], Rotate[1]);


                Rotate = _Rotate(1, 0, 0, V_HdgX.ToDegreesFromShort() + x, V_HdgY.ToDegreesFromShort() + y, V_HdgZ.ToDegreesFromShort() + z);

                V_HdgX = Math.Atan2(Rotate[0], Rotate[2]);

                Rotate = _Rotate(0, 1, 0, V_HdgX.ToDegreesFromShort() + x, V_HdgY.ToDegreesFromShort() + y, V_HdgZ.ToDegreesFromShort() + z);

                V_HdgY = Math.Atan2(Rotate[2], Rotate[1]);

                Rotate = _Rotate(0, 0, 1, V_HdgX.ToDegreesFromShort() + x, V_HdgY.ToDegreesFromShort() + y, V_HdgZ.ToDegreesFromShort() + z);

                V_HdgZ = Math.Atan2(Rotate[0], Rotate[1]);






                double NetVelocity = Math.Sqrt(Math.Pow(V_PosX, 2) + Math.Pow(V_PosY, 2) + Math.Pow(V_PosZ, 2));

                V_HdgX *= 65536 / 2;
                V_PosX = Math.Sin(Angle_X) * Math.Cos(Angle_Y) * NetVelocity;

                V_HdgY *= 65536 / 2;
                V_PosY = Math.Sin(Angle_Y) * NetVelocity;

                V_HdgZ *= 65536 / 2;
                V_PosZ = Math.Cos(Angle_X) * Math.Cos(Angle_Y) * NetVelocity;

                //Rotate = _Rotate(HdgX, HdgY, HdgZ, x, y, z);

                HdgX = Angle_X / (180) * (65536/2);
                Logger.Console.Debug(HdgX.ToString());
                HdgY = 0;
                HdgZ = 0;

                V_HdgX = 0;
                V_HdgY = 0;
                V_HdgZ = 0;

                V_PosX = 0;
                V_PosY = 0;
                V_PosZ = 0;

                //HdgX = 0;
                HdgY = 0;
                HdgZ = 0;
            }

            private double[] _Rotate(double x, double y, double z, double rx, double ry, double rz)
            {
                //YSFlight x/y axis is swapped!
                double new_vx = x;
                double new_vy = y;
                double new_vz = z;
                #region RotZ
                //Logger.Console.Debug("&aPASS 0:" + new_vx.ToString() + " " + new_vy.ToString() + " " + new_vz.ToString());
                new_vx = (x * Math.Cos(rz.ToRadians())) - (y * Math.Sin(rz.ToRadians())) + (z * 0);
                new_vy = (x * Math.Sin(rz.ToRadians())) + (y * Math.Cos(rz.ToRadians())) + (z * 0);
                new_vz = (x * 0) + (y * 0) + (z * 1);
                #endregion
                #region RotY
                //Logger.Console.Debug("&bPASS 1:" + new_vx.ToString() + " " + new_vy.ToString() + " " + new_vz.ToString());
                new_vx = (new_vx * Math.Cos(ry.ToRadians())) + (new_vy * 0) + (new_vz * Math.Sin(ry.ToRadians()));
                new_vy = (new_vx * 0) + (new_vy * 1) + (new_vz * 0);
                new_vz = -(new_vx * Math.Sin(ry.ToRadians())) + (new_vy * 0) + (new_vz * Math.Cos(ry.ToRadians()));
                #endregion
                #region RotX
                //Logger.Console.Debug("&cPASS 2:" + new_vx.ToString() + " " + new_vy.ToString() + " " + new_vz.ToString());
                new_vx = (new_vx * 1) + (new_vy * 0) + (new_vz * 0);
                new_vy = (new_vx * 0) + (new_vy * Math.Cos(rx.ToRadians())) - (new_vz * Math.Sin(rx.ToRadians()));
                new_vz = (new_vx * 0) + (new_vy * Math.Sin(rx.ToRadians())) + (new_vz * Math.Cos(rx.ToRadians()));
                //Logger.Console.Debug("&dPASS 3:" + new_vx.ToString() + " " + new_vy.ToString() + " " + new_vz.ToString());
                
                #endregion
                
                return new double[] { new_vx, new_vy, new_vz };
            }
            #endregion
        }
    }
}