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
[CreateAssetMenu(fileName = "Untitled Wave", menuName = "Cosmic Forge/New Wave", order = 999)]
public class WaveTemplate : ScriptableObject {
	[Range(0,99999)]public float seed;
	[Range(0,1)]public float frequency;
	[Range(1,100)]public float amplitude;
}

}

// =======================================================================================
