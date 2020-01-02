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
public abstract class ComplexTemplate : SimpleTemplate {

	[Range(0,1)]public float chance = 1;
	public HeightTemplate[] heightTypes;
	public TemperatureTemplate[] heatTypes;
	public MoistureTemplate[] moistureTypes;
	public DifficultyTemplate[] difficultyTypes;
	
	
	// -----------------------------------------------------------------------------------
	// 
	// -----------------------------------------------------------------------------------
	public virtual bool ValidateTerrainType(HeightTemplate heightType, TemperatureTemplate heatType, MoistureTemplate moistureType, DifficultyTemplate difficultyType, BiomeTemplate biomeType=null)
	{
	
		bool bValid = (Random.value <= chance);
		
		// -- validate height (if any)
		if (heightType != null && heightTypes != null && heightTypes.Length > 0)
			bValid = (heightTypes.Any(x => x.name == heightType.name)) ? bValid : false;
		
		
		// -- validate heat (if any)
		if (heatType != null && heatTypes != null && heatTypes.Length > 0)
			bValid = (heatTypes.Any(x => x.name == heatType.name)) ? bValid : false;
		
		
		// -- validate moisture (if any)
		if (moistureType != null && moistureTypes != null && moistureTypes.Length > 0)
			bValid = (moistureTypes.Any(x => x.name == moistureType.name)) ? bValid : false;
		
		// -- validate difficulty (if any)
		if (difficultyType != null && difficultyTypes != null && difficultyTypes.Length > 0)
			bValid = (difficultyTypes.Any(x => x.name == difficultyType.name)) ? bValid : false;
		
		
		return bValid;
	
	}
	
	// -----------------------------------------------------------------------------------
	
}

}

// =======================================================================================
