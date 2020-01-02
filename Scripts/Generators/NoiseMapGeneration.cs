using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
public class NoiseMapGeneration : MonoBehaviour {
	
	public LevelGeneration root;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public float[,] GeneratePerlinNoiseMap(int tileSize, float scale, float offsetX, float offsetZ, WaveTemplate[] waves) {
		
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[tileSize, tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				// calculate sample indices based on the coordinates, the scale and the offset
				float sampleX = (xIndex + offsetX) / scale;
				float sampleZ = (zIndex + offsetZ) / scale;

				float noise = 0f;
				float normalization = 0f;
				
				foreach (WaveTemplate wave in waves) {
					// generate noise value using PerlinNoise for a given Wave
					noise += wave.amplitude * Mathf.PerlinNoise (sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
					normalization += wave.amplitude;
				}
				
				// normalize the noise value so that it is within 0 and 1
				noise /= normalization;

				noiseMap [zIndex, xIndex] = noise;
			}
		}

		return noiseMap;
	}
	
	// -----------------------------------------------------------------------------------
	// GenerateUniformNoiseMapZ
	// create a uniform noise map along the z axis
	// -----------------------------------------------------------------------------------
	public float[,] GenerateUniformNoiseMapZ(int tileSize, float scale, float offsetZ, bool invert=false) {
	
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[tileSize, tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			// calculate the sampleZ by summing the index and the offset
			float sampleZ = (zIndex + offsetZ) / scale;
			// calculate the noise proportional to the distance of the sample to the center of the level
			float noise = Mathf.Abs (sampleZ - root.maxDistance) / root.maxDistance;
			
			if (invert) noise = Mathf.Max(0.0f, 1.0f - noise);
			
			// apply the noise for all points with this Z coordinate
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				noiseMap [tileSize - zIndex - 1, xIndex] = noise;
			}
		}

		return noiseMap;
	}
		
		
	// -----------------------------------------------------------------------------------
	// GenerateUniformNoiseMap
	// create a uniform noise map along the z and x axis
	// -----------------------------------------------------------------------------------
	public float[,] GenerateUniformNoiseMap(int tileSize, float scale, float offsetX, float offsetZ, bool invert=false) {
	
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[tileSize, tileSize];
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
			
			// calculate the sampleZ/sampleX by summing the index and the offset
			float sampleZ = (zIndex + offsetZ) / scale;
			float sampleX = (xIndex + offsetX) / scale;
			
			// calculate the noise proportional to the distance of the sample to the center of the level
			float noiseZ = Mathf.Abs (sampleZ - root.maxDistance) / root.maxDistance;
			float noiseX = Mathf.Abs (sampleX - root.maxDistance) / root.maxDistance;
			
			float noise = (noiseZ + noiseX) *0.5f;
			
			if (invert) noise = Mathf.Max(0.0f, 1.0f - noise);
			
			noiseMap [zIndex, xIndex] = noise;
			
			}
		}

		return noiseMap;
	}
		
		
		
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public float[,] GenerateGradientCircleMap(int tileSize, float scale, float offsetX, float offsetZ, bool invert=false) {
		
		float[,] noiseMap = new float[tileSize, tileSize];
		
		float size = (tileSize * root.levelSizeInTiles * 0.5f) - ((tileSize + root.levelSizeInTiles) * 0.5f);
		
		for (int zIndex = 0; zIndex < tileSize; zIndex++) {
			for (int xIndex = 0; xIndex < tileSize; xIndex++) {
				
				float sampleX = (xIndex + offsetX + size) / scale;
				float sampleZ = (zIndex + offsetZ + size) / scale;
				
				float val = Mathf.Sqrt((sampleZ * sampleZ) + (sampleX*sampleX));
				
				float noise = val / root.maxDistance * 0.5f;
				
				if (invert) noise = Mathf.Max(0.0f, 1.0f - noise);
				
				noiseMap [zIndex, xIndex] = noise;
			}			
		}
		
		return noiseMap;
		
	}
	
	// -----------------------------------------------------------------------------------

}

}