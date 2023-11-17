using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Framework.Base.Movement
{
    public class ToPointMovementHelper
    {
        public delegate UniTask MovementAction(Transform item, MovementPointData pointData,
            CancellationToken cancellationToken);

        public bool IsMovementInProcess { get; set; }

        private MovementAction movementAction;

        public ToPointMovementHelper(MovementAction movementAction)
        {
            if (movementAction != null)
                this.movementAction = movementAction;
            else Debug.LogException(new ArgumentNullException(nameof(movementAction)));
        }

        public async UniTask Move(
            Transform item,
            MovementPointData movementPointData,
            CancellationToken cancellationToken = default)
        {
            IsMovementInProcess = true;
            await movementAction.Invoke(item, movementPointData, cancellationToken);
            IsMovementInProcess = false;
        }

        public void SetMoveAction(MovementAction action)
        {
            if (action != null)
                movementAction = action;
        }
    }
}