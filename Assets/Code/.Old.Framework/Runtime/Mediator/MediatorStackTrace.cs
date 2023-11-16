#if MEDIATOR_STAKE_TRACE
using System.Collections.Generic;
using System.Diagnostics;

namespace General.Mediator.Runtime
{
    public static class MediatorStackTrace
    {
        public static Dictionary<string, List<StackTrace>> stackTraces = new Dictionary<string, List<StackTrace>>();

        public static void GetStackTrace(string methodName)
        {
            if (!stackTraces.ContainsKey(methodName))
            {
                StackTrace stack = new StackTrace();
                stackTraces.Add(methodName,new List<StackTrace>() {stack});
            }
            else
            {
                StackTrace stack = new StackTrace();
                stackTraces.TryGetValue(methodName, out var list);
                list.Add(stack);
            }
        }
    }
}
#endif
