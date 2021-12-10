using System;
using System.Threading;
using MemoryManagement;

namespace Trainer
{
    class Program
    {
        public class CsgoHack
        {
            public static class Offsets
            {
                public const int dwClientState_GetLocalPlayer = (0x180);
                public const int dwEntityList                 = (0x4dc178c);
                public const int dwLocalPlayer                = (0xda746c);
                public const int dwClientState                = (0x589fcc);
                public const int dwClientState_ViewAngles     = (0x4d90);
                public const int dwForceAttack                = (0x31f1d14);
                public const int dwForceJump                  = (0x526b5a0);
                public const int m_bDormant                   = (0xed);
                public const int m_fFlags                     = (0x104);
                public const int m_iCrosshairId               = (0x11838);
                public const int m_iTeamNum                   = (0xf4);
                public const int m_aimPunchAngle              = (0x303c);
                public const int m_vecOrigin                  = (0x138);
                public const int m_dwBoneMatrix               = (0x26a8);
                public const int m_vecViewOffset              = (0x108);
                public const int m_iHealth                    = (0x100);
                public const int m_bSpottedByMask             = (0x980);
            }

            public class Vector3
            {
                public float x, y, z;

                public Vector3(float x, float y, float z)
                {
                    this.x = x;
                    this.y = y;
                    this.z = z;
                }

                public float distanceTo(Vector3 vector)
                {
                    Vector3 delta = vector - this;

                    return (float) Math.Sqrt((delta.x * delta.x) + (delta.y * delta.y) + (delta.z * delta.z));
                }

                public static Vector3 operator +(Vector3 vectorA, Vector3 vectorB)
                {
                    return new Vector3(
                        vectorA.x + vectorB.x,
                        vectorA.y + vectorB.y,
                        vectorA.z + vectorB.z
                    );
                }

                public static Vector3 operator -(Vector3 vectorA, Vector3 vectorB)
                {
                    return new Vector3(
                        vectorA.x - vectorB.x,
                        vectorA.y - vectorB.y,
                        vectorA.z - vectorB.z
                    );
                }

                public static Vector3 operator *(Vector3 vector, float scale)
                {
                    return new Vector3(
                        vector.x * scale,
                        vector.y * scale,
                        vector.z * scale
                    );
                }

                public override string ToString() => $"({x}, {y}, {z})";
            }

            public struct ANGLE
            {
                public float pitch, yaw;

                public ANGLE(float x, float y)
                {
                    pitch = x;
                    yaw   = y;
                }

                public void normalize()
                {
                    if (yaw >  180.0f) yaw -= 360.0f;
                    if (yaw < -180.0f) yaw += 360.0f;
                    
                    if (pitch >  89.0f) pitch -= 180.0f;
                    if (pitch < -89.0f) pitch += 180.0f;
                }
            }

            static MemoryManager mm = new MemoryManager("csgo");

            static UIntPtr client = mm.GetModuleByName("client.dll").BaseAddress;
            static UIntPtr engine = mm.GetModuleByName("engine.dll").BaseAddress;

            static UIntPtr getPlayer(int index) => mm.ReadUIntPtr(client + Offsets.dwEntityList + (index * 0x10));
            static UIntPtr getLocalPlayer() => mm.ReadUIntPtr(client + Offsets.dwLocalPlayer);
            static UIntPtr getClientState() => mm.ReadUIntPtr(engine + Offsets.dwClientState);
            static int getPlayerHealth(UIntPtr player) => mm.ReadInt(player + Offsets.m_iHealth);
            static int getPlayerTeam(UIntPtr player) => mm.ReadInt(player + Offsets.m_iTeamNum);
            static int getLocalPlayerFlags() => mm.ReadInt(getLocalPlayer() + Offsets.m_fFlags);
            static bool isDormant(UIntPtr player) => mm.ReadInt(player + Offsets.m_bDormant) == 1;
            static bool isDead(UIntPtr player) => getPlayerHealth(player) <= 0 || getPlayerHealth(player) > 100;
            static bool isSameTeam(UIntPtr playerA, UIntPtr playerB) => getPlayerTeam(playerA) == getPlayerTeam(playerB);
            static UIntPtr getPlayerBoneMatrix(UIntPtr player) => mm.ReadUIntPtr(player + Offsets.m_dwBoneMatrix);
            
            static Vector3 getPlayerLocation(UIntPtr player) => new Vector3(
                mm.ReadFloat(player + Offsets.m_vecOrigin + 0x0),
                mm.ReadFloat(player + Offsets.m_vecOrigin + 0x4),
                mm.ReadFloat(player + Offsets.m_vecOrigin + 0x8)
            );

            static Vector3 getPlayerBoneLocation(UIntPtr player, int bone)
            {
                UIntPtr boneMatrix = getPlayerBoneMatrix(player);

                return new Vector3(
                    mm.ReadFloat(boneMatrix + 0x30 * bone + 0x0C),
                    mm.ReadFloat(boneMatrix + 0x30 * bone + 0x1C),
                    mm.ReadFloat(boneMatrix + 0x30 * bone + 0x2C)
                );
            }

            static Vector3 getLocalPlayerViewOffset()
            {
                UIntPtr localPlayer = getLocalPlayer();

                return new Vector3(
                    mm.ReadFloat(localPlayer + Offsets.m_vecViewOffset + 0x0),
                    mm.ReadFloat(localPlayer + Offsets.m_vecViewOffset + 0x4),
                    mm.ReadFloat(localPlayer + Offsets.m_vecViewOffset + 0x8)
                );
            }

            static Vector3 getLocalPlayerViewAngles()
            {
                UIntPtr clientState = getClientState();

                return new Vector3(
                    mm.ReadFloat(clientState + Offsets.dwClientState_ViewAngles + 0x0),
                    mm.ReadFloat(clientState + Offsets.dwClientState_ViewAngles + 0x4),
                    mm.ReadFloat(clientState + Offsets.dwClientState_ViewAngles + 0x8)
                );
            }

            static Vector3 getLocalPlayerLocation() => getPlayerLocation(getLocalPlayer());

            static void writeLocalPlayerViewAngles(ANGLE angle)
            {
                angle.normalize();

                UIntPtr clientState = getClientState();

                mm.WriteFloat(clientState + Offsets.dwClientState_ViewAngles + 0x0, angle.pitch);
                mm.WriteFloat(clientState + Offsets.dwClientState_ViewAngles + 0x4, angle.yaw  );
            }

            static void forceLocalPlayerAimTo(Vector3 target)
            {
                Vector3 localPlayerHead = getLocalPlayerLocation() + getLocalPlayerViewOffset();

                Vector3 delta = target - localPlayerHead;
                float deltaLength = localPlayerHead.distanceTo(target);
                
                writeLocalPlayerViewAngles(new ANGLE(
                    (float) (-Math.Asin (delta.z / deltaLength) * (180.0f / Math.PI)), 
                    (float) ( Math.Atan2(delta.y , delta.x    ) * (180.0f / Math.PI))
                ));
            }

            public static void run()
            {
                while (!User32.IsKeyDown(User32.VK_DELETE))
                {
                    Console.WriteLine($"LocalPlayer Health: {getPlayerHealth(getLocalPlayer())}");
                    Thread.Sleep(1000);
                }
            }
        }
        
        static void Main(string[] args)
        {
            CsgoHack.run();
        }
    }
}
