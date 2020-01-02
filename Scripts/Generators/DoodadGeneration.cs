using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class DoodadGeneration : MonoBehaviour {
	
	public bool active;
	
	[SerializeField] private  LevelGeneration levelGeneration;
	[SerializeField] private NoiseMapGeneration noiseMapGeneration;
	
	[SerializeField] private float doodadScale;
	[SerializeField] private WaveTemplate[] waves;
	[SerializeField] private DoodadRow[] doodads;
	
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
	private GameObject root;
	private float distanceBetweenVertices;
	private Vector3[] meshVertices;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void Generate(int mapSize, float _distanceBetweenVertices, LevelData levelData) {
		
		if (!active) return;
		
		distanceBetweenVertices = _distanceBetweenVertices;
		
		for (int i = 0; i < doodads.Length; i++) {
			
			// generate a root object
			root = new GameObject("DoodadRoot_"+i.ToString());
			root.transform.parent = this.gameObject.transform;
		
			// select doodads to generate
			DoodadRow chosenDoodads = doodads[i];
	
			// generate a tree noise map using Perlin Noise
			float[,] treeMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (mapSize, levelData.scale, 0, 0, this.waves);

			//float levelSizeX = mapSize * distanceBetweenVertices;
		
			for (int zIndex = 0; zIndex < mapSize; zIndex++) {
				for (int xIndex = 0; xIndex < mapSize; xIndex++) {
			
					// convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
					TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate (zIndex, xIndex);
					TileData tileData = levelData.tilesData [tileCoordinate.tileZIndex, tileCoordinate.tileXIndex];
					int tileWidth = tileData.heightMap.GetLength (1);

					// calculate the mesh vertex index
					meshVertices = tileData.mesh.vertices;
					int vertexIndex = tileCoordinate.coordinateZIndex * tileWidth + tileCoordinate.coordinateXIndex;
				
					// sample all map data at this coordinate
					HeightTemplate 			heightType 		= tileData.chosenHeightTypes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];
					TemperatureTemplate 	heatType 		= tileData.chosenHeatTypes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];
					MoistureTemplate 		moistureType 	= tileData.chosenMoistureTypes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];
					DifficultyTemplate 		difficultyType 	= tileData.chosenDifficultyTypes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];
					BiomeTemplate			biomeType 		= tileData.chosenBiomes [tileCoordinate.coordinateZIndex, tileCoordinate.coordinateXIndex];

				
					// choose doodad template (if any)
					DoodadTemplate template = null;
					System.Random rnd = new System.Random();
					List<DoodadTemplate> finalDoodads = new List<DoodadTemplate>();
				
					foreach (DoodadTemplate tmpl in chosenDoodads.doodads) {
						if (tmpl.ValidateTerrainType(heightType, heatType, moistureType, difficultyType, biomeType)) {
							finalDoodads.Add(tmpl);
						}
					}
				
					if (finalDoodads.Count > 0)
						template = finalDoodads[rnd.Next(finalDoodads.Count)];
				
					// validate doodad
					if (template != null ) {
				
						float treeValue = treeMap [zIndex, xIndex];

						//int terrainTypeIndex = terrainType.index;

						// compares the current tree noise value to the neighbor ones
						int neighborZBegin 	= (int)Mathf.Max (0, 				zIndex - template.neighborRadius * levelGeneration.scaleMultiplier );
						int neighborZEnd 	= (int)Mathf.Min (mapSize-1, 		zIndex + template.neighborRadius * levelGeneration.scaleMultiplier );
						int neighborXBegin 	= (int)Mathf.Max (0, 				xIndex - template.neighborRadius * levelGeneration.scaleMultiplier );
						int neighborXEnd 	= (int)Mathf.Min (mapSize-1, 		xIndex + template.neighborRadius * levelGeneration.scaleMultiplier );
					
						float maxValue = 0f;
					
						for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
							for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
								float neighborValue = treeMap [neighborZ, neighborX];
								// saves the maximum tree noise value in the radius
								if (neighborValue >= maxValue) {
									maxValue = neighborValue;
								}
							}
						}

						// if the current tree noise value is the maximum one, place a tree in this location
						if (treeValue == maxValue)
							InstantiateDoodad(template, zIndex, xIndex, vertexIndex);
					
					}
				}
			
			}
		
		}
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void InstantiateDoodad(DoodadTemplate template, int zIndex, int xIndex, int vertexIndex) {
		
		float xPos						= xIndex * distanceBetweenVertices + Random.Range(-0.05f, 0.05f);
		float zPos						= zIndex * distanceBetweenVertices + Random.Range(-0.05f, 0.05f);
		
		RaycastHit hit;
		
		if (Physics.Raycast(new Vector3(xPos, 5f, zPos), -Vector3.up, out hit)) {
		
			Vector3 doodadRotation 			= new Vector3(0, Random.Range(0, 360), 0);
			Vector3 doodadPosition			= hit.point;
			doodadPosition.y 				-= template.yAxisCorrection;
			
			GameObject doodad 				= Instantiate (template.gameObject, doodadPosition, Quaternion.Euler(doodadRotation)) as GameObject;
			doodad.transform.parent 		= root.transform;
			doodad.transform.localScale 	= new Vector3 (doodadScale, doodadScale, doodadScale);
			
 		}
		
	}
	
	// -----------------------------------------------------------------------------------
	
}

}