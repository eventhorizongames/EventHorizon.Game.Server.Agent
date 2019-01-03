using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Agent.Model;

namespace EventHorizon.Game.Server.Agent.State
{
    public interface IAgentRepository
    {
        Task<IList<AgentEntity>> All();
        Task<IList<AgentEntity>> AllByZoneTag(string zoneTag);
        Task<AgentEntity> FindById(string id);
        Task Update(AgentEntity agent);
    }
}