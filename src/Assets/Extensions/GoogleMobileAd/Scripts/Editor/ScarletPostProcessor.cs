//#define CODE_DISABLED

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/*
	A simple, shared PostProcessBuildPlayer script to enable Objective-C modules. This lets us add frameworks
	from our source files, rather than through modifying the Xcode project. 
*/

public class ScarletPostProcessor {

    [PostProcessBuild(1500)] // We should try to run last
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		#if UNITY_IPHONE


		UnityEngine.Debug.Log("ScarletPostProcessor: Enabling Objective-C modules");
		string pbxproj = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

		// Looking for the buildSettings sections of the pbxproj
		string insertKeyword = "buildSettings = {";
		string foundKeyword = "CLANG_ENABLE_MODULES"; // for checking if it's already inserted
		string modulesFlag = "				CLANG_ENABLE_MODULES = YES;";

		List<string> lines = new List<string>();
			
		foreach (string str in File.ReadAllLines(pbxproj)) {
			if (!str.Contains(foundKeyword)) { 
				lines.Add(str);
			}
			if (str.Contains(insertKeyword)) {
				lines.Add(modulesFlag);
			}
		}
			
		// Clear the file
		// http://stackoverflow.com/questions/16212127/add-a-new-line-at-a-specific-position-in-a-text-file
		using (File.Create(pbxproj)) {}

		foreach (string str in lines) {
			File.AppendAllText(pbxproj, str + Environment.NewLine);
		}

		#endif
	}
	
}
