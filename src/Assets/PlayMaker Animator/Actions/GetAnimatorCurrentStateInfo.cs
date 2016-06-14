// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Gets the current State information on a specified layer")]
	public class GetAnimatorCurrentStateInfo : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The layer's index")]
		public FsmInt layerIndex;
		
		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;
		
		[ActionSection("Results")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name.")]
		public FsmString name;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		public FsmInt nameHash;
		
		#if UNITY_5
		[UIHint(UIHint.Variable)]
		[Tooltip("The full path hash for this state.")]
		public FsmInt fullPathHash;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The name Hash. Doest not include the parent layer's name")]
		public FsmInt shortPathHash;
		#endif

		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's tag hash")]
		public FsmInt tagHash;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Is the state looping. All animations in the state must be looping")]
		public FsmBool isStateLooping;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		public FsmFloat length;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		public FsmFloat normalizedTime;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		public FsmInt loopCount;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		public FsmFloat currentLoopProgress;
		
		private PlayMakerAnimatorMoveProxy _animatorProxy;
		
		private Animator _animator;
		
		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			
			name = null;
			nameHash = null;

			#if UNITY_5
			fullPathHash = null;
			shortPathHash = null;
			#endif

			tagHash = null;
			length = null;
			normalizedTime = null;
			isStateLooping = null;
			loopCount = null;
			currentLoopProgress = null;
			
			everyFrame = false;
		}

		public override void OnEnter()
		{
			// get the animator component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go==null)
			{
				Finish();
				return;
			}
			
			_animator = go.GetComponent<Animator>();
			
			if (_animator==null)
			{
				Finish();
				return;
			}
			
			_animatorProxy = go.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy!=null)
			{		
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
			
			GetLayerInfo();
			
			if (!everyFrame) 
			{
				Finish();
			}
		}
	
		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				GetLayerInfo();
			}
		}
		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy!=null)
			{
				GetLayerInfo();
			}
		}
		
		void GetLayerInfo()
		{		
			if (_animator!=null)
			{
				AnimatorStateInfo _info = _animator.GetCurrentAnimatorStateInfo(layerIndex.Value);

				#if UNITY_5
				fullPathHash.Value = _info.fullPathHash;
				shortPathHash.Value = _info.shortNameHash;
				nameHash.Value = _info.shortNameHash;
				#else
				nameHash.Value = _info.nameHash;
				#endif


				if (!name.IsNone)
				{
					name.Value = _animator.GetLayerName(layerIndex.Value);	
				}
				
				tagHash.Value = _info.tagHash;
				length.Value = _info.length;
				isStateLooping.Value = _info.loop;
				normalizedTime.Value = _info.normalizedTime;
				if (!loopCount.IsNone || !currentLoopProgress.IsNone)
				{
					loopCount.Value = (int)System.Math.Truncate(_info.normalizedTime);
					currentLoopProgress.Value = _info.normalizedTime-loopCount.Value;
				}
				
				
			
			}
		}
		
		public override void OnExit()
		{
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorMoveEvent -= OnAnimatorMoveEvent;
			}
		}	
	}
}