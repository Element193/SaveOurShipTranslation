using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SaveOurCat
{
    public class SaveOurCatSetts : ModSettings
    {
        public bool enableOdysseyTexPatch = false;
        public bool enableAntiproton = false;
        public bool enableStarSectorShips = false;
        public bool enableX4FoundationsShips = false;
        public bool enableOtherShips = false;
        public bool enableHoverModePatch = false;
        public bool enableEnergopatch = false;
        public bool enableThrustersPatch = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enableOdysseyTexPatch, "enableOdysseyTexPatch", false);
            Scribe_Values.Look(ref enableAntiproton, "enableAntiproton", false);
            Scribe_Values.Look(ref enableStarSectorShips, "enableStarSectorShips", false);
            Scribe_Values.Look(ref enableX4FoundationsShips, "enableX4FoundationsShips", false);
            Scribe_Values.Look(ref enableOtherShips, "enableOtherShips", false);
            Scribe_Values.Look(ref enableHoverModePatch, "enableHoverModePatch", false);
            Scribe_Values.Look(ref enableEnergopatch, "enableEnergopatch", false);
            Scribe_Values.Look(ref enableThrustersPatch, "enableThrustersPatch", false);
            base.ExposeData();
        }
    }

    public class SaveOurCat : Mod
    {
        public static SaveOurCatSetts settings;

        public SaveOurCat(ModContentPack content) : base(content)
        {
            settings = GetSettings<SaveOurCatSetts>();
            LongEventHandler.ExecuteWhenFinished(ApplyPatchIfNeeded);
        }

        private static void ApplyPatchIfNeeded()
        {
            ARLOdysseyPatchApplier.ApplyPatchIfNeeded();
            ARLOdysseyThrustersPatchApplier.ApplyPatchIfNeeded();
            if (settings.enableEnergopatch)
            {
                ARLEnergyPatchApplier.ApplyPatch();
            }
            if (settings.enableHoverModePatch)
            {
                ARLHoverModePatchApplier.ApplyPatch();
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Widgets.DrawLineVertical(inRect.center.x, inRect.yMin, inRect.height);

            Listing_Standard listing = new Listing_Standard();
            listing.ColumnWidth = inRect.width / 2;
            listing.Begin(inRect);

            // Addons
            Text.Font = GameFont.Medium;
            listing.Label("ARL.SOSSetts.Addons.Title".Translate());
            Text.Font = GameFont.Small;
            listing.GapLine();
            listing.CheckboxLabeled("ARL.SOSSetts.Antiproton.Label".Translate(), ref settings.enableAntiproton, "ARL.SOSSetts.Antiproton.Desc".Translate());
            listing.Gap();

            // Ships
            Text.Font = GameFont.Medium;
            listing.Label("ARL.SOSSetts.Ships.Title".Translate());
            Text.Font = GameFont.Small;
            listing.GapLine();
            listing.CheckboxLabeled("ARL.SOSSetts.Ships.Label".Translate(), ref settings.enableStarSectorShips, "ARL.SOSSetts.Ships.Desc".Translate());
            listing.CheckboxLabeled("ARL.SOSSetts.X4Foundations.Label".Translate(), ref settings.enableX4FoundationsShips, "ARL.SOSSetts.X4Foundations.Desc".Translate());
            listing.CheckboxLabeled("ARL.SOSSetts.OtherShips.Label".Translate(), ref settings.enableOtherShips, "ARL.SOSSetts.OtherShips.Desc".Translate());
            listing.Gap();

            // Patches
            Text.Font = GameFont.Medium;
            listing.Label("ARL.SOSSets.Patches.Title".Translate());
            Text.Font = GameFont.Small;
            listing.GapLine();
            listing.CheckboxLabeled("ARL.SOSSets.Energopatch.Label".Translate(), ref settings.enableEnergopatch, "ARL.SOSSets.Energopatch.Desc".Translate());
            listing.CheckboxLabeled("ARL.SOSSets.Odysseytexpatch.Label".Translate(), ref settings.enableOdysseyTexPatch, "ARL.SOSSets.Odysseytexpatch.Desc".Translate());
            listing.CheckboxLabeled("ARL.SOSSets.ThrustersPatch.Label".Translate(), ref settings.enableThrustersPatch, "ARL.SOSSets.ThrustersPatch.Desc".Translate());
            listing.CheckboxLabeled("ARL.SOSSets.HoverModepatch.Label".Translate(), ref settings.enableHoverModePatch, "ARL.SOSSets.HoverModepatch.Desc".Translate());

            listing.End();
        }

        public override string SettingsCategory() => "Save Our Cat";
    }

    public static class ARLOdysseyPatchApplier
    {
        public static void ApplyPatchIfNeeded()
        {
if (!(SaveOurCat.settings?.enableOdysseyTexPatch ?? false))
            {
                return;
            }

            if (!IsSaveOurShipLoaded())
            {
                return;
            }

            ApplyPatch();
        }

        public static bool IsSaveOurShipLoaded()
        {
            foreach (var mod in LoadedModManager.RunningModsListForReading)
            {
                if (mod.Name == "Save Our Ship" || mod.PackageIdPlayerFacing == "Save Our Ship" ||
                    mod.Name.Contains("Save Our Ship") || mod.PackageIdPlayerFacing.Contains("Save Our Ship") ||
                    mod.PackageId == "kentington.saveourship2")
                {
                    return true;
                }
            }

            return false;
        }

        private static void ApplyPatch()
        {
            // Ship_Beam
            PatchGraphic("Ship_Beam", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);

            // Ship_Beam_Unpowered
            PatchGraphic("Ship_Beam_Unpowered", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);

            // ShipAirlockBeam
            PatchGraphic("ShipAirlockBeam", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);

            // Passive vents
            PatchTex("ShipInside_PassiveVent", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_PassiveVentArchotech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_PassiveVentMechanoid", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");

            // Solar generators
            PatchTex("ShipInside_SolarGenerator", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_SolarGeneratorArchotech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_SolarGeneratorMech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");

        }

        private static void PatchGraphic(string defName, string texPath, System.Type graphicClass, LinkDrawerType linkType)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def?.graphicData != null)
            {
                def.graphicData.texPath = texPath;
                def.graphicData.graphicClass = graphicClass;
                def.graphicData.linkType = linkType;
            }
        }

        private static void PatchTex(string defName, string texPath)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def?.graphicData != null)
            {
                def.graphicData.texPath = texPath;
            }
        }
    }

    public static class ARLOdysseyThrustersPatchApplier
    {
        public static void ApplyPatchIfNeeded()
        {
            if (!(SaveOurCat.settings?.enableThrustersPatch ?? false))
            {
                return;
            }

            if (!ARLOdysseyPatchApplier.IsSaveOurShipLoaded())
            {
                return;
            }

            ApplyPatch();
        }

        private static void ApplyPatch()
        {
            PatchThruster("Ship_Engine_Small", "Things/Building/SmallThruster/SmallThruster", new Vector2(2f, 3f));
            PatchThruster("Ship_Engine", "Things/Building/LateralThruster/LateralThruster", new Vector2(3f, 3f));
            PatchThrusterAltitude("Ship_Thruster");
        }

        private static void PatchThrusterAltitude(string defName)
        {
            var thruster = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (thruster != null)
            {
                thruster.altitudeLayer = AltitudeLayer.Building;
            }
        }

        private static void PatchThruster(string defName, string texPath, Vector2 drawSize)
        {
            PatchTex(defName, texPath);
            var engine = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (engine?.graphicData != null)
            {
                engine.graphicData.drawSize = drawSize;
            }
        }

        private static void PatchTex(string defName, string texPath)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def?.graphicData != null)
            {
                def.graphicData.texPath = texPath;
            }
        }
    }

    public static class ARLEnergyPatchApplier
    {
        public static void ApplyPatch()
        {
            ApplyEnergyPatch("Ship_LifeSupport", 400f);
            ApplyEnergyPatch("Ship_LifeSupport_Small", 800f);
        }

        private static void ApplyEnergyPatch(string defName, float newPower)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def?.comps == null)
            {
                return;
            }

            foreach (var comp in def.comps)
            {
                if (comp == null || comp.GetType().Name != "CompProperties_Power")
                {
                    continue;
                }

                var valueField = comp.GetType().GetField("basePowerConsumption", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                 ?? comp.GetType().GetField("powerConsumption", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (valueField == null || valueField.FieldType != typeof(float))
                {
                    continue;
                }

                valueField.SetValue(comp, newPower);
                return;
            }
        }
    }

    public static class ARLHoverModePatchApplier
    {
        public static void ApplyPatch()
        {
            PatchDraftLabel("SoS2_Shuttle_Personal");
            PatchDraftLabel("SoS2_Shuttle");
            PatchDraftLabel("SoS2_Shuttle_Heavy");
            PatchDraftLabel("SoS2_Shuttle_Superheavy");
        }

        private static void PatchDraftLabel(string defName)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def == null || def.GetType().Name != "VehicleDef")
            {
                return;
            }

            var field = def.GetType().GetField("draftLabel", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(def, "Режим парения");
            }
        }
    }
}
