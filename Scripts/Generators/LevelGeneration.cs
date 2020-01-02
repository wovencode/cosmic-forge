using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class LevelGeneration : MonoBehaviour {

	[Header("-=-=- General Settings -=-=-")]
	[Range(1,100)] public int levelSizeInTiles;
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private DoodadGeneration doodadGeneration;
	[SerializeField] private RiverGeneration riverGeneration;
	public VisualizationMode visualizationMode;
	[Range(1,100)]public float scaleMultiplier;
	[Range(1,100)]public float heightMultiplier;
	[SerializeField] private bool deleteOldTiles;
	[SerializeField] private bool saveNewTiles;
	
	[Header("-=-=- Individual Settings -=-=-")]
	public BiomeSettings			biomeSettings;
	public HeightSettings			heightSettings;
	public TemperatureSettings		temperatureSettings;
	public MoistureSettings			moistureSettings;
	public DifficultySettings		difficultySettings;
	
	
	[HideInInspector] public float maxDistance, tileSize;
	
	private LevelData levelData;
	
	// -----------------------------------------------------------------------------------
	// Awake
	// -----------------------------------------------------------------------------------
	void Awake() {
		deleteOldTiles = true;
		GenerateMap();
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateMap
	// -----------------------------------------------------------------------------------
	public void GenerateMap() {
	
		// delete the old tiles first
		DeleteMap();
		
		// get the tile dimensions from the tile Prefab
		int tileDepth = (int)tilePrefab.GetComponent<MeshRenderer> ().bounds.size.x;
		
		// calculate the number of vertices of the tile in each axis using its mesh
		Vector3[] tileMeshVertices = tilePrefab.GetComponent<MeshFilter> ().sharedMesh.vertices;
		int tileDepthInVertices = (int)Mathf.Sqrt (tileMeshVertices.Length);
		
		// update max distance according to total map size
		maxDistance	= levelSizeInTiles * tileDepthInVertices*0.5f;
		tileSize = tilePrefab.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		
		
		float distanceBetweenVertices = (float)tileDepth / (float)tileDepthInVertices;

		// build an empty LevelData object, to be filled with the tiles to be generated
		levelData = new LevelData (tileDepthInVertices, levelSizeInTiles, this.scaleMultiplier);

		// for each Tile, instantiate a Tile in the correct position
		for (int xTileIndex = 0; xTileIndex < levelSizeInTiles; xTileIndex++) {
			for (int zTileIndex = 0; zTileIndex < levelSizeInTiles; zTileIndex++) {
			
				// calculate the tile position based on the X and Z indices
				Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileDepth, 
					this.gameObject.transform.position.y, 
					this.gameObject.transform.position.z + zTileIndex * tileDepth);
				
				// instantiate a new Tile
				GameObject tile = Instantiate (tilePrefab, tilePosition, Quaternion.identity) as GameObject;
				tile.transform.parent = this.gameObject.transform;
				
				// generate the Tile texture and save it in the levelData
				TileData tileData = tile.GetComponent<TileGeneration> ().GenerateTile (this);
				levelData.AddTileData (tileData, zTileIndex, xTileIndex);
				
			}
		}

		// generate doodads for the level
		doodadGeneration.Generate(levelSizeInTiles * tileDepthInVertices, distanceBetweenVertices, levelData);

		// generate rivers for the level
		//riverGeneration.Generate(this.levelDepthInTiles * tileDepthInVertices, this.levelWidthInTiles * tileWidthInVertices, levelData);
	}
	
	// -----------------------------------------------------------------------------------
	// DeleteMap
	// -----------------------------------------------------------------------------------
	public void DeleteMap(bool forceDelete = false) {
	
		if (!forceDelete && !deleteOldTiles) return;
		
		for (int i = this.gameObject.transform.childCount - 1; i >= 0; i--) 
			DestroyImmediate(this.gameObject.transform.GetChild(i).gameObject);
		
	}
	
	// -----------------------------------------------------------------------------------
	// OnValidate
	// -----------------------------------------------------------------------------------
	protected virtual void OnValidate() {
		
		
	}
	
	// -----------------------------------------------------------------------------------
	
}



}