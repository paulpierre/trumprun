using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GUI)]
    [Tooltip("Set the height and center values of a capsule collider component.")]
    public class SetColliderProperties : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(CapsuleCollider))]
        [Tooltip("Capsule Collider Object.")]
        public FsmOwnerDefault capsuleColliderObject;

        [RequiredField]
        [Tooltip("Capsule Collider Properties")]
        public FsmFloat height;
        public FsmVector3 center;

        private CapsuleCollider capsuleColliderComponent;

        public override void Reset()
        {
            capsuleColliderObject = null;
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(capsuleColliderObject);
            if (go == null)
            {
                return;
            }

            capsuleColliderComponent = go.GetComponent<CapsuleCollider>();
            if (capsuleColliderComponent == null)
            {
                return;
            }
            DoSetColor();
        }

        void DoSetColor()
        {
            capsuleColliderComponent.height = height.Value;
            capsuleColliderComponent.center = center.Value;
            Finish();
        }

    }
}