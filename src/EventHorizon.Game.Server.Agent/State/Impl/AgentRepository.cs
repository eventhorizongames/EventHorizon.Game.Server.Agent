using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Model;

namespace EventHorizon.Game.Server.Agent.State.Impl
{
    public class AgentRepository : IAgentRepository
    {
        private static readonly ConcurrentDictionary<string, AgentEntity> ENTITIES = new ConcurrentDictionary<string, AgentEntity>();

        public Task<IList<AgentEntity>> AllByZoneTag(string zoneTag)
        {
            return Task.FromResult((IList<AgentEntity>)ENTITIES.Values.Where(entity => entity.Position.ZoneTag == zoneTag).ToList());
        }

        public Task<IList<AgentEntity>> All()
        {
            return Task.FromResult((IList<AgentEntity>)ENTITIES.Values.ToList());
        }

        public Task<AgentEntity> FindById(string id)
        {
            var agent = default(AgentEntity);
            ENTITIES.TryGetValue(id, out agent);
            return Task.FromResult(agent);
        }

        public Task Update(AgentEntity agent)
        {
            ENTITIES.AddOrUpdate(agent.Id, agent, (key, current) => agent);
            return Task.CompletedTask;
        }
    }
}