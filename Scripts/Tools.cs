using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================

public static class Tools {
	
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public static float Evaluate(float value) {
		float a = 3;
		float b = 2.2f;
		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------	
	public static float[,] BlendMaps(int tileSize, float[,] map1, float[,] map2) {
	
		float[,] blendMap = new float[tileSize, tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				
				// mix both heat maps together by multiplying their values
				blendMap [zIndex, xIndex] = map1 [zIndex, xIndex] * map2 [zIndex, xIndex];
				
			}
		}
		
		return blendMap;
		
	}
	
}

// =======================================================================================


}