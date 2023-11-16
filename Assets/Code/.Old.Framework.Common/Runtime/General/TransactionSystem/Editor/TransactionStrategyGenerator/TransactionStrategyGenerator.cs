#if UNITY_EDITOR
namespace GameKit.General.TransactionSystem.Editor
{
    public class TransactionStrategyGenerator
    {
        #region Methods

        public static void ScriptBasedStrategyGenerate(string sourceScriptName) => 
            ScriptBasedStrategyGenerator.Generate(sourceScriptName);

        #endregion
    }
}
#endif