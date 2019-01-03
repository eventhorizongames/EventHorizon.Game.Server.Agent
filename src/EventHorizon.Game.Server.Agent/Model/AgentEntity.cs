using System;
using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Game.Server.Agent.Model
{
    public struct AgentEntity
    {
        public static AgentEntity NULL = default(AgentEntity);

        public string Id { get; set; }
        public string Name { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }
        public object Data { get; set; }

        public AgentEntity(string id, PositionState positionState)
        {
            this.Id = id;
            this.Name = "";
            this.Position = positionState;
            this.TagList = new List<String>();

            this.Data = new { };
        }
    }
}