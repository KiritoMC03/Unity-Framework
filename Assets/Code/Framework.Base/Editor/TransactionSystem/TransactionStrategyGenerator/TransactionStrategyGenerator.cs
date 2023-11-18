#if UNITY_EDITOR
namespace Framework.Base.Transactions.Editor
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