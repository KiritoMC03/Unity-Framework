using System;
using Framework.Base.Collections;

namespace Framework.Base.Extensions
{
    public static class SerializedInterfacesListExtension
    {
        #region Methods

        public static T GetRandomWhere<T>(this SerializedInterfacesList<T> list, Predicate<T> predicate)
            where T : class
        {
            SerializedInterfacesList<T> filteredList = new SerializedInterfacesList<T>(list.Count);
            T current = default;
            for (int i = 0; i < list.Count; i++)
            {
                current = list.GetAt(i);
                if (predicate.Invoke(current)) filteredList.Add(list[i]);
            }

            if (filteredList.IsNullOrEmpty()) return default;
            return filteredList.GetAt(UnityEngine.Random.Range(0, filteredList.Count));
        }

        public static T GetRandomItem<T>(this SerializedInterfacesList<T> list)
            where T : class =>
            list.GetAt(UnityEngine.Random.Range(0, list.Count));

        #endregion
    }
}