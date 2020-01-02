using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

[System.Serializable]
public abstract class SettingsType {
	[Range(0.1f,10.0f)]public float scaleModifier = 1.0f;
	[Range(0.1f,10.0f)]public float heightModifier = 1.0f;
}

[System.Serializable]
public class BiomeSettings : SettingsType {
	public BiomeTemplate[] 			templates;
}

[System.Serializable]
public class HeightSettings : SettingsType {
	public HeightTemplate[] 		templates;
	public AnimationCurve 			curve;
	public WaveTemplate[] 			waves;
	public bool						fallOff;
}

[System.Serializable]
public class TemperatureSettings : SettingsType {
	public TemperatureTemplate[] 	templates;
	public AnimationCurve 			curve;
	public WaveTemplate[] 			waves;
}

[System.Serializable]
public class MoistureSettings : SettingsType {
	public MoistureTemplate[] 		templates;
	public AnimationCurve 			curve;
	public WaveTemplate[] 			waves;
}

[System.Serializable]
public class DifficultySettings : SettingsType {
	public DifficultyTemplate[] 	templates;
	public AnimationCurve 			curve;
	public WaveTemplate[] 			waves;
}

[System.Serializable]
public class DoodadRow {
	public DoodadTemplate[] doodads;
}

public enum VisualizationMode { Height, Heat, Moisture, Biome, Difficulty, Material }

}