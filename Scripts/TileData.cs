using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class TileData {

	public float[,]  heightMap;
	public float[,]  heatMap;
	public float[,]  moistureMap;
	public float[,]  difficultyMap;
	public HeightTemplate[,] chosenHeightTypes;
	public TemperatureTemplate[,] chosenHeatTypes;
	public MoistureTemplate[,] chosenMoistureTypes;
	public DifficultyTemplate[,] chosenDifficultyTypes;
	public BiomeTemplate[,] chosenBiomes;
	public Mesh mesh;
	public Texture2D texture;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public TileData(float[,]  heightMap, float[,]  heatMap, float[,]  moistureMap, float[,]  difficultyMap, 
		HeightTemplate[,] chosenHeightTypes, TemperatureTemplate[,] chosenHeatTypes, MoistureTemplate[,] chosenMoistureTypes, DifficultyTemplate[,] chosenDifficultyTypes,
		BiomeTemplate[,] chosenBiomes, Mesh mesh, Texture2D texture) {
		
		this.heightMap 				= heightMap;
		this.heatMap 				= heatMap;
		this.moistureMap 			= moistureMap;
		this.chosenHeightTypes 		= chosenHeightTypes;
		this.chosenHeatTypes 		= chosenHeatTypes;
		this.chosenMoistureTypes 	= chosenMoistureTypes;
		this.chosenDifficultyTypes	= chosenDifficultyTypes;
		this.chosenBiomes 			= chosenBiomes;
		this.mesh 					= mesh;
		this.texture 				= texture;
	}
	
}

}