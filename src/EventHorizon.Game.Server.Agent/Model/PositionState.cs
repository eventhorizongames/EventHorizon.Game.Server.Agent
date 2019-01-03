using System.Numerics;

namespace EventHorizon.Game.Server.Agent.Model
{
    public struct PositionState
    {
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }
        public Vector3 Position { get; set; }
    }
}