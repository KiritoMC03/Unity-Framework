using System;
using System.Collections.Generic;
using Framework.Base.Extensions;

namespace Framework.Base.Dependencies.Mediator
{
    internal class PermissionChecker
    {
        #region Fields

        private const string AppealTypeIsNull = "AppealType is null.";
        private const string CheckTypeIsNull = "CheckType is null";
        private const string TypeIsNull = "Type is null.";
        private const string TypesIsNull = "Types is null.";
        private Dictionary<Type, Dictionary<Type,bool>> permissions = new Dictionary<Type, Dictionary<Type, bool>>(20);
        private Dictionary<Type, bool> lockedPermissions = new Dictionary<Type, bool>(20);

        #endregion
        

        #region Methods
        
        public void SetPermission(Type type,Type[] types)
        {
            if (type.LogIfNull(TypeIsNull)) return;
            if (types.LogIfNull(TypesIsNull)) return;
            if (types.Length <= 0) return;
            if (!permissions.ContainsKey(type))
            {
                permissions.Add(type,CreatePermissions(types));
            }
            else
            {
                permissions[type] = CreatePermissions(types);
            }
        }

        public void SetLock(in Type type,in bool lockedState)
        {
            if (type.LogIfNull(TypeIsNull)) return;
            if (lockedPermissions.ContainsKey(type))
            {
                lockedPermissions[type] = lockedState;
            }
            else
            {
                lockedPermissions.Add(type, lockedState);
            }
        }

        public bool IsLocked(in Type type)
        {
            type.LogIfNull(TypeIsNull);
            return lockedPermissions.TryGetValue(type, out var lockedState) && lockedState;
        }

        public PermissionStates CheckPermission(in Type appealType,in Type checkType)
        {
            if (appealType.LogIfNull(AppealTypeIsNull) || checkType.LogIfNull(CheckTypeIsNull))
                return PermissionStates.PermissionNotFound;

            if (!permissions.TryGetValue(checkType, out Dictionary<Type, bool> types))
                return PermissionStates.PermissionNotFound;
            
            return types.ContainsKey(appealType) ? PermissionStates.HaveAccess : PermissionStates.HaveNoAccess;
        }

        private Dictionary<Type, bool> CreatePermissions(in Type[] types)
        {
            var localPermissions = new Dictionary<Type, bool>(types.Length);
            foreach (var item in types)
            {
                localPermissions.Add(item,false);
            }

            return localPermissions;
        }
        
        #endregion
    }
}
