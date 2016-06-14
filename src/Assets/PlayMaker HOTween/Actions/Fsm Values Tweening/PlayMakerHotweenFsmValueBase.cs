// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	public class PlayMakerHotweenFsmValueBase : PlayMakerHOTweenAction
	{
		[ActionSection("Tween Set up")]
		
		[Tooltip("The reference is you want to control it from elsewhere.")]
		public FsmString id;
		
		[Tooltip("The seconds of duration of the tween.")]
		public FsmFloat duration;
		
		[Tooltip("If true, ")]
		public FsmBool SpeedBased;
		
		[Tooltip("If true, tween from the end value to the start value.")]
		public HOTweenDirection direction;
		
		[Tooltip("If true, considers the endValue as relative instead than absolute (meaning it will animate BY instead than animate TO.")]
		public FsmBool isRelative;
		
		public EaseType easing;
		
		public LoopType loopType;
		
		public FsmInt loopCount;
		
		public UpdateType updateType;
		
		public FsmBool StopOnExit;
		
		[ActionSection("Feedback")]
		
		[UIHint(UIHint.Variable)]
		public FsmInt stepCompleted;
		
		
		public FsmEvent onStepComplete;
		public FsmEvent onComplete;		
		
		public override void Reset()
		{
			id = "";
			duration = 1;
			SpeedBased = false;
			isRelative = false;
			easing = HOTween.defEaseType;
			loopType = HOTween.defLoopType;
			loopCount = -1;
			updateType = HOTween.defUpdateType;
			
			stepCompleted = null;
			
			onStepComplete = null;
			onComplete = null;
			
			StopOnExit = false;
		}
		
		
		protected FsmHOTweenProxy _tweenProxy;
		
		protected void SetupTween()
		{	
			_tweenProxy.From = direction == HOTweenDirection.From;
			_tweenProxy.id = id.Value;
			_tweenProxy.isRelative = isRelative.Value;
			_tweenProxy.easing = easing;
			_tweenProxy.loopType = loopType;
			_tweenProxy.loopCount = loopCount.Value;
			_tweenProxy.speedBased = SpeedBased.Value;
			_tweenProxy.updateType = updateType;

			_tweenProxy.OnHOTweenComplete += onCompleteCB;
			_tweenProxy.OnHOTweenStepComplete += onStepCompleteCB;
		}
		
		public override void OnExit()
		{
			if (_tweenProxy !=null && StopOnExit.Value)
			{
				_tweenProxy.Kill();
				_tweenProxy = null;
			}
				
		}
		
		public void onStepCompleteCB()
		{
			stepCompleted.Value = _tweenProxy.getCompletedLoops();
			
			Fsm.Event(onStepComplete);
		}
		
		public void onCompleteCB()
		{
			_tweenProxy.Kill();
			_tweenProxy = null;
			
			Fsm.Event(onComplete);
			Finish();
		}

	}

	
}