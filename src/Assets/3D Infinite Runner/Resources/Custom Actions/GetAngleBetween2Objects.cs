using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("XY angle")]

	public class xyAngleBetweenTwoPoints : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The game object whose forward axis we measure from. If the target is dead ahead the angle will be 0.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The target object to measure the angle to. Or use target position.")]
		public FsmGameObject targetObject;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the angle in a float variable.")]
		public FsmFloat storeAngle;
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		public override void Reset() {
			gameObject = null;
			targetObject = null;
			storeAngle = null;
			everyFrame = false;
		}
		
		public override void OnLateUpdate() {
			DoGetAngleToTarget();
			
			if (!everyFrame) {
				Finish();
			}
		}
		
		void DoGetAngleToTarget() {
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go == null) {
				return;
			}
			
			var goTarget = targetObject.Value;
			
			if (goTarget == null) {
				return;
			}
			
			float xDiff = goTarget.transform.position.x - go.transform.position.x; 
			float yDiff = goTarget.transform.position.y - go.transform.position.y; 
			storeAngle.Value =  Mathf.Atan2(yDiff, xDiff) * (180 / Mathf.PI);
			
		}
		
	}

}