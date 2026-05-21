using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace SaveOurCat // Зоны для проверок условий
{
	public class PlaceWorker_FrontZoneClear : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef == null)
				return AcceptanceReport.WasAccepted;

			List<IntVec3> frontCells = GetFrontCells(loc, rot, thingDef);
			
			if (frontCells.Count == 0)
				return AcceptanceReport.WasAccepted;

			foreach (IntVec3 frontCell in frontCells)
			{
				if (!frontCell.InBounds(map))
					continue;

				if (!frontCell.Walkable(map))
				{
				return new AcceptanceReport("SaveOurCat_FrontZoneNotClear".Translate());
			}
		}

		return AcceptanceReport.WasAccepted;
	}

	public override void DrawGhost(ThingDef thingDef, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			List<IntVec3> frontCells = GetFrontCells(loc, rot, thingDef);
			GenDraw.DrawFieldEdges(frontCells, Color.white);
		}

		private List<IntVec3> GetFrontCells(IntVec3 loc, Rot4 rot, ThingDef def)
		{
			List<IntVec3> cells = new List<IntVec3>();
			
			CellRect buildingRect = GenAdj.OccupiedRect(loc, rot, def.size);

			if (rot == Rot4.North)
			{
				int z = buildingRect.maxZ + 1;
				for (int x = buildingRect.minX; x <= buildingRect.maxX; x++)
				{
					cells.Add(new IntVec3(x, 0, z));
				}
			}
			else if (rot == Rot4.South)
			{
				int z = buildingRect.minZ - 1;
				for (int x = buildingRect.minX; x <= buildingRect.maxX; x++)
				{
					cells.Add(new IntVec3(x, 0, z));
				}
			}
			else if (rot == Rot4.East)
			{
				int x = buildingRect.maxX + 1;
				for (int z = buildingRect.minZ; z <= buildingRect.maxZ; z++)
				{
					cells.Add(new IntVec3(x, 0, z));
				}
			}
			else if (rot == Rot4.West)
			{
				int x = buildingRect.minX - 1;
				for (int z = buildingRect.minZ; z <= buildingRect.maxZ; z++)
				{
					cells.Add(new IntVec3(x, 0, z));
				}
			}

			return cells;
		}
	}
}
