using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// TextureTemplate
// =======================================================================================
[CreateAssetMenu(fileName = "Untitled Textures", menuName = "Cosmic Forge/New Textures", order = 999)]
public class TextureTemplate : ScriptableObject {
	
	const int textureSize = 512;
	const TextureFormat textureFormat = TextureFormat.RGB565;
	
	public Material material;
	public Layer[] layers;

	private float savedMinHeight;
	private float savedMaxHeight;
	
	// -----------------------------------------------------------------------------------
	// OnValidate
	// -----------------------------------------------------------------------------------
	protected void OnValidate() {
		if (material == null || layers.Length == 0) return;
		
		layers.OrderBy(x => x.heightType.threshold);	
		ApplyToMaterial (material);
		UpdateMeshHeights (0, 1);
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void ApplyToMaterial(Material material) {
		
		material.SetInt 		("layerCount", layers.Length);
		material.SetColorArray 	("baseColours", layers.Select(x => x.tint).ToArray());
		material.SetFloatArray 	("baseStartHeights", layers.Select(x => x.heightType.threshold).ToArray());
		//material.SetFloatArray ("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
		material.SetFloatArray 	("baseBlends", layers.Select(x => x.blendStrength).ToArray());
		material.SetFloatArray 	("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
		material.SetFloatArray 	("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
		Texture2DArray texturesArray = GenerateTextureArray (layers.Select (x => x.texture).ToArray ());
		material.SetTexture ("baseTextures", texturesArray);

		UpdateMeshHeights (savedMinHeight, savedMaxHeight);
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void UpdateMeshHeights(float minHeight, float maxHeight) {
		savedMinHeight = minHeight;
		savedMaxHeight = maxHeight;
		material.SetFloat ("minHeight", minHeight);
		material.SetFloat ("maxHeight", maxHeight);
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	Texture2DArray GenerateTextureArray(Texture2D[] textures) {
		Texture2DArray textureArray = new Texture2DArray (textureSize, textureSize, textures.Length, textureFormat, true);
		for (int i = 0; i < textures.Length; i++) {
			textureArray.SetPixels (textures [i].GetPixels (), i);
		}
		textureArray.Apply ();
		return textureArray;
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public class Layer {
		public Texture2D texture;
		public Color tint;
		[Range(0,1)] public float tintStrength;
		//[Range(0,1)] public float startHeight;
		[Range(0,1)] public float blendStrength;
		public float textureScale;
		
		public HeightTemplate heightType;
		
		
	}
	// -----------------------------------------------------------------------------------
	 
}

}