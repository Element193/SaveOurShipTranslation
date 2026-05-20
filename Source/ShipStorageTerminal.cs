using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;

namespace SaveOurCat
{
	/// <summary>
	/// Simple storage class for ShipCargoTerminal that allows configurable item stacking per cell
	/// Based on concepts from Adaptive Storage Framework (MIT License)
	/// </summary>
	public class ShipStorageClass : Building_Storage
	{
		private int _cachedMaxItemsInCell = -1;
		private int _cachedStorageSize = -1;
		private int _cachedTotalSlots = -1;

		/// <summary>
		/// Maximum items allowed per storage cell (configurable via XML)
		/// Cached for performance - recalculated only on SpawnSetup
		/// </summary>
		public override int MaxItemsInCell
		{
			get
			{
				if (_cachedMaxItemsInCell == -1)
				{
					_cachedMaxItemsInCell = def.building?.maxItemsInCell ?? base.MaxItemsInCell;
				}
				return _cachedMaxItemsInCell;
			}
		}

		/// <summary>
		/// Get storage size (width × height) - cached for performance
		/// </summary>
		public int StorageSize
		{
			get
			{
				if (_cachedStorageSize == -1)
				{
					// Calculate storage cells: width * height of the building
					IntVec2 size = def.Size;
					_cachedStorageSize = size.x * size.z;
				}
				return _cachedStorageSize;
			}
		}

		/// <summary>
		/// Get total storage slots available - cached for performance
		/// </summary>
		public int TotalSlots
		{
			get
			{
				if (_cachedTotalSlots == -1)
				{
					// Total slots = storage cells × max items per cell
					_cachedTotalSlots = StorageSize * MaxItemsInCell;
				}
				return _cachedTotalSlots;
			}
		}

		/// <summary>
		/// Check if there's free space quickly
		/// </summary>
		public bool HasFreeSlots
		{
			get
			{
				// Check if any slot has free capacity
				SlotGroup slotGroup = GetSlotGroup();
				if (slotGroup == null)
					return false;
				
				int heldCount = slotGroup.HeldThings.Count();
				int totalCapacity = TotalSlots;
				return heldCount < totalCapacity;
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			// Reset all caches on spawn to ensure fresh calculations
			_cachedMaxItemsInCell = -1;
			_cachedStorageSize = -1;
			_cachedTotalSlots = -1;
		}
	}

	/// <summary>
	/// Harmony patches to hide items stored in ShipStorage from rendering
	/// Inspired by ASF's approach but using publicly accessible RimWorld methods
	/// </summary>
	[HarmonyPatch(typeof(Thing), nameof(Thing.Print))]
	public static class HideShipStorageItems
	{
		/// <summary>
		/// Prevent items stored in ShipStorage from being rendered
		/// </summary>
		[HarmonyPrefix]
		public static bool Prefix(Thing __instance)
		{
			// Skip rendering if this is an item stored in ShipStorage
			if (__instance.def.category != ThingCategory.Item)
				return true;

			// Check if in a ShipStorage slot group
			Map map = __instance.Map;
			if (map != null && __instance.Position.InBounds(map))
			{
				ISlotGroupParent slotGroupParent = map.haulDestinationManager.SlotGroupParentAt(__instance.Position);
				if (slotGroupParent is ShipStorageClass)
				{
					return false; // Skip rendering - item is stored in ShipStorage
				}
			}

			// Also check if held inside storage container
			if (__instance.holdingOwner?.Owner is ShipStorageClass)
			{
				return false; // Skip rendering
			}

			return true; // Allow normal rendering
		}
	}

	/// <summary>
	/// Harmony patch to hide item labels (count) for items stored in ShipStorage
	/// </summary>
	[HarmonyPatch(typeof(ThingWithComps), nameof(ThingWithComps.DrawGUIOverlay))]
	public static class HideShipStorageItemLabels
	{
		/// <summary>
		/// Prevent labels (item count) from being drawn for items in ShipStorage
		/// </summary>
		[HarmonyPrefix]
		public static bool Prefix(ThingWithComps __instance)
		{
			// Only process items
			if (__instance.def.category != ThingCategory.Item)
				return true;

			// Check if in a ShipStorage slot group
			Map map = __instance.Map;
			if (map != null && __instance.Position.InBounds(map))
			{
				ISlotGroupParent slotGroupParent = map.haulDestinationManager.SlotGroupParentAt(__instance.Position);
				if (slotGroupParent is ShipStorageClass)
				{
					return false; // Skip label drawing
				}
			}

			// Also check if held inside storage container
			if (__instance.holdingOwner?.Owner is ShipStorageClass)
			{
				return false; // Skip label drawing
			}

			return true; // Allow normal label drawing
		}
	}

	/// <summary>
	/// Harmony patch to disable material selection for ShipStorage building
	/// Removes stuffCategories and madeFromStuff to prevent material selection dialog
	/// </summary>
	[HarmonyPatch(typeof(ThingDef), nameof(ThingDef.PostLoad))]
	public static class DisableShipStorageMaterial
	{
		/// <summary>
		/// Override ThingDef settings for ShipStorage to disable material selection
		/// </summary>
		[HarmonyPostfix]
		public static void Postfix(ThingDef __instance)
		{
			// Apply to ShipStorage only
			if (__instance.defName != "ShipStorage")
				return;

			// Force disable material selection system
			__instance.stuffCategories = null;             // Clear all stuff categories (Metallic, Woody, Stony, etc)
			__instance.costStuffCount = 0;                // No additional material costs
			
			// Use reflection to set readonly MadeFromStuff property
			FieldInfo fieldInfo = typeof(BuildableDef).GetField("madeFromStuff", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(__instance, false);
			}
		}
	}

	/// <summary>
	/// Handler for ShipCargoTerminal enable/disable setting
	/// Removes defName from game if setting is disabled
	/// </summary>
	public static class ShipCargoTerminalControl
	{
		/// <summary>
		/// Called from SoSAdvSettings to apply ShipCargoTerminal setting
		/// </summary>
		public static void ApplyShipCargoTerminalSetting(bool isEnabled)
		{
			ThingDef shipStorageDef = DefDatabase<ThingDef>.GetNamedSilentFail("ShipStorage");
			if (shipStorageDef == null)
				return;

			if (!isEnabled)
			{
				// Disable the building when setting is off
				RemoveShipStorageFromGame(shipStorageDef);
			}
		}

		/// <summary>
		/// Remove ShipStorage from designators and visibility
		/// </summary>
		private static void RemoveShipStorageFromGame(ThingDef def)
		{
			// Clear research prerequisites
			def.researchPrerequisites?.Clear();
			
			// Remove from designator dropdown
			def.designatorDropdown = null;
			
			// Remove from designation category (makes it invisible in architect menu)
			def.designationCategory = null;
		}
	}

	/// <summary>
	/// Mod initialization to apply Harmony patches
	/// </summary>
	public class ShipStorageMod : Mod
	{
		public ShipStorageMod(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("SaveOurCat.ShipStorage");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
