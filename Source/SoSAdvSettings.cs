using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace SaveOurCat
{
    public class Soc_ModSettings : ModSettings
    {
        public bool Soc_Flag_OdysseyHullGraphics = false;
        public bool Soc_Flag_AccelerationParticles = true;
        public bool Soc_Flag_RussianTranslation = false;
        public bool Soc_Flag_LifeSupportEnergy = false;
        public bool Soc_Flag_OdysseyThrustersGraphics = false;
        public bool Soc_Flag_RCSLayerFix = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Soc_Flag_OdysseyHullGraphics, "enableOdysseyHullGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_AccelerationParticles, "enableAccelerationParticles", true);
            Scribe_Values.Look(ref Soc_Flag_RussianTranslation, "enableRussianTranslation", false);
            Scribe_Values.Look(ref Soc_Flag_LifeSupportEnergy, "enableLifeSupportEnergy", false);
            Scribe_Values.Look(ref Soc_Flag_OdysseyThrustersGraphics, "enableOdysseyThrustersGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_RCSLayerFix, "enableRCSLayerFix", true);
            base.ExposeData();
        }
    }

    public class SaveOurCat : Mod
    {
        public static Soc_ModSettings settings;

        public SaveOurCat(ModContentPack content) : base(content)
        {
            settings = GetSettings<Soc_ModSettings>();
            LongEventHandler.ExecuteWhenFinished(ApplyPatchIfNeeded);
        }

        private static void ApplyPatchIfNeeded()
        {
            Soc_PatchLogics.ApplyIfNeed_OdysseyHullGraphics();
            Soc_PatchLogics.ApplyIfNeedOdysseyThrustersGraphics();
            Soc_PatchLogics.ApplyIfNeed_AcceleratingParticles();
            if (settings.Soc_Flag_RCSLayerFix)
            {
                Soc_PatchLogics.ApplyIfNeed_RCSLayerFix();
            }
            if (settings.Soc_Flag_LifeSupportEnergy)
            {
                Soc_PatchLogics.ApplyIfNeed_LifeSupportEnergy();
            }
            if (settings.Soc_Flag_RussianTranslation)
            {
                Soc_PatchLogics.ApplyIfNeed_RussianTranslation();
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Widgets.DrawLineVertical(inRect.center.x, inRect.yMin, inRect.height);

            Listing_Standard listing = new Listing_Standard();
            listing.ColumnWidth = inRect.width / 2;
            listing.Begin(inRect);

            // Addons
            Text.Anchor = TextAnchor.MiddleCenter;
            listing.Label("SocSetts.Addons.title".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            listing.GapLine();
            listing.CheckboxLabeled("SocSetts.AcceleratingParticles.label".Translate(), ref settings.Soc_Flag_AccelerationParticles, "SocSetts.AcceleratingParticles.desc".Translate());
            listing.Gap();

            // Ships
            Text.Anchor = TextAnchor.MiddleCenter;
            listing.Label("SocSetts.Ships.title".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            listing.GapLine();
            
            // StarSector Ships
            Rect starSectorRect = listing.GetRect(24f);
            Rect starSectorLabelRect = new Rect(starSectorRect.x, starSectorRect.y, starSectorRect.width - 80, starSectorRect.height);
            Widgets.Label(starSectorLabelRect, "SocSetts.StarSectorShips.label".Translate());
            TooltipHandler.TipRegion(starSectorLabelRect, "SocSetts.StarSectorShips.desc".Translate());
            GUI.color = Color.green;
            Widgets.Label(new Rect(starSectorRect.xMax - 100, starSectorRect.y, 100, 24), "SocSetts.Active.stat".Translate());
            GUI.color = Color.white;
            listing.Gap(4f);

            // X4 Foundations Ships
            Rect x4Rect = listing.GetRect(24f);
            Rect x4LabelRect = new Rect(x4Rect.x, x4Rect.y, x4Rect.width - 80, x4Rect.height);
            Widgets.Label(x4LabelRect, "SocSetts.X4FoundationsShips.label".Translate());
            TooltipHandler.TipRegion(x4LabelRect, "SocSetts.X4FoundationsShips.desc".Translate());
            GUI.color = Color.green;
            Widgets.Label(new Rect(x4Rect.xMax - 100, x4Rect.y, 100, 24), "SocSetts.Active.stat".Translate());
            GUI.color = Color.white;
            listing.Gap(4f);

            // Other Ships
            Rect otherRect = listing.GetRect(24f);
            Rect otherLabelRect = new Rect(otherRect.x, otherRect.y, otherRect.width - 80, otherRect.height);
            Widgets.Label(otherLabelRect, "SocSetts.DifferentShips.label".Translate());
            TooltipHandler.TipRegion(otherLabelRect, "SocSetts.DifferentShips.desc".Translate());
            GUI.color = Color.green;
            Widgets.Label(new Rect(otherRect.xMax - 100, otherRect.y, 100, 24), "SocSetts.Active.stat".Translate());
            GUI.color = Color.white;
            listing.Gap(4f);
            
            listing.Gap();

            // Patches
            Rect patchesTitleRect = listing.GetRect(24f);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(patchesTitleRect, "SocSetts.Patches.Title".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            listing.GapLine();
            listing.CheckboxLabeled("SocSetts.LifeSupportEnergy.label".Translate(), ref settings.Soc_Flag_LifeSupportEnergy, "SocSetts.LifeSupportEnergy.desc".Translate());
            listing.CheckboxLabeled("SocSetts.OdysseyHullGraphics.label".Translate(), ref settings.Soc_Flag_OdysseyHullGraphics, "SocSetts.OdysseyHullGraphics.desc".Translate());
            listing.CheckboxLabeled("SocSetts.OdysseyThrustersGraphics.label".Translate(), ref settings.Soc_Flag_OdysseyThrustersGraphics, "SocSetts.OdysseyThrustersGraphics.desc".Translate());
            listing.CheckboxLabeled("SocSetts.RCSLayerFix.label".Translate(), ref settings.Soc_Flag_RCSLayerFix, "SocSetts.RCSLayerFix.desc".Translate());
            listing.CheckboxLabeled("SocSetts.RussianTranslation.label".Translate(), ref settings.Soc_Flag_RussianTranslation, "SocSetts.RussianTranslation.desc".Translate());

            listing.End();
        }

        public override string SettingsCategory() => "Save Our Cat";
    }

    public static class Soc_PatchLogics
    {
        private static bool Soc_OdysseyGraphics_Applied = false;
        private static bool Soc_OdysseyThrusters_Applied = false;
        private static bool Soc_RCSLayerFix_Applied = false;
        private static bool Soc_LifeSypportEnergy_Applied = false;
        private static bool Soc_RussianTranslation_Applied = false;
        private static bool Soc_AccelerationParticles_Applied = false;
        public static void ApplyIfNeed_OdysseyHullGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_OdysseyHullGraphics ?? false) || Soc_OdysseyGraphics_Applied)
            {
                return;
            }

            if (!IsSaveOurShipLoaded())
            {
                return;
            }

            ApplyOdysseyTexPatch();
            Soc_OdysseyGraphics_Applied = true;
        }

        public static void ApplyIfNeedOdysseyThrustersGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_OdysseyThrustersGraphics ?? false) || Soc_OdysseyThrusters_Applied)
            {
                return;
            }

            if (!IsSaveOurShipLoaded())
            {
                return;
            }

            ApplyOdysseyThrustersPatch();
            Soc_OdysseyThrusters_Applied = true;
        }

        public static void ApplyIfNeed_LifeSupportEnergy()
        {
            if (Soc_LifeSypportEnergy_Applied)
            {
                return;
            }

            ApplyEnergyPatchInternal("Ship_LifeSupport", 400f);
            ApplyEnergyPatchInternal("Ship_LifeSupport_Small", 800f);
            Soc_LifeSypportEnergy_Applied = true;
        }

        public static void ApplyIfNeed_RussianTranslation()
        {
            if (Soc_RussianTranslation_Applied)
            {
                return;
            }

            PatchDraftLabel("SoS2_Shuttle_Personal");
            PatchDraftLabel("SoS2_Shuttle");
            PatchDraftLabel("SoS2_Shuttle_Heavy");
            PatchDraftLabel("SoS2_Shuttle_Superheavy");
            Soc_RussianTranslation_Applied = true;
        }

        private static bool IsSaveOurShipLoaded()
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

        private static void ApplyOdysseyTexPatch()
        {
            PatchGraphic("Ship_Beam", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);
            PatchGraphic("Ship_Beam_Unpowered", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);
            PatchGraphic("ShipAirlockBeam", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas", typeof(Graphic_Single), LinkDrawerType.Basic);
            PatchTex("ShipInside_PassiveVent", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_PassiveVentArchotech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_PassiveVentMechanoid", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_SolarGenerator", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_SolarGeneratorArchotech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
            PatchTex("ShipInside_SolarGeneratorMech", "Things/Building/Linked/GravshipHull/GravshipHull_Atlas");
        }

        private static void ApplyOdysseyThrustersPatch()
        {
            PatchThruster("Ship_Engine_Small", "Things/Building/SmallThruster/SmallThruster", new Vector2(2f, 3f));
            PatchThruster("Ship_Engine", "Things/Building/LateralThruster/LateralThruster", new Vector2(3f, 3f));
        }

        public static void ApplyIfNeed_AcceleratingParticles()
        {
            bool isEnabled = SaveOurCat.settings?.Soc_Flag_AccelerationParticles ?? false;
            
            if (!IsSaveOurShipLoaded())
            {
                return;
            }

            if (!isEnabled && !Soc_AccelerationParticles_Applied)
            {
                RemoveAcceleratingParticles();
                Soc_AccelerationParticles_Applied = true;
            }
        }

        public static void ApplyIfNeed_RCSLayerFix()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_RCSLayerFix ?? false) || Soc_RCSLayerFix_Applied)
            {
                return;
            }

            if (!IsSaveOurShipLoaded())
            {
                return;
            }

            ApplyRCSLayerFix();
            Soc_RCSLayerFix_Applied = true;
        }

        private static void ApplyRCSLayerFix()
        {
            var thruster = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_Thruster");
            if (thruster != null)
            {
                thruster.altitudeLayer = AltitudeLayer.FloorEmplacement;
            }
        }

        private static void ApplyAcceleratingParticles()
        {
            var designationCategory = DefDatabase<DesignationCategoryDef>.GetNamed("Ship");
            var designatorDropdown = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail("Ship_Engines");
            
            var antiproton = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_AntiprotoniumThruster");
            if (antiproton != null)
            {
                if (designatorDropdown != null)
                {
                    antiproton.designatorDropdown = designatorDropdown;
                }
                antiproton.designationCategory = designationCategory;
            }
            
            var ion = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_IonThruster");
            if (ion != null)
            {
                if (designatorDropdown != null)
                {
                    ion.designatorDropdown = designatorDropdown;
                }
                ion.designationCategory = designationCategory;
            }
            
            var recipe = DefDatabase<RecipeDef>.GetNamedSilentFail("MakeAntiprotoniumPods");
            if (recipe != null)
            {
                if (recipe.recipeUsers == null)
                {
                    recipe.recipeUsers = new System.Collections.Generic.List<ThingDef>();
                }
                
                var table = DefDatabase<ThingDef>.GetNamedSilentFail("TableMachining");
                if (table != null && !recipe.recipeUsers.Contains(table))
                {
                    recipe.recipeUsers.Add(table);
                }
            }
        }

        private static void RemoveAcceleratingParticles()
        {
            var antiproton = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_AntiprotoniumThruster");
            if (antiproton != null)
            {
                antiproton.designatorDropdown = null;
                antiproton.designationCategory = null;
            }
            
            var ion = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_IonThruster");
            if (ion != null)
            {
                ion.designatorDropdown = null;
                ion.designationCategory = null;
            }
            
            var recipe = DefDatabase<RecipeDef>.GetNamedSilentFail("MakeAntiprotoniumPods");
            if (recipe != null && recipe.recipeUsers != null)
            {
                var table = DefDatabase<ThingDef>.GetNamedSilentFail("TableMachining");
                if (table != null)
                {
                    recipe.recipeUsers.Remove(table);
                }
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

        private static void ApplyEnergyPatchInternal(string defName, float newPower)
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
}