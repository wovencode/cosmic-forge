using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class TileGeneration : MonoBehaviour {

	[Header("-=-=- Tile Settings -=-=-")]
	[SerializeField] private Terrain terrain;
	[SerializeField] private NoiseMapGeneration noiseMapGeneration;
	[SerializeField] private LevelGeneration levelGeneration;
	[SerializeField] private MeshRenderer tileRenderer;
	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private MeshCollider meshCollider;
	[SerializeField] private Material material;
	
	
	private LevelGeneration root;

	private float  maxDistance, vertexOffset;
	private float offsetX, offsetZ;
	private Vector3[] meshVertices;
	private Texture2D heightTexture, heatTexture, moistureTexture, difficultyTexture, biomeTexture;
	private float[,] heightMap, heatMap, moistureMap, difficultyMap, biomeMap;
	private int tileSize;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public TileData GenerateTile(LevelGeneration _levelGeneration) {
		
		// setup dependencies
		root = _levelGeneration;
		noiseMapGeneration.root = root;
		
		// calculate tile depth and width based on the mesh vertices
		meshVertices = meshFilter.sharedMesh.vertices;
		tileSize = (int)Mathf.Sqrt (meshVertices.Length);
		
		// calculate the offsets based on the tile position
		offsetX = -this.gameObject.transform.position.x;
		offsetZ = -this.gameObject.transform.position.z;


		// calculate vertex offset based on the Tile position and the distance between vertices
		Vector3 tileDimensions = meshFilter.sharedMesh.bounds.size;
		float distanceBetweenVertices = tileDimensions.z / (float)tileSize;
		vertexOffset = this.gameObject.transform.position.z / distanceBetweenVertices;
		
		GenerateHeightMap();
		GenerateHeatMap();
		GenerateMoistureMap();
		GenerateDifficultyMap();
		
		// ------- build all textures ---------
		
		
		// build a Texture2D from the height map
		HeightTemplate[,] chosenHeightTypes = new HeightTemplate[tileSize, tileSize];
		heightTexture = BuildTexture (heightMap, root.heightSettings.templates, chosenHeightTypes);
		
		// build a Texture2D from the heat map
		TemperatureTemplate[,] chosenHeatTypes = new TemperatureTemplate[tileSize, tileSize];
		heatTexture = BuildTexture (heatMap, root.temperatureSettings.templates, chosenHeatTypes);
		
		// build a Texture2D from the moisture map
		MoistureTemplate[,] chosenMoistureTypes = new MoistureTemplate[tileSize, tileSize];
		moistureTexture = BuildTexture (moistureMap, root.moistureSettings.templates, chosenMoistureTypes);

		// build a Texture2D from the difficulty map
		DifficultyTemplate[,] chosenDifficultyTypes = new DifficultyTemplate[tileSize, tileSize];
		difficultyTexture = BuildTexture (difficultyMap, root.difficultySettings.templates, chosenDifficultyTypes);

		// build a biomes Texture2D from the three other noise variables
		BiomeTemplate[,] chosenBiomes = new BiomeTemplate[tileSize, tileSize];
		biomeTexture = BuildBiomeTexture(chosenHeightTypes, chosenHeatTypes, chosenMoistureTypes, chosenDifficultyTypes, chosenBiomes);
		
		// ------- 
		
		// update the visualization mode
		UpdateVisualizationMode();

		


		// update the tile mesh vertices according to the height map
		UpdateMeshVertices (heightMap);

		TileData tileData = new TileData (heightMap, heatMap, moistureMap, difficultyMap,
			chosenHeightTypes, chosenHeatTypes, chosenMoistureTypes, chosenDifficultyTypes, chosenBiomes, 
			meshFilter.mesh, (Texture2D)this.tileRenderer.sharedMaterial.mainTexture);

		return tileData;
		
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	private Texture2D BuildTexture(float[,] heightMap, SimpleTemplate[] terrainTypes, SimpleTemplate[,] chosenTerrainTypes) {
	
		int tileSize = heightMap.GetLength (0);
		
		Color[] colorMap = new Color[tileSize * tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
			
				// transform the 2D map index is an Array index
				int colorIndex = zIndex * tileSize + xIndex;
				float height = heightMap [zIndex, xIndex];
				// choose a terrain type according to the height value
				SimpleTemplate terrainType = ChooseTerrainType (height, terrainTypes);
				// assign the color according to the terrain type
				colorMap[colorIndex] = terrainType.color;

				// save the chosen terrain type
				chosenTerrainTypes [zIndex, xIndex] = terrainType;
				
			}
		}

		// create a new texture and set its pixel colors
		Texture2D tileTexture = new Texture2D (tileSize, tileSize);
		tileTexture.wrapMode = TextureWrapMode.Clamp;
		tileTexture.SetPixels (colorMap);
		tileTexture.Apply ();

		return tileTexture;
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	SimpleTemplate ChooseTerrainType(float noise, SimpleTemplate[] terrainTypes) {
	
		//terrainTypes.OrderBy(x => x.threshold);
		
		
		
		// for each terrain type, check if the height is lower than the one for the terrain type
		
		
		
		foreach (SimpleTemplate terrainType in terrainTypes) {
			// return the first terrain type whose height is higher than the generated one
			if (noise < terrainType.threshold) {
				return terrainType;
			}
		}
		return terrainTypes [terrainTypes.Length - 1];
		
		
		
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	private void UpdateMeshVertices(float[,] heightMap) {
	
		int tileSize = heightMap.GetLength (0);
		
		// iterate through all the heightMap coordinates, updating the vertex index
		int vertexIndex = 0;
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				float height = heightMap [zIndex, xIndex];

				Vector3 vertex = meshVertices [vertexIndex];
				// change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
				meshVertices[vertexIndex] = new Vector3(vertex.x, root.heightSettings.curve.Evaluate(height) * root.heightMultiplier, vertex.z);

				vertexIndex++;
			}
		}

		// update the vertices in the mesh and update its properties
		meshFilter.mesh.vertices = meshVertices;
		meshFilter.mesh.RecalculateBounds ();
		meshFilter.mesh.RecalculateNormals ();
		// update the mesh collider
		this.meshCollider.sharedMesh = meshFilter.mesh;
		
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	private Texture2D BuildBiomeTexture(HeightTemplate[,] heightTypes, TemperatureTemplate[,] heatTypes, MoistureTemplate[,] moistureTypes, DifficultyTemplate[,] difficultyTypes,  BiomeTemplate[,] chosenBiomes) {
		
		int tileSize = heatTypes.GetLength (0);
		
		Color[] colorMap = new Color[tileSize * tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				int colorIndex = zIndex * tileSize + xIndex;

				HeightTemplate heightType = heightTypes [zIndex, xIndex];
				
				// check if the current coordinate is a water region
				//if (heightTerrainType.name != "water") {
				
					// a coordinates biome will be defined by the heat and moisture values
					TemperatureTemplate heatType 		= heatTypes [zIndex, xIndex];
					MoistureTemplate moistureType 		= moistureTypes [zIndex, xIndex];
					DifficultyTemplate difficultyType 	= difficultyTypes [zIndex, xIndex];
					
					// choose biome template (if any)
					System.Random rnd = new System.Random();
					List<BiomeTemplate> finalBiomes = new List<BiomeTemplate>();
				
					foreach (BiomeTemplate tmpl in root.biomeSettings.templates) {
						if (tmpl.ValidateTerrainType(heightType, heatType, moistureType, difficultyType))
							finalBiomes.Add(tmpl);
					}
					
					BiomeTemplate biome = finalBiomes[rnd.Next(finalBiomes.Count)];
					
					// assign the color according to the selected biome
					colorMap [colorIndex] = biome.color;

					// save biome in chosenBiomes matrix only when it is not water
					chosenBiomes [zIndex, xIndex] = biome;
					
				//} else {
					// water regions don't have biomes, they always have the same color
				//	colorMap [colorIndex] = this.waterColor;
				//}
			}
		}

		// create a new texture and set its pixel colors
		Texture2D tileTexture = new Texture2D (tileSize, tileSize);
		tileTexture.wrapMode = TextureWrapMode.Clamp;
		tileTexture.SetPixels (colorMap);
		tileTexture.Apply ();

		return tileTexture;
	}
	
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void UpdateVisualizationMode() {
	
		switch (root.visualizationMode) {
		
		case VisualizationMode.Height:
			this.tileRenderer.material.mainTexture = heightTexture;
			break;
		case VisualizationMode.Heat:
			this.tileRenderer.material.mainTexture = heatTexture;
			break;
		case VisualizationMode.Moisture:
			this.tileRenderer.material.mainTexture = moistureTexture;
			break;
		case VisualizationMode.Difficulty:
			this.tileRenderer.material.mainTexture = difficultyTexture;
			break;
		case VisualizationMode.Biome:
			this.tileRenderer.material.mainTexture = biomeTexture;
			break;
		case VisualizationMode.Material:
			this.tileRenderer.material = material;
			break;
			
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateHeightMap
	// -----------------------------------------------------------------------------------
	public void GenerateHeightMap()
	{
		
		// -- generate height map
		float scale = root.scaleMultiplier * root.heightSettings.scaleModifier;
		
		
		heightMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileSize, scale, offsetX, offsetZ, root.heightSettings.waves);
		
		// -- generate falloff map
		
		if (root.heightSettings.fallOff) {
			
			
			
			//float[,] uniformMap1 = this.noiseMapGeneration.GenerateUniformNoiseMap(tileSize, scale, offsetX, offsetZ, true);

			float[,] uniformMap1 = this.noiseMapGeneration.GenerateGradientCircleMap (tileSize, 1, offsetX, offsetZ, true);
			// mix both height+falloff maps together by multiplying their values
			heightMap = Tools.BlendMaps(tileSize, uniformMap1, heightMap);
			//heightMap = uniformMap1; 
			
		}
		
		
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateHeatMap
	// -----------------------------------------------------------------------------------
	public void GenerateHeatMap()
	{
	
		float scale = root.scaleMultiplier * root.temperatureSettings.scaleModifier;
		
		float[,] uniformHeatMap = this.noiseMapGeneration.GenerateUniformNoiseMapZ(tileSize, scale, vertexOffset);
		
		// generate a heatMap using Perlin Noise
		float[,] randomHeatMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileSize, scale, offsetX, offsetZ, root.temperatureSettings.waves);
		heatMap = new float[tileSize, tileSize];
		
		
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				// mix both heat maps together by multiplying their values
				heatMap [zIndex, xIndex] = uniformHeatMap [zIndex, xIndex] * randomHeatMap [zIndex, xIndex];
				
				// makes higher regions colder, by adding the height value to the heat map
				heatMap [zIndex, xIndex] += root.temperatureSettings.curve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
			}
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateMoistureMap
	// -----------------------------------------------------------------------------------
	public void GenerateMoistureMap()
	{
	
		float scale = root.scaleMultiplier * root.moistureSettings.scaleModifier;
		
		moistureMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileSize, scale, offsetX, offsetZ, root.moistureSettings.waves);
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				// makes higher regions dryer, by reducing the height value from the heat map
				moistureMap [zIndex, xIndex] -= root.moistureSettings.curve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
			}
		}
	
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateDifficultyMap
	// -----------------------------------------------------------------------------------
	public void GenerateDifficultyMap()
	{
	
		
		float scale = root.scaleMultiplier * root.difficultySettings.scaleModifier;
		
		difficultyMap = this.noiseMapGeneration.GeneratePerlinNoiseMap (tileSize, scale, offsetX, offsetZ, root.difficultySettings.waves);
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				// makes higher regions dryer, by reducing the height value from the heat map
				difficultyMap [zIndex, xIndex] -= root.difficultySettings.curve.Evaluate(heightMap [zIndex, xIndex]) * heightMap [zIndex, xIndex];
			}
		}
	
	
	}
	
	
	// -----------------------------------------------------------------------------------
	
}

}