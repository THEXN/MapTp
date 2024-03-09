using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using System.Timers;

namespace MapTeleport
{
    [ApiVersion(2, 1)]
    public class MapTeleport : TerrariaPlugin
    {
        public MapTeleport(Main game) : base(game)
        {
            Order = 1;
        }
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public override string Author
        {
            get { return "Nova4334，肝帝熙恩汉化适配1449"; }
        }
        public override string Name
        {
            get { return "MapTeleport"; }
        }
        public override string Description
        {
            get { return "允许玩家传送到地图上的选定位置."; }
        }

        public override void Initialize()
        {
            GetDataHandlers.ReadNetModule.Register(teleport);
        }

        public const string ALLOWED = "maptp";

        public const string ALLOWEDSOLIDS = "maptp.noclip";

        private void teleport(object unused, GetDataHandlers.ReadNetModuleEventArgs args)
        {
            if (args.Player.HasPermission(ALLOWED))
            {
                if (args.ModuleType == GetDataHandlers.NetModuleType.Ping)
                {
                    using (var reader = new BinaryReader(args.Data))
                    {
                        Vector2 pos = reader.ReadVector2();
                        if (!(pos.X == Tile.Type_Solid && pos.Y == Tile.Type_Solid) || args.Player.HasPermission(ALLOWEDSOLIDS))
                        {
                            args.Player.Teleport(pos.X * 16, pos.Y * 16); return;
                        }
                        else args.Player.SendErrorMessage("您正在尝试传送到实心方块中。请在地图上选择一个不包含实心方块的地方，然后重试.");
                        return;
                    }
                }
            }
        }
    }
}