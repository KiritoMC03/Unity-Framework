using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Framework.Base.Extensions
{
    public static class NavMeshAgentExtensions
    {
        public static async UniTask SetDestinationAsync(this NavMeshAgent agent, Vector3 target, 
            CancellationToken cancellationToken = default)
        {
            agent.SetDestination(target);
            await UniTask.WaitUntil(IsStopped, cancellationToken: cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                agent.isStopped = true;
            bool IsStopped() => !agent.hasPath || 
                                Vector3.Distance(agent.transform.position, agent.pathEndPosition) < 0.1f;
        }
    }
}
