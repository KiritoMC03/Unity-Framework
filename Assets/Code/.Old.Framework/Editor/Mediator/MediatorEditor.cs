using UnityEditor;

namespace General.Editor
{
    public class MediatorEditor
    {
        public const string MediatorStakeTrace = "MEDIATOR_STAKE_TRACE";
        public const string SectionName = "Mediator";

        [MenuItem("Plugins/General Plugin/Mediator/Activate StackTraces")]
        private static void Activate()
        {
            DependencyController.AddDefine(MediatorStakeTrace,SectionName);
        }
    
        [MenuItem("Plugins/General Plugin/Mediator/Deactivate StackTraces")]
        private static void Deactivate()
        {
            DependencyController.DeleteSection(SectionName);
        }
    }
}
