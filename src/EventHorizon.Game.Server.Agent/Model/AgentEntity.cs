namespace EventHorizon.Game.Server.Agent.Model
{
    using System;
    using System.Collections.Generic;
    
    public struct AgentEntity
    {
        public static AgentEntity NULL = default(AgentEntity);

        public string Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public LocationState Location { get; set; }
        public IList<string> TagList { get; set; }
        public object Data { get; set; }

        public AgentEntity(
            string id
        )
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.Transform = default(TransformState);
            this.Location = default(LocationState);
            this.TagList = new List<String>();

            this.Data = new { };
        }
    }
}