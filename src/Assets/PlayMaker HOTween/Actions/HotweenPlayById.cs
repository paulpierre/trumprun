// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Play a predefine HOTween tween by its ID.")]
	public class HotweenPlayById: PlayMakerHOTweenAction
	{
		
		public enum PlayType {play,playForward,playBackward};
		
		public enum OnExitAction {Nothing,PauseOnExit,RewindOnExit};
		
		public enum OnStartAction {Default,RewindIfCompleted,RewindIfPlaying,alwaysRewind}
		
		public FsmString tweenID;
		
		[Tooltip("optionnally constraint on a target to look for the tween ID")]
		//public FsmOwnerDefault target;
		
		
		public PlayType playType;
		
		public OnStartAction startAction;

		
		public OnExitAction onExit;
		
		[ActionSection("Results")]
		
		[UIHint(UIHint.Variable)]
		public FsmInt completedLoops;
		
			
		[ActionSection("Events")]
		public FsmEvent completed;
		public FsmEvent failed;
		
		
		private IHOTweenComponent tween;
		
		
		public override void Reset()
		{
			tweenID = null;	
		//	target = new FsmOwnerDefault();
		//	target.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
			completed = null;
			failed = null;
			playType = PlayType.play;
			onExit = OnExitAction.Nothing;
			startAction = OnStartAction.Default;
		}
		
		public override void OnExit()
		{
			if (tween==null || onExit == OnExitAction.Nothing )
			{
				return;
			}
			
			if (onExit == OnExitAction.RewindOnExit)
			{
				tween.Rewind();
			}else if (onExit == OnExitAction.PauseOnExit)
			{
				tween.Pause();
			}
		}
		
		
		public override void OnEnter()
		{
//		var goTarget = Fsm.GetOwnerDefaultTarget(target);
			
			List<IHOTweenComponent> tweens = new List<IHOTweenComponent>();
			
			/*
			if (goTarget != null)
			{
				Debug.Log("looking for tweens by target :"+goTarget.name);
				List<Tweener> tweeners =	HOTween. .GetTweenersByTarget(goTarget.transform,true);
				foreach (Tweener tweener in tweeners)
				{
					Debug.Log(tweener.id+ " "+tweenID.Value);
					
					if (string.Equals(tweener.id,tweenID.Value))
					{
						tweens.Add(tweener);
					}
				}
			}else{
				
		   		tweens =	HOTween.GetTweensById(tweenID.Value,false);
				
			}
			*/

			tweens =	HOTween.GetTweensById(tweenID.Value,false);
			
			if (tweens.Count ==0)
			{
				LogWarning("HOTween "+tweenID.Value+" not found");
				Fsm.Event(failed);
				Finish();
				return;
			}else{
				tween = tweens[0];
				tween.autoKillOnComplete = false;
				
				
			
				if (tween.isComplete)
				{
					if (startAction == OnStartAction.RewindIfCompleted || startAction == OnStartAction.alwaysRewind) 
					{
						tween.Rewind();
					}
					
				}
				
				if (tween.hasStarted) 
				{
					if (startAction == OnStartAction.RewindIfPlaying || startAction == OnStartAction.alwaysRewind) 
					{
						tween.Rewind();
					}
				}
				
				if  (playType == PlayType.playForward)
				{
					tween.PlayForward();
				}else if  (playType == PlayType.playBackward)
				{
					tween.PlayBackwards();
				}else{
					tween.Play();
				}
				
			}
			
			if (completed == null)
			{
				Debug.Log("finished");
				Finish();	
			}else{
				if (tween.loops ==-1 && completed!=null)
				{
					LogWarning("tween "+tweenID.Value+" is looping to infinite, the completed event will never be fired");
				}
			}
		}
		
		public override void OnUpdate()
		{
			if (tween!=null)
			{
				completedLoops.Value = tween.completedLoops;
				
				if (tween.isComplete)
				{
					Fsm.Event(completed);
				}
			}
		}


	}
}