using System.IO;
using UnityEditor;
using UnityEngine;
using SimpleJSON;

namespace SIS
{
    [InitializeOnLoad]
    public class ReviewWindowEditor : EditorWindow
    {
		private static Texture2D reviewWindowImage;
		private static string imagePath = "/EditorFiles/Asset_smallLogo.png";
		private static string keyName = "SimpleIAPSystem-Review";

		
        static ReviewWindowEditor()
        {
			EditorApplication.update += Startup;
		}
		
		
		static void Startup()
		{
			EditorApplication.update -= Startup;
			
			if (!EditorPrefs.HasKey(keyName))
			{
                JSONNode data = new JSONClass();
				data["active"].AsBool = true;
				EditorPrefs.SetString(keyName, data.ToString());
			}
			
			Count();
		}


        [MenuItem("Window/Simple IAP System/Review Asset")]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(ReviewWindowEditor), new Rect(0, 0, 256, 260), false, "Review Window");
        }


        void OnGUI()
        {		
			if(reviewWindowImage == null)
			{
				var script = MonoScript.FromScriptableObject(this);
				string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
				reviewWindowImage = AssetDatabase.LoadAssetAtPath(path + imagePath, typeof(Texture2D)) as Texture2D;
			}
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(31);
			GUILayout.Label(reviewWindowImage);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(48);
			EditorGUILayout.LabelField("Review Simple IAP System", GUILayout.Width(160));
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please consider giving us a rating on the");
            EditorGUILayout.LabelField("Unity Asset Store. Your support helps us");
			EditorGUILayout.LabelField("to improve this product. Thank you!");

			GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Rate now!"))
            {
				Help.BrowseURL("https://www.assetstore.unity3d.com/#!/content/12343");
                DisableRating();
            }
            if (GUILayout.Button("Later"))
            {
				JSONNode data = new JSONClass();
				data = SimpleJSON.JSON.Parse(EditorPrefs.GetString(keyName));
				data["counter"].AsInt = 5;
				
				EditorPrefs.SetString(keyName, data.ToString());
				this.Close();
            }
			if (GUILayout.Button("Never"))
            {
                DisableRating();
            }
            EditorGUILayout.EndHorizontal();
        }
		
		
		static void Count()
		{
			JSONNode data = new JSONClass();
			data = SimpleJSON.JSON.Parse(EditorPrefs.GetString(keyName));
			
			if(data["active"].AsBool == false)
				return;
			
			double time = GenerateUnixTime();
			double diff = time - data["lastCheck"].AsDouble;
			if(diff < 20) return;
			
			data["lastCheck"].AsDouble = time;
			data["counter"].AsInt = data["counter"].AsInt + 1;
			EditorPrefs.SetString(keyName, data.ToString());
						
			if(data["counter"].AsInt > 5)
				Init();
		}
		
		
        static double GenerateUnixTime()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (System.DateTime.UtcNow - epochStart).TotalHours;
        }
	

        void DisableRating()
        {
			JSONNode data = new JSONClass();
			data = SimpleJSON.JSON.Parse(EditorPrefs.GetString(keyName));
			
			data["active"].AsBool = false;
			data["counter"].AsInt = 0;
			EditorPrefs.SetString(keyName, data.ToString());
			this.Close();
        }
    }
}