
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SaveOurCat
{
    public class Soc_ModSettings : ModSettings
    {
        public bool Soc_Flag_OdysseyHullGraphics = false;
        public bool Soc_Flag_AccelerationParticles = true;
        public bool Soc_Flag_SmallNuclearEngine = true;
        public bool Soc_Flag_ShipCargoTerminal = false;
        public bool Soc_Flag_SOSNuclearThrusterGraphics = false;
        public bool Soc_Flag_SOSSalvageBayGraphics = false;
        public bool Soc_Flag_SOSChemEnginesGraphics = false;
        public bool Soc_Flag_SOSShuttleBayGraphics = false;
        public bool Soc_Flag_SOSRCSEnginesGraphics = false;
        public bool Soc_Flag_RussianTranslation = false;
        public bool Soc_Flag_LifeSupportEnergy = false;
        public bool Soc_Flag_OdysseyThrustersGraphics = false;
        public bool Soc_Flag_RCSLayerFix = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Soc_Flag_OdysseyHullGraphics, "enableOdysseyHullGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_AccelerationParticles, "enableAccelerationParticles", true);
            Scribe_Values.Look(ref Soc_Flag_SmallNuclearEngine, "enableSmallNuclearEngine", true);
            Scribe_Values.Look(ref Soc_Flag_ShipCargoTerminal, "enableShipCargoTerminal", false);
            Scribe_Values.Look(ref Soc_Flag_SOSNuclearThrusterGraphics, "enableSOSNuclearThrusterGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_SOSSalvageBayGraphics, "enableSOSSalvageBayGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_SOSChemEnginesGraphics, "enableSOSChemEnginesGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_SOSShuttleBayGraphics, "enableSOSShuttleBayGraphics", false);
            Scribe_Values.Look(ref Soc_Flag_SOSRCSEnginesGraphics, "enableSOSRCSEnginesGraphics", false);
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
            Soc_PatchLogics.ApplyIfNeed_SmallNuclearEngine();
            ShipCargoTerminalControl.ApplyShipCargoTerminalSetting(settings.Soc_Flag_ShipCargoTerminal);
            Soc_PatchLogics.ApplyIfNeed_SOSNuclearThrusterGraphics();
            Soc_PatchLogics.ApplyIfNeed_SOSSalvageBayGraphics();
            Soc_PatchLogics.ApplyIfNeed_SOSChemEnginesGraphics();
            Soc_PatchLogics.ApplyIfNeed_SOSShuttleBayGraphics();
            Soc_PatchLogics.ApplyIfNeed_SOSRCSEnginesGraphics();
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
    // Линия разделения
    Widgets.DrawLineVertical(inRect.center.x, inRect.yMin, inRect.height);

    // Левая колонка
    Rect leftRect = new Rect(inRect.x, inRect.y, inRect.width / 2 - 6f, inRect.height);
    Listing_Standard leftListing = new Listing_Standard();
    leftListing.Begin(leftRect);

    // Addons
    Text.Anchor = TextAnchor.MiddleCenter;
    leftListing.Label("SocSetts.Addons.title".Translate());
    Text.Anchor = TextAnchor.UpperLeft;
    leftListing.GapLine();
    leftListing.CheckboxLabeled("SocSetts.AcceleratingParticles.label".Translate(), ref settings.Soc_Flag_AccelerationParticles, "SocSetts.AcceleratingParticles.desc".Translate());
    leftListing.CheckboxLabeled("SocSetts.SmallNuclearEngine.label".Translate(), ref settings.Soc_Flag_SmallNuclearEngine, "SocSetts.SmallNuclearEngine.desc".Translate());
    leftListing.CheckboxLabeled("SocSetts.ShipCargoTerminal.label".Translate(), ref settings.Soc_Flag_ShipCargoTerminal, "SocSetts.ShipCargoTerminal.desc".Translate());
    leftListing.Gap();

    // Ships
    Text.Anchor = TextAnchor.MiddleCenter;
    leftListing.Label("SocSetts.Ships.title".Translate());
    Text.Anchor = TextAnchor.UpperLeft;
    leftListing.GapLine();

    // StarSector Ships
    Rect starSectorRect = leftListing.GetRect(24f);
    Rect starSectorLabelRect = new Rect(starSectorRect.x, starSectorRect.y, starSectorRect.width - 80, starSectorRect.height);
    Widgets.Label(starSectorLabelRect, "SocSetts.StarSectorShips.label".Translate());
    TooltipHandler.TipRegion(starSectorLabelRect, "SocSetts.StarSectorShips.desc".Translate());
    GUI.color = Color.green;
    Widgets.Label(new Rect(starSectorRect.xMax - 90, starSectorRect.y, 90, 24), "SocSetts.Active.stat".Translate());
    GUI.color = Color.white;
    leftListing.Gap(4f);

    // X4 Foundations Ships
    Rect x4Rect = leftListing.GetRect(24f);
    Rect x4LabelRect = new Rect(x4Rect.x, x4Rect.y, x4Rect.width - 80, x4Rect.height);
    Widgets.Label(x4LabelRect, "SocSetts.X4FoundationsShips.label".Translate());
    TooltipHandler.TipRegion(x4LabelRect, "SocSetts.X4FoundationsShips.desc".Translate());
    GUI.color = Color.green;
    Widgets.Label(new Rect(x4Rect.xMax - 90, x4Rect.y, 90, 24), "SocSetts.Active.stat".Translate());
    GUI.color = Color.white;
    leftListing.Gap(4f);

    // Other Ships
    Rect otherRect = leftListing.GetRect(24f);
    Rect otherLabelRect = new Rect(otherRect.x, otherRect.y, otherRect.width - 80, otherRect.height);
    Widgets.Label(otherLabelRect, "SocSetts.DifferentShips.label".Translate());
    TooltipHandler.TipRegion(otherLabelRect, "SocSetts.DifferentShips.desc".Translate());
    GUI.color = Color.green;
    Widgets.Label(new Rect(otherRect.xMax - 90, otherRect.y, 90, 24), "SocSetts.Active.stat".Translate());
    GUI.color = Color.white;
    leftListing.Gap(4f);

    leftListing.Gap();

    // Patches
    Rect patchesTitleRect = leftListing.GetRect(24f);
    Text.Anchor = TextAnchor.MiddleCenter;
    Widgets.Label(patchesTitleRect, "SocSetts.Patches.Title".Translate());
    Text.Anchor = TextAnchor.UpperLeft;
    leftListing.GapLine();
    leftListing.CheckboxLabeled("SocSetts.LifeSupportEnergy.label".Translate(), ref settings.Soc_Flag_LifeSupportEnergy, "SocSetts.LifeSupportEnergy.desc".Translate());
    leftListing.CheckboxLabeled("SocSetts.RussianTranslation.label".Translate(), ref settings.Soc_Flag_RussianTranslation, "SocSetts.RussianTranslation.desc".Translate());

    leftListing.End();

    // Правая колонка
    Rect rightRect = new Rect(inRect.center.x + 6f, inRect.y, inRect.width / 2 - 6f, inRect.height);
    Listing_Standard rightListing = new Listing_Standard();
    rightListing.Begin(rightRect);

    // Graphics
    Text.Anchor = TextAnchor.MiddleCenter;
    rightListing.Label("SocSetts.Graphics.title".Translate());
    Text.Anchor = TextAnchor.UpperLeft;
    rightListing.GapLine();
    rightListing.CheckboxLabeled("SocSetts.OdysseyHullGraphics.label".Translate(), ref settings.Soc_Flag_OdysseyHullGraphics, "SocSetts.OdysseyHullGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.OdysseyThrustersGraphics.label".Translate(), ref settings.Soc_Flag_OdysseyThrustersGraphics, "SocSetts.OdysseyThrustersGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.RCSLayerFix.label".Translate(), ref settings.Soc_Flag_RCSLayerFix, "SocSetts.RCSLayerFix.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.SOSNuclearThrusterGraphics.label".Translate(), ref settings.Soc_Flag_SOSNuclearThrusterGraphics, "SocSetts.SOSNuclearThrusterGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.SOSSalvageBayGraphics.label".Translate(), ref settings.Soc_Flag_SOSSalvageBayGraphics, "SocSetts.SOSSalvageBayGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.SOSChemEnginesGraphics.label".Translate(), ref settings.Soc_Flag_SOSChemEnginesGraphics, "SocSetts.SOSChemEnginesGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.SOSShuttleBayGraphics.label".Translate(), ref settings.Soc_Flag_SOSShuttleBayGraphics, "SocSetts.SOSShuttleBayGraphics.desc".Translate());
    rightListing.CheckboxLabeled("SocSetts.SOSRCSEnginesGraphics.label".Translate(), ref settings.Soc_Flag_SOSRCSEnginesGraphics, "SocSetts.SOSRCSEnginesGraphics.desc".Translate());

    rightListing.End();
}


        public override string SettingsCategory() => "Save Our Cat";
    }

    public static class Soc_PatchLogics
    {
        private static HashSet<string> appliedPatches = new HashSet<string>();
        private static bool? _isSaveOurShipLoaded;

        private static bool AlreadyApplied(string key) => appliedPatches.Contains(key);
        private static void MarkApplied(string key) => appliedPatches.Add(key);

        private static bool IsSaveOurShipLoadedCached()
        {
            if (_isSaveOurShipLoaded == null)
                _isSaveOurShipLoaded = IsSaveOurShipLoaded();
            return _isSaveOurShipLoaded.Value;
        }
        public static void ApplyIfNeed_OdysseyHullGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_OdysseyHullGraphics ?? false) || AlreadyApplied("OdysseyHullGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplyOdysseyTexPatch();
            MarkApplied("OdysseyHullGraphics");
        }

        public static void ApplyIfNeedOdysseyThrustersGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_OdysseyThrustersGraphics ?? false) || AlreadyApplied("OdysseyThrusters"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplyOdysseyThrustersPatch();
            MarkApplied("OdysseyThrusters");
        }

        public static void ApplyIfNeed_LifeSupportEnergy()
        {
            if (AlreadyApplied("LifeSupportEnergy"))
            {
                return;
            }

            ApplyEnergyPatchInternal("Ship_LifeSupport", 400f);
            ApplyEnergyPatchInternal("Ship_LifeSupport_Small", 800f);
            MarkApplied("LifeSupportEnergy");
        }

        public static void ApplyIfNeed_RussianTranslation()
        {
            if (AlreadyApplied("RussianTranslation"))
            {
                return;
            }

            PatchDraftLabel("SoS2_Shuttle_Personal");
            PatchDraftLabel("SoS2_Shuttle");
            PatchDraftLabel("SoS2_Shuttle_Heavy");
            PatchDraftLabel("SoS2_Shuttle_Superheavy");
            MarkApplied("RussianTranslation");
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
            string texPath = "Things/Building/Linked/GravshipHull/GravshipHull_Atlas";
            
            string[] linkDefs = { "Ship_Beam", "Ship_Beam_Unpowered", "ShipAirlockBeam" };
            foreach (var defName in linkDefs)
            {
                PatchThingGraphic(defName, texPath, typeof(Graphic_Single), LinkDrawerType.Basic);
            }
            
            string[] texDefs = {
                "ShipInside_PassiveVent", "ShipInside_PassiveVentArchotech", "ShipInside_PassiveVentMechanoid",
                "ShipInside_SolarGenerator", "ShipInside_SolarGeneratorArchotech", "ShipInside_SolarGeneratorMech"
            };
            foreach (var defName in texDefs)
            {
                PatchThingGraphic(defName, texPath);
            }
        }

        private static void ApplyOdysseyThrustersPatch()
        {
            PatchThingGraphic("Ship_Engine_Small", "Things/Building/SmallThruster/SmallThruster", drawSize: new Vector2(2f, 3f));
            PatchThingGraphic("Ship_Engine", "Things/Building/LateralThruster/LateralThruster", drawSize: new Vector2(3f, 3f));
        }

        public static void ApplyIfNeed_AcceleratingParticles()
        {
            bool isEnabled = SaveOurCat.settings?.Soc_Flag_AccelerationParticles ?? false;
            
            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            if (!isEnabled && !AlreadyApplied("AccelerationParticles"))
            {
                RemoveAcceleratingParticles();
                MarkApplied("AccelerationParticles");
            }
        }

        public static void ApplyIfNeed_SmallNuclearEngine()
        {
            bool isEnabled = SaveOurCat.settings?.Soc_Flag_SmallNuclearEngine ?? false;
            
            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            if (!isEnabled && !AlreadyApplied("SmallNuclearEngine"))
            {
                RemoveSmallNuclearEngine();
                MarkApplied("SmallNuclearEngine");
            }
        }

        public static void ApplyIfNeed_SOSNuclearThrusterGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_SOSNuclearThrusterGraphics ?? false) || AlreadyApplied("SOSNuclearThrusterGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplySOSNuclearThrusterGraphicsPatch();
            MarkApplied("SOSNuclearThrusterGraphics");
        }

        public static void ApplyIfNeed_SOSSalvageBayGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_SOSSalvageBayGraphics ?? false) || AlreadyApplied("SOSSalvageBayGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplySOSSalvageBayGraphicsPatch();
            MarkApplied("SOSSalvageBayGraphics");
        }

        public static void ApplyIfNeed_SOSChemEnginesGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_SOSChemEnginesGraphics ?? false) || AlreadyApplied("SOSChemEnginesGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            // Skip if OdysseyThrustersGraphics is enabled (they conflict on Ship_Engine and Ship_Engine_Small)
            if (SaveOurCat.settings?.Soc_Flag_OdysseyThrustersGraphics ?? false)
            {
                MarkApplied("SOSChemEnginesGraphics");
                return;
            }

            ApplySOSChemEnginesGraphicsPatch();
            MarkApplied("SOSChemEnginesGraphics");
        }

        public static void ApplyIfNeed_SOSShuttleBayGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_SOSShuttleBayGraphics ?? false) || AlreadyApplied("SOSShuttleBayGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplySOSShuttleBayGraphicsPatch();
            MarkApplied("SOSShuttleBayGraphics");
        }

        public static void ApplyIfNeed_SOSRCSEnginesGraphics()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_SOSRCSEnginesGraphics ?? false) || AlreadyApplied("SOSRCSEnginesGraphics"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplySOSRCSEnginesGraphicsPatch();
            MarkApplied("SOSRCSEnginesGraphics");
        }

        public static void ApplyIfNeed_RCSLayerFix()
        {
            if (!(SaveOurCat.settings?.Soc_Flag_RCSLayerFix ?? false) || AlreadyApplied("RCSLayerFix"))
            {
                return;
            }

            if (!IsSaveOurShipLoadedCached())
            {
                return;
            }

            ApplyRCSLayerFix();
            MarkApplied("RCSLayerFix");
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
            
            PatchThrusterCategory("Ship_AntiprotoniumThruster", designatorDropdown, designationCategory);
            PatchThrusterCategory("Ship_IonThruster", designatorDropdown, designationCategory);
            
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
            PatchThrusterCategory("Ship_AntiprotoniumThruster", null, null);
            PatchThrusterCategory("Ship_IonThruster", null, null);
            
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

        private static void RemoveSmallNuclearEngine()
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail("Ship_NuclearEngineSmall");
            if (def != null)
            {
                def.designatorDropdown = null;
                def.designationCategory = null;
            }
        }

        private static void ApplySOSNuclearThrusterGraphicsPatch()
        {
            PatchThingGraphic("Ship_Engine_Large", "Things/Building/Ship/Replace/NuclearEngine", drawSize: new Vector2(5.04f, 7f));
        }

        private static void ApplySOSSalvageBayGraphicsPatch()
        {
            PatchThingGraphic("ShipSalvageBay", "Things/Building/Ship/Replace/SalvageBay");
            PatchThingGraphic("ShipSalvageBayNano", "Things/Building/Ship/Replace/SalvageBayNano");
        }

        private static void ApplySOSChemEnginesGraphicsPatch()
        {
            PatchThingGraphic("Ship_Engine", "Things/Building/Ship/Replace/ShipEngineRocket");
            PatchThingGraphic("Ship_Engine_Small", "Things/Building/Ship/Replace/ShipEngineRocket_Single");
        }

        private static void ApplySOSShuttleBayGraphicsPatch()
        {
            PatchThingGraphic("ShipShuttleBay", "Things/Building/Ship/Replace/Shuttle_Bay", typeof(Graphic_Single));
            PatchThingGraphic("ShipShuttleBayLarge", "Things/Building/Ship/Replace/Shuttle_Bay", typeof(Graphic_Single));
        }

        private static void ApplySOSRCSEnginesGraphicsPatch()
        {
            PatchThingGraphic("Ship_Thruster", "Things/Building/Ship/Replace/ShipThruster", typeof(Graphic_Multi), drawSize: new Vector2(2.3f, 2.3f));
        }

        private static void PatchThrusterCategory(string defName, DesignatorDropdownGroupDef dropdown, DesignationCategoryDef category)
        {
            var thruster = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (thruster != null)
            {
                thruster.designatorDropdown = dropdown;
                thruster.designationCategory = category;
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

        private static void PatchThingGraphic(string defName, string texPath, 
            System.Type graphicClass = null, 
            LinkDrawerType? linkType = null, 
            Vector2? drawSize = null)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def?.graphicData == null)
            {
                Log.Warning($"[SoC] Failed to patch {defName}: def or graphicData is null");
                return;
            }

            if (texPath != null) def.graphicData.texPath = texPath;
            if (graphicClass != null) def.graphicData.graphicClass = graphicClass;
            if (linkType != null) def.graphicData.linkType = linkType.Value;
            if (drawSize != null) def.graphicData.drawSize = drawSize.Value;
            
            Log.Message($"[SoC] Successfully patched {defName} with texture {texPath}");
        }
    }
}