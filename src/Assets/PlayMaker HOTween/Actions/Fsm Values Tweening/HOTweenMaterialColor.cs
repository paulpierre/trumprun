// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate a material color using HOTween.")]
	public class HOTweenMaterialColor : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public PlugSetColor.ColorName colorName;
		
		public FsmMaterial material;
		
		public FsmColor end;
		
		public bool ResetColorOnExit;
		
		private Color originalColor;
		

		public override void Reset()
		{
			base.Reset ();
			colorName = PlugSetColor.ColorName._Color;
			material = null;
			end = null;
			ResetColorOnExit = false;
		}
		
		public override void OnExit()
		{
			if (_tweenProxy!=null)
			{
				return;
			}
			
			if (ResetColorOnExit )
			{
				if (material.Value != null)
				{
					material.Value.SetColor(colorName.ToString(), originalColor);
					
				}
			}
		}
		
		
		public override void OnEnter()
		{
			if (_tweenProxy!=null)
			{
				return;
			}
			
			if (ResetColorOnExit && material.Value!=null)
			{
				Debug.Log(colorName.ToString());
				originalColor = material.Value.GetColor(colorName.ToString());
				Debug.Log(originalColor);
			}
			
			FsmMaterialColorHOTween _proxy = new FsmMaterialColorHOTween();
			
			_proxy.material = material.Value;
			_proxy.Target = end.Value;
			_proxy.colorName = colorName;
			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}