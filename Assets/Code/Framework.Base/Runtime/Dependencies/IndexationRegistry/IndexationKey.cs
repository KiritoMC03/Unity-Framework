using UnityEngine;

namespace Framework.Base.Dependencies.Indexation
{
    [CreateAssetMenu(fileName = "IndexationKey", menuName = "Framework/IndexationRegistry/IndexationKey", order = 50)]
    public class IndexationKey : ScriptableObject, IIndexationKey
    {
        public string GetId() => name;
    }
}