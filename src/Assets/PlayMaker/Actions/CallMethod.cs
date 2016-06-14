// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Call a method in a behaviour.")]
    public class CallMethod : FsmStateAction
    {
        [ObjectType(typeof(MonoBehaviour))]
        [Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
        public FsmObject behaviour;

        //[UIHint(UIHint.Method)]
        [Tooltip("Name of the method to call on the component")]
        public FsmString methodName;

        [Tooltip("Method paramters. NOTE: these must match the method's signature!")]
        public FsmVar[] parameters;

        [ActionSection("Store Result")]

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result of the method call.")]
        public FsmVar storeResult;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private FsmObject cachedBehaviour;
        private FsmString cachedMethodName;
        private Type cachedType;
        private MethodInfo cachedMethodInfo;
        private ParameterInfo[] cachedParameterInfo;
        private object[] parametersArray;
        private string errorString;
        
        public override void OnEnter()
        {
            parametersArray = new object[parameters.Length];

            DoMethodCall();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoMethodCall();
        }

        private void DoMethodCall()
        {
            if (behaviour.Value == null)
            {
                Finish();
                return;
            }

            if (NeedToUpdateCache())
            {
                if(!DoCache())
                {
                    Debug.LogError(errorString);
                    Finish();
                    return;
                }
            }

            object result = null;
            if (cachedParameterInfo.Length == 0)
            {
                result = cachedMethodInfo.Invoke(cachedBehaviour.Value, null);
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    parameter.UpdateValue();
                    parametersArray[i] = parameter.GetValue();
                }

                result = cachedMethodInfo.Invoke(cachedBehaviour.Value, parametersArray);
            }
            storeResult.SetValue(result);
        }

        // TODO: Move tests to helper function in core
        private bool NeedToUpdateCache()
        {
            return cachedBehaviour == null || cachedMethodName == null || // not cached yet
                cachedBehaviour.Value != behaviour.Value ||     // behavior value changed
                cachedBehaviour.Name != behaviour.Name ||       // behavior variable name changed
                cachedMethodName.Value != methodName.Value ||   // methodName value changed
                cachedMethodName.Name != methodName.Name;       // methodName variable name changed
        }

        private bool DoCache()
        {
            //Debug.Log("DoCache");

            errorString = string.Empty;
            cachedBehaviour = new FsmObject(behaviour);
            cachedMethodName = new FsmString(methodName);
            
            if (cachedBehaviour.Value == null)
            {
                if (behaviour.UsesVariable && !Application.isPlaying)
                {
                    // Value might be set at runtime
                    // Display/Log this info...?
                }
                else
                {
                    errorString += "Behaviour is invalid!\n";
                }
                Finish();
                return false;
            }
            
            cachedType = behaviour.Value.GetType();

#if NETFX_CORE
            cachedMethodInfo = cachedType.GetTypeInfo().GetDeclaredMethod(methodName.Value);
#else
            var types = new List<Type>(parameters.Length);
            foreach (var each in parameters)
            {
                types.Add(each.RealType);
            }

            cachedMethodInfo = cachedType.GetMethod(methodName.Value, types.ToArray());
#endif            
            if (cachedMethodInfo == null)
            {
                errorString += "Invalid Method Name or Parameters: " + methodName.Value + "\n";
                Finish();
                return false;
            }

            cachedParameterInfo = cachedMethodInfo.GetParameters();
            return true;
        }

        public override string ErrorCheck()
        {
            /* We could only error check if when we recache,
             * however NeedToUpdateCache() is not super robust
             * So for now we just recache every frame in editor
             * Need to test editor perf...
            if (!NeedToUpdateCache())
            {
                return errorString; // last error message
            }*/

            if (Application.isPlaying)
            {
                return errorString; // last error message
            }

            errorString = string.Empty;
            if (!DoCache())
            {
                return errorString;
            }

            if (parameters.Length != cachedParameterInfo.Length)
            {
                return "Parameter count does not match method.\nMethod has " + cachedParameterInfo.Length + " parameters.\nYou specified " +parameters.Length + " paramaters.";
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var paramType = p.RealType;
                var paramInfoType = cachedParameterInfo[i].ParameterType;
                if (!ReferenceEquals(paramType, paramInfoType ))
                {
                    return "Parameters do not match method signature.\nParameter " + (i + 1) + " (" + paramType + ") should be of type: " + paramInfoType;
                }
            }

            if (ReferenceEquals(cachedMethodInfo.ReturnType, typeof(void)))
            {
                if (!string.IsNullOrEmpty(storeResult.variableName))
                {
                    return "Method does not have return.\nSpecify 'none' in Store Result.";
                }
            }
            else if (!ReferenceEquals(cachedMethodInfo.ReturnType,storeResult.RealType))
            {
                return "Store Result is of the wrong type.\nIt should be of type: " + cachedMethodInfo.ReturnType;
            }

            return string.Empty;
        }
    }
}
