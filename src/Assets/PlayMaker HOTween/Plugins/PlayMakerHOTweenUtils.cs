using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using HutongGames.PlayMaker;

using Holoville.HOTween.Plugins;

public static class PlayMakerHOTweenUtils
{
	// nothing much for now
}

public abstract class FsmHOTweenProxy : object
{
	
	public delegate void HOTweenStepComplete ();
	public event HOTweenStepComplete OnHOTweenStepComplete;
	
	public delegate void HOTweenComplete ();
	public event HOTweenComplete OnHOTweenComplete;
	
	
	public string id;
	public bool isRelative;
	public bool speedBased;
	public bool From;
	public EaseType easing;
	public LoopType loopType;
	public int loopCount;
	public UpdateType updateType;
	
	
	
	protected Tweener _tweener;
	
	public void Kill ()
	{
		if (_tweener != null) {
			_tweener.Kill ();
		}
	}
	
	public int getCompletedLoops()
	{
		if (_tweener != null) {
			return _tweener.completedLoops;
		}
		return 0;
	}
	protected void _stepCompleteCB (TweenEvent data)
	{
		OnHOTweenStepComplete ();
	}
	
	protected void _completeCB (TweenEvent data)
	{
		OnHOTweenComplete ();
	}	
}
	

/// <summary>
/// Fsm float HOTween proxy
/// </summary>
public sealed class FsmFloatHOTween : FsmHOTweenProxy
{
	
	public float Target;
	public FsmFloat FsmResult;
	public float Current;

	
	public void start (float duration)
	{
		//Current = FsmResult.Value;	// otherwise starting again fails if the fsmResult is not reseted to the start value... d'oh...
		
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
	
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
		
}

/// <summary>
/// Fsm int HOTween proxy
/// </summary>
public sealed class FsmIntHOTween : FsmHOTweenProxy
{
	public int Target;
	public FsmInt FsmResult;
	public int Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}


/// <summary>
/// Fsm string HOTween proxy
/// </summary>
public sealed class FsmStringHOTween : FsmHOTweenProxy
{
	public string Target;
	public FsmString FsmResult;
	public string Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}

/// <summary>
/// Fsm vector2 HOTween proxy
/// </summary>
public sealed class FsmVector2HOTween : FsmHOTweenProxy
{
	public Vector2 Target;
	public FsmVector2 FsmResult;
	public Vector2 Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}

/// <summary>
/// Fsm vector3 HOTween proxy
/// </summary>
public sealed class FsmVector3HOTween : FsmHOTweenProxy
{
	public Vector3 Target;
	public FsmVector3 FsmResult;
	public Vector3 Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}


/// <summary>
/// Fsm Material color HOTween proxy
/// </summary>
public sealed class FsmMaterialColorHOTween : FsmHOTweenProxy
{
	public Color Target;
	public Material material;
	public PlugSetColor.ColorName colorName;
	
		
	public void start (float duration)
	{		
		PlugSetColor plug = new PlugSetColor(Target,easing,isRelative);
		plug.Property(colorName);
		
		
		if (From) {
			
		
			_tweener = HOTween.From (material, duration, new TweenParms ().Prop ("color", plug)
				.Id(id)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
	
			_tweener = HOTween.To (material, duration, new TweenParms ().Prop ("color", plug)
				.Id(id)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
}



/// <summary>
/// Fsm color HOTween proxy
/// </summary>
public sealed class FsmColorHOTween : FsmHOTweenProxy
{
	public Color Target;
	public FsmColor FsmResult;
	public Color Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}


/// <summary>
/// Fsm Quaternion HOTween proxy
/// </summary>
public sealed class FsmQuaternionHOTween : FsmHOTweenProxy
{
	public Quaternion Target;
	public FsmQuaternion FsmResult;
	public Quaternion Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}


/// <summary>
/// Fsm Rect HOTween proxy
/// </summary>
public sealed class FsmRectHOTween : FsmHOTweenProxy
{
	public Rect Target;
	public FsmRect FsmResult;
	public Rect Current;
		
	public void start (float duration)
	{
		//Current = FsmResult.Value;	
		FsmResult.Value = Current;
		
		if (From) {
			
			_tweener = HOTween.From (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
			
		}else{
			
			_tweener = HOTween.To (this, duration, new TweenParms ().Prop ("Current", Target, isRelative)
				.Id(id)
				.OnUpdate (_updateCB)
				.OnComplete (_completeCB)
				.OnStepComplete (_stepCompleteCB)
				.Ease(easing)
				.Loops(loopCount,loopType)
				.UpdateType(updateType)
				.SpeedBased(speedBased)
				);
		}		
	}
		
	private void _updateCB (TweenEvent data)
	{
		FsmResult.Value = Current;
	}
			
}