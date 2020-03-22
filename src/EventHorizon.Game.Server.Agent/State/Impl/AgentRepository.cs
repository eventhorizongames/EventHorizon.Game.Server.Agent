namespace EventHorizon.Game.Server.Agent.State.Impl
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Agent.Model;

    public class AgentRepository : IAgentRepository
    {
        private static readonly ConcurrentDictionary<string, AgentEntity> ENTITIES = new ConcurrentDictionary<string, AgentEntity>();

        public Task<IList<AgentEntity>> AllByZoneTag(
            string zoneTag
        )
        {
            return Task.FromResult(
                (IList<AgentEntity>)ENTITIES.Values.Where(
                    entity => entity.Location.ZoneTag == zoneTag
                ).ToList()
            );
        }

        public Task<IList<AgentEntity>> All()
        {
            return Task.FromResult(
                (IList<AgentEntity>)ENTITIES.Values.ToList()
            );
        }

        public Task<AgentEntity> FindById(
            string id
        )
        {
            ENTITIES.TryGetValue(
                id, out var agent
            );
            return Task.FromResult(
                agent
            );
        }

        public Task Update(
            AgentEntity agent
        )
        {
            ENTITIES.AddOrUpdate(
                agent.Id, 
                agent, 
                (_, __) => agent
            );
            return Task.CompletedTask;
        }
    }
}