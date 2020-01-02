// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/Fhizban
// =======================================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UCE.CosmicForge;

namespace UCE.CosmicForge {

// =======================================================================================
// 
// =======================================================================================
[CreateAssetMenu(fileName = "Untitled Doodad", menuName = "Cosmic Forge/New Doodad", order = 999)]
public class DoodadTemplate : ComplexTemplate {

	public GameObject gameObject;
	public float yAxisCorrection = 0.15f;
	public float neighborRadius;
	
	public BiomeTemplate[] biomeTypes;
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public override bool ValidateTerrainType(HeightTemplate heightType, TemperatureTemplate heatType, MoistureTemplate moistureType, DifficultyTemplate difficultyType, BiomeTemplate biomeType=null)
	{
	
		bool bValid = base.ValidateTerrainType(heightType, heatType, moistureType, difficultyType);
		
		// -- validate biome (if any)
		if (biomeType != null && biomeTypes.Length > 0)
			bValid = (biomeTypes.Any(x => x.name == biomeType.name)) ? bValid : false;	
		
		return bValid;
	
	}
	
	// -----------------------------------------------------------------------------------
	
}

}

// =======================================================================================
