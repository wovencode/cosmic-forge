using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class LevelData {

	private int tileDepthInVertices;
	public float scale;
	public TileData[,] tilesData;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public LevelData(int tileDepthInVertices, int levelDepthInTiles, float scale) {
		// build the tilesData matrix based on the level depth and width
		tilesData = new TileData[tileDepthInVertices * levelDepthInTiles, tileDepthInVertices * levelDepthInTiles];

		this.tileDepthInVertices = tileDepthInVertices;
		
		this.scale = scale;
		
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public void AddTileData(TileData tileData, int tileZIndex, int tileXIndex) {
		// save the TileData in the corresponding coordinate
		tilesData [tileZIndex, tileXIndex] = tileData;
	}
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public TileCoordinate ConvertToTileCoordinate(int zIndex, int xIndex) {
		// the tile index is calculated by dividing the index by the number of tiles in that axis
		int tileZIndex = (int)Mathf.Floor ((float)zIndex / (float)this.tileDepthInVertices);
		int tileXIndex = (int)Mathf.Floor ((float)xIndex / (float)this.tileDepthInVertices);
		// the coordinate index is calculated by getting the remainder of the division above
		// we also need to translate the origin to the bottom left corner
		int coordinateZIndex = this.tileDepthInVertices - (zIndex % this.tileDepthInVertices) - 1;
		int coordinateXIndex = this.tileDepthInVertices - (xIndex % this.tileDepthInVertices) - 1;

		TileCoordinate tileCoordinate = new TileCoordinate (tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
		return tileCoordinate;
	}
	
	// -----------------------------------------------------------------------------------
	
}


}