
using UnityEngine;
using UnityEditor;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
[CustomEditor(typeof (UCE.CosmicForge.LevelGeneration))]
public class LevelGenerationEditor : Editor
{

    // -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
    public override void OnInspectorGUI()
	{
	
		//store the object the original Inspector in a variable of the same type:
		LevelGeneration generator = (LevelGeneration)target;
  
		DrawDefaultInspector ();
		
		GUI.color = Color.red;
		GUILayout.Space(10);
		
		if (GUILayout.Button("Delete Terrain"))
		{
			generator.DeleteMap(true);
		}
		
		GUI.color = Color.green;
		GUILayout.Space(10);
		
		if (GUILayout.Button("Generate Terrain"))
		{
			generator.GenerateMap();
		}
    	   
    }
    
    // -----------------------------------------------------------------------------------
    
}

}