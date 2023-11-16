using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameKit.General.Structures;
using General.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace GameKit.General.Extensions
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
        
        public static void SetDestination(this NavMeshAgent agent, Vector3 target, Action onComplete)
        {
            agent.SetDestination(target);
            agent.InvokeOnComplete(onComplete);
        }
        
        public static void InvokeOnComplete(this NavMeshAgent agent, Action action)
        {
            if (agent.IsNull() || action.IsNull()) return;
            if (!agent.TryGetComponent(out EmptyMonoBehaviour emptyMonoBehaviour))
                emptyMonoBehaviour = agent.gameObject.AddComponent<EmptyMonoBehaviour>();
            emptyMonoBehaviour.StartCoroutine(WaitCompletionRoutine(agent, action, emptyMonoBehaviour));
        }

        private static IEnumerator WaitCompletionRoutine(
            NavMeshAgent agent, 
            Action action,
            UnityEngine.Object emptyMonoBehaviour)
        {
            while (agent.hasPath && Vector3.Distance(agent.transform.position, agent.pathEndPosition) > 0.1f)
                yield return null;
            UnityEngine.Object.DestroyImmediate(emptyMonoBehaviour);
            action?.Invoke();
        }
    }
}
