using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;

namespace SaveOurCat // Грузовой терминал корабля и склады
{
	public class ShipStorageClass : Building_Storage
	{
		private int _cachedMaxItemsInCell = -1;
		private int _cachedStorageSize = -1;
		private int _cachedTotalSlots = -1;

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

		public int StorageSize
		{
			get
			{
				if (_cachedStorageSize == -1)
				{
					IntVec2 size = def.Size;
					_cachedStorageSize = size.x * size.z;
				}
				return _cachedStorageSize;
			}
		}

		public int TotalSlots
		{
			get
			{
				if (_cachedTotalSlots == -1)
				{
					_cachedTotalSlots = StorageSize * MaxItemsInCell;
				}
				return _cachedTotalSlots;
			}
		}

		public bool HasFreeSlots
		{
			get
			{
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
			_cachedMaxItemsInCell = -1;
			_cachedStorageSize = -1;
			_cachedTotalSlots = -1;
		}
	}

	[HarmonyPatch(typeof(Thing), nameof(Thing.Print))]
	public static class HideShipStorageItems
	{
		[HarmonyPrefix]
		public static bool Prefix(Thing __instance)
		{
			if (__instance.def.category != ThingCategory.Item)
				return true;

			Map map = __instance.Map;
			if (map != null && __instance.Position.InBounds(map))
			{
				ISlotGroupParent slotGroupParent = map.haulDestinationManager.SlotGroupParentAt(__instance.Position);
				if (slotGroupParent is ShipStorageClass)
				{
					return false;
				}
			}

			if (__instance.holdingOwner?.Owner is ShipStorageClass)
			{
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(ThingWithComps), nameof(ThingWithComps.DrawGUIOverlay))]
	public static class HideShipStorageItemLabels
	{
		[HarmonyPrefix]
		public static bool Prefix(ThingWithComps __instance)
		{
			if (__instance.def.category != ThingCategory.Item)
				return true;

			Map map = __instance.Map;
			if (map != null && __instance.Position.InBounds(map))
			{
				ISlotGroupParent slotGroupParent = map.haulDestinationManager.SlotGroupParentAt(__instance.Position);
				if (slotGroupParent is ShipStorageClass)
				{
					return false;
				}
			}

			if (__instance.holdingOwner?.Owner is ShipStorageClass)
			{
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(ThingDef), nameof(ThingDef.PostLoad))]
	public static class DisableShipStorageMaterial
	{
		[HarmonyPostfix]
		public static void Postfix(ThingDef __instance)
		{
			if (__instance.defName != "ShipStorage")
				return;

			__instance.stuffCategories = null;
			__instance.costStuffCount = 0;
			
			FieldInfo fieldInfo = typeof(BuildableDef).GetField("madeFromStuff", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(__instance, false);
			}
		}
	}

	public static class ShipCargoTerminalControl
	{
		public static void ApplyShipCargoTerminalSetting(bool isEnabled)
		{
			ThingDef shipStorageDef = DefDatabase<ThingDef>.GetNamedSilentFail("ShipStorage");
			if (shipStorageDef == null)
				return;

			if (!isEnabled)
			{
				RemoveShipStorageFromGame(shipStorageDef);
			}
		}

		private static void RemoveShipStorageFromGame(ThingDef def)
		{
			def.researchPrerequisites?.Clear();
			
			def.designatorDropdown = null;
			
			def.designationCategory = null;
		}
	}

	public class ShipStorageMod : Mod
	{
		public ShipStorageMod(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("SaveOurCat.ShipStorage");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
