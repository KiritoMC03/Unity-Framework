using System;
using System.Collections.Generic;
using Framework.Base.Collections;
using UnityEngine;

namespace Framework.Base.Dependencies.Indexation
{
    [Serializable]
    public class IndexReference : BaseIndexReference<IIndexationKey>
    {
        [SerializeField]
        private SerializedInterfacesList<IIndexationKey> indexes;

        public override IEnumerable<IIndexationKey> IterateIndexes() => indexes.Iterate();
    }
}