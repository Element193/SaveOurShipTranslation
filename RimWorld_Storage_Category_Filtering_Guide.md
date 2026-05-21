# RimWorld Item Category Filtering Guide for Storage Buildings

## 1. Vanilla RimWorld Thing Categories

RimWorld defines the following **ThingCategory** types (from the vanilla core definitions):

### Primary Thing Categories:
- **Item** - General items/inventory objects
- **Pawn** - Living creatures (colonists, animals, enemies)
- **Building** - Buildings and structures
- **Plant** - Flora and vegetation
- **Filth** - Waste and pollution
- **Ethereal** - Non-physical entities

### Common Item Subcategories (filterable):
- **Apparel** - Clothing and armor
- **Weapon** - Melee and ranged weapons
- **Shell** - Ammunition and mortars
- **Chunk** - Stone and metal chunks
- **Medicine** - Medical supplies
- **Food** - Consumable food items
- **Drugs** - Recreational and medicinal drugs
- **Plant** - Seeds and crops
- **Leather** - Leather materials
- **Textile** - Cloth and fabrics
- **Material** - Generic crafting materials
- **Corpse** - Bodies of dead creatures
- **ResourcePod** - Supply pods and containers
- **Art** - Sculptures and artwork
- **Trap** - Traps and explosives

---

## 2. XML Examples: Using StorageSettings with Filters

### Example 1: Basic Storage Building with Category Filter

```xml
<?xml version="1.0" encoding="utf-8"?>
<Defs>

  <ThingDef ParentName="StorageShelfBase">
    <defName>WeaponStorage</defName>
    <label>Weapon Rack</label>
    <description>A specialized storage rack for weapons only.</description>
    <graphicData>
      <texPath>Things/Building/Storage/WeaponRack</texPath>
      <drawSize>(2,1)</drawSize>
    </graphicData>
    <size>(2,1)</size>
    <building>
      <storageGroupTag>Shelf</storageGroupTag>
      <ignoreStoredThingsBeauty>true</ignoreStoredThingsBeauty>
      <maxItemsInCell>5</maxItemsInCell>
      <!-- Fixed Storage Settings - restricts to weapons only -->
      <fixedStorageSettings>
        <filter>
          <categories>
            <li>Weapon</li>
          </categories>
        </filter>
      </fixedStorageSettings>
    </building>
  </ThingDef>

</Defs>
```

### Example 2: Medicine Storage with Multiple Allowed Categories

```xml
<ThingDef ParentName="StorageShelfBase">
  <defName>MedicineStorage</defName>
  <label>Medicine Cabinet</label>
  <description>Specialized storage for medical supplies and medicine.</description>
  <size>(2,1)</size>
  <building>
    <storageGroupTag>Shelf</storageGroupTag>
    <ignoreStoredThingsBeauty>true</ignoreStoredThingsBeauty>
    <maxItemsInCell>8</maxItemsInCell>
    <!-- Allow multiple categories -->
    <fixedStorageSettings>
      <filter>
        <categories>
          <li>Medicine</li>
          <li>Drug</li>
        </categories>
      </filter>
    </fixedStorageSettings>
  </building>
</ThingDef>
```

### Example 3: Apparel Storage with Specific Item Exclusions

```xml
<ThingDef ParentName="StorageShelfBase">
  <defName>ApparelStorage</defName>
  <label>Clothing Rack</label>
  <description>Storage for clothing and apparel.</description>
  <size>(3,2)</size>
  <building>
    <storageGroupTag>Shelf</storageGroupTag>
    <maxItemsInCell>10</maxItemsInCell>
    <!-- Allow apparel category but exclude specific items -->
    <fixedStorageSettings>
      <filter>
        <categories>
          <li>Apparel</li>
        </categories>
        <disallowedThingDefs>
          <!-- Exclude power armor if too bulky -->
          <li>Apparel_PowerArmor</li>
          <li>Apparel_PowerArmorHelmet</li>
        </disallowedThingDefs>
      </filter>
    </fixedStorageSettings>
  </building>
</ThingDef>
```

### Example 4: Food & Drug Storage (Exclude Alcohol)

```xml
<ThingDef ParentName="StorageShelfBase">
  <defName>FoodStorage</defName>
  <label>Pantry</label>
  <description>Storage for food, meals, and provisioning.</description>
  <size>(3,1)</size>
  <building>
    <storageGroupTag>Shelf</storageGroupTag>
    <maxItemsInCell>20</maxItemsInCell>
    <fixedStorageSettings>
      <filter>
        <categories>
          <li>Food</li>
        </categories>
        <!-- Explicitly exclude recreational drugs -->
        <disallowedThingDefs>
          <li>Beer</li>
          <li>Whiskey</li>
          <li>Wine</li>
        </disallowedThingDefs>
      </filter>
    </fixedStorageSettings>
  </building>
</ThingDef>
```

### Example 5: Ship Cargo Terminal with Custom Filtering

```xml
<!-- Your ShipCargoTerminal example updated with category filtering -->
<ThingDef ParentName="StorageShelfBase">
  <defName>ShipCargoTerminal</defName>
  <label>Ship Cargo Terminal</label>
  <description>Advanced cargo storage for ships. Accepts most items.</description>
  <thingClass>SaveOurCat.ShipStorageClass</thingClass>
  <graphicData>
    <texPath>Things/Building/Ship/ShipCargoTerminal</texPath>
    <graphicClass>Graphic_Multi</graphicClass>
    <drawSize>(3,3)</drawSize>
  </graphicData>
  <size>(2,1)</size>
  <building>
    <storageGroupTag>Shelf</storageGroupTag>
    <ignoreStoredThingsBeauty>true</ignoreStoredThingsBeauty>
    <preventDeteriorationOnTop>true</preventDeteriorationOnTop>
    <maxItemsInCell>15</maxItemsInCell>
    <!-- Optional: restrict cargo types if needed -->
    <defaultStorageSettings>
      <filter>
        <categories>
          <li>Item</li>
        </categories>
        <!-- Exclude things that shouldn't go in cargo -->
        <disallowedThingDefs>
          <li>Human_Corpse</li>
          <li>Chunk_Plasteel</li>
        </disallowedThingDefs>
      </filter>
    </defaultStorageSettings>
  </building>
</ThingDef>
```

---

## 3. C# Code Examples: Filtering Storage by Category

### Example 1: Custom Building_Storage with Category Filter

```csharp
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace SaveOurCat
{
    /// <summary>
    /// Storage building that restricts items to specific categories
    /// </summary>
    public class CategorizedStorageBuilding : Building_Storage
    {
        /// <summary>
        /// Override Accepts to add category-based filtering
        /// </summary>
        public override bool Accepts(Thing t)
        {
            // First check the base storage settings filter
            if (!base.Accepts(t))
                return false;

            // Apply additional category restrictions
            if (!AllowedCategories.Contains(t.def.category))
                return false;

            return true;
        }

        /// <summary>
        /// Define allowed thing categories for this storage
        /// </summary>
        public virtual List<ThingCategory> AllowedCategories
        {
            get { return new List<ThingCategory> { ThingCategory.Item }; }
        }
    }
}
```

### Example 2: Weapon-Only Storage

```csharp
public class WeaponStorage : Building_Storage
{
    public override bool Accepts(Thing t)
    {
        // Must pass base filter
        if (!base.Accepts(t))
            return false;

        // Must be a weapon
        if (t.def.category != ThingCategory.Item)
            return false;

        // Must have weapon properties
        if (t is ThingWithComps thingWithComps)
        {
            var equipComp = thingWithComps.GetComp<CompEquippable>();
            if (equipComp == null)
                return false;

            // Check if it's actually a weapon
            return equipComp.EquippedBy == null; // Not currently equipped
        }

        return false;
    }
}
```

### Example 3: Medicine & Drug Filtering

```csharp
public class MedicalStorage : Building_Storage
{
    private static readonly List<string> AllowedThingDefs = new()
    {
        "Medicine",
        "MedicineIndustrial",
        "MedicineUltratech",
        "Glitterworld",
        "Penoxycyline",
        "WakeUp",
        "GoJuice",
        "Yayo",
        "Psychite"
    };

    public override bool Accepts(Thing t)
    {
        if (!base.Accepts(t))
            return false;

        // Check if item is in allowed list
        return AllowedThingDefs.Contains(t.def.defName);
    }

    public override void GetChildHolders(List<IThingHolder> outChildren)
    {
        base.GetChildHolders(outChildren);
        
        // Track only medicine items
        if (settings != null)
        {
            Log.Message($"Medical Storage contains {GetSlotGroup().HeldThings.Count} items");
        }
    }
}
```

### Example 4: Storage with Dynamic Category Filtering

```csharp
public class DynamicCategoryStorage : Building_Storage
{
    // XML-configurable category restrictions
    [PostLoadInit]
    public List<string> allowedCategories = new()
    {
        "Item"
    };

    public override bool Accepts(Thing t)
    {
        if (!base.Accepts(t))
            return false;

        // Dynamic category checking based on XML config
        string categoryName = t.def.category.ToString();
        
        return allowedCategories.Contains(categoryName);
    }
}
```

### Example 5: Filtering with Storage Settings

```csharp
/// <summary>
/// Check what items a storage building is accepting
/// </summary>
public static bool IsItemAllowedInStorage(Building_Storage storage, Thing item)
{
    // Get the storage's filter settings
    StorageSettings settings = storage.GetStoreSettings();
    
    if (settings == null)
        return storage.Accepts(item);

    // Check against the filter
    return settings.filter.Allows(item);
}

/// <summary>
/// Restrict storage to specific thingCategories
/// </summary>
public static void RestrictStorageToCategories(Building_Storage storage, 
    params ThingCategory[] categories)
{
    StorageSettings settings = storage.GetStoreSettings();
    
    if (settings?.filter == null)
        return;

    // Clear existing filters
    settings.filter.SetDisallowAll();

    // Allow only specified categories
    foreach (var category in categories)
    {
        foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListByName)
        {
            if (thingDef.category == category)
            {
                settings.filter.SetAllow(thingDef, true);
            }
        }
    }
}

/// <summary>
/// Example usage
/// </summary>
public void SetupWeaponStorage(Building_Storage weaponRack)
{
    RestrictStorageToCategories(weaponRack, ThingCategory.Item);
    
    // But also filter to weapons only
    StorageSettings settings = weaponRack.GetStoreSettings();
    foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListByName)
    {
        bool isWeapon = thingDef.tools?.Count > 0 || 
                       thingDef.GetComp<CompEquippable>() != null;
        settings.filter.SetAllow(thingDef, isWeapon);
    }
}
```

---

## 4. Alternative Storage Filtering Methods

### Method 1: Using StorageSettings Filter (Most Common)

**Pros:** Flexible, respects player configuration, integrates with UI  
**Cons:** Requires UI interactions to modify

```csharp
// Get storage settings filter
StorageSettings storeSettings = storage.GetStoreSettings();
bool allowsItem = storeSettings.filter.Allows(item);

// Modify filter programmatically
storeSettings.filter.SetAllow(thingDef, true);
```

### Method 2: Override Accepts() Method (Storage Building Class)

**Pros:** Direct control, no UI needed, enforced at code level  
**Cons:** Ignores player modifications, less flexible

```csharp
public override bool Accepts(Thing t)
{
    return base.Accepts(t) && t.def.category == ThingCategory.Item;
}
```

### Method 3: Harmony Patches on Storage Methods

**Pros:** Modify existing storage without changing class  
**Cons:** Fragile, performance impact, compatibility issues

```csharp
[HarmonyPatch(typeof(Building_Storage), nameof(Building_Storage.Accepts))]
public static class Patch_StorageAccepts
{
    public static void Postfix(Building_Storage __instance, Thing t, ref bool __result)
    {
        if (__instance.def.defName == "MySpecialStorage")
        {
            __result = __result && t.def.category != ThingCategory.Corpse;
        }
    }
}
```

### Method 4: Adaptive Storage Framework Extensions (Recommended for Mods)

**Pros:** Uses ASF system, works with multiple storage mods  
**Cons:** Requires ASF as dependency

```xml
<!-- In ModExtension for your storage -->
<Adaptive_Storage.Extension>
    <thingClass>SaveOurCat.ShipStorageClass</thingClass>
    <maxItemsPerCell>15</maxItemsPerCell>
</Adaptive_Storage.Extension>
```

### Method 5: Storage Group Settings

**Pros:** Affects multiple buildings together  
**Cons:** Requires storage group setup

```csharp
public void ApplyCategoryFilterToGroup(StorageGroup group, ThingCategory category)
{
    if (group?.settings?.filter == null)
        return;

    group.settings.filter.SetDisallowAll();
    
    foreach (var def in DefDatabase<ThingDef>.AllDefsListByName)
    {
        if (def.category == category)
            group.settings.filter.SetAllow(def, true);
    }
}
```

---

## 5. Best Practices for Storage Filtering

### 1. **Always Check Base Filter First**
```csharp
public override bool Accepts(Thing t)
{
    // Always call base implementation first
    if (!base.Accepts(t))
        return false;
        
    // Then add your category logic
    return AllowedCategories.Contains(t.def.category);
}
```

### 2. **Use XML for Configuration**
```xml
<!-- This allows modders to configure without code changes -->
<fixedStorageSettings>
  <filter>
    <categories>
      <li>Apparel</li>
      <li>Weapon</li>
    </categories>
  </filter>
</fixedStorageSettings>
```

### 3. **Cache Allowed Categories for Performance**
```csharp
private List<ThingCategory> _cachedAllowedCategories;

public override bool Accepts(Thing t)
{
    if (!base.Accepts(t))
        return false;

    if (_cachedAllowedCategories == null)
        _cachedAllowedCategories = GetAllowedCategoriesFromXML();

    return _cachedAllowedCategories.Contains(t.def.category);
}
```

### 4. **Provide UI Feedback**
```csharp
public override string GetInspectString()
{
    string baseInfo = base.GetInspectString();
    string categories = string.Join(", ", AllowedCategories);
    return $"{baseInfo}\nAccepts: {categories}";
}
```

### 5. **Test with DefDatabase**
```csharp
// Verify your filters catch the right items
public static void TestStorageFilter(Building_Storage storage)
{
    int accepted = 0;
    foreach (var def in DefDatabase<ThingDef>.AllDefsListByName)
    {
        if (storage.Accepts(ThingMaker.MakeThing(def)))
            accepted++;
    }
    Log.Message($"Storage accepts {accepted} item types");
}
```

---

## 6. Common RimWorld Item Categories Reference

| Category | Contains | Notes |
|----------|----------|-------|
| **Apparel** | Clothing, armor, accessories | EquipmentSlot defines wear location |
| **Weapon** | Melee/ranged weapons | Check tools[] and EquipmentType |
| **Shell** | Ammunition, mortars | For turrets and launchers |
| **Medicine** | Medical supplies | Quality affects effectiveness |
| **Food** | Meals, meat, vegetables | Nutritiousness varies |
| **Drug** | Alcohol, psychite, etc | Addiction causes issues |
| **Plant** | Seeds, crops | Sow-able and harvestable |
| **Leather** | Hide, skin materials | Used in recipes |
| **Textile** | Cloth, insulation | Building/apparel material |
| **Resource** | Generic materials | Base crafting materials |
| **Chunk** | Stone/metal chunks | Mineable materials |
| **Corpse** | Dead bodies | Requires special handling |
| **Filth** | Waste products | Usually excluded |

---

## 7. Integration with Your ShipCargoTerminal

For your **ShipCargoTerminal**, you could optionally add category filtering:

```csharp
public class ShipStorageClass : Building_Storage
{
    // Your existing code...

    /// <summary>
    /// Optional: Exclude certain categories from ship storage
    /// </summary>
    public override bool Accepts(Thing t)
    {
        // Don't accept corpses or fresh items that decay
        if (t.def.category == ThingCategory.Corpse)
            return false;

        // Don't accept items in chunks (mineable material)
        if (t.def.stackLimit == 1 && t.def.HasComp(typeof(CompRottable)))
            return false;

        // Otherwise, accept if base allows and storage has space
        return base.Accepts(t);
    }
}
```

---

## 8. Testing Your Filters

**In Development Mode:**
```
1. Enable "God Mode" (Ctrl+Shift+F12)
2. Place your storage building
3. Try to haul different item types to it
4. Check the log for filtering messages
5. Use "Inspect" to verify accepted items
```

**Programmatic Testing:**
```csharp
[Test]
public void TestWeaponStorageFilter()
{
    var storage = DefDatabase<ThingDef>.GetNamed("WeaponRack");
    Assert.IsTrue(storage.building.fixedStorageSettings.filter.Allows(ThingMaker.MakeThing(ThingDefOf.Gun_Pistol)));
    Assert.IsFalse(storage.building.fixedStorageSettings.filter.Allows(ThingMaker.MakeThing(ThingDefOf.Apparel_Pants_Leather)));
}
```

---

## 9. Vanilla RimWorld Storage Building Examples

### OutfitStand Base (Odyssey DLC) - Abstract Base

```xml
<ThingDef ParentName="FurnitureBase" Name="OutfitStandBase" Abstract="True">
  <drawerType>RealtimeOnly</drawerType>
  <altitudeLayer>Building</altitudeLayer>
  <passability>PassThroughOnly</passability>
  <fillPercent>0.4</fillPercent>
  <castEdgeShadows>false</castEdgeShadows>
  <pathCost>50</pathCost>
  <researchPrerequisites>
    <li>ComplexFurniture</li>
  </researchPrerequisites>
  <stuffCategories>
    <li>Metallic</li>
    <li>Woody</li>
    <li>Stony</li>
  </stuffCategories>
  <defaultPlacingRot>South</defaultPlacingRot>
  <statBases>
    <Flammability>1.0</Flammability>
    <Beauty>0.5</Beauty>
    <MaxHitPoints>60</MaxHitPoints>
    <Mass>3</Mass>
    <WorkToBuild>350</WorkToBuild>
  </statBases>

  <building>
    <!-- Blueprint class for storage UI -->
    <blueprintClass>Blueprint_StorageWithRoomHighlight</blueprintClass>
    
    <!-- Whether stored items affect room beauty -->
    <ignoreStoredThingsBeauty>false</ignoreStoredThingsBeauty>
    
    <!-- FIXED SETTINGS: Cannot be modified by players -->
    <fixedStorageSettings>
      <filter>
        <!-- Prevents storing items that can't normally be stored -->
        <disallowNotEverStorable>true</disallowNotEverStorable>
        
        <!-- Inherit="False" means override parent categories entirely -->
        <categories Inherit="False">
          <li>Apparel</li>
          <li>Weapon</li>
        </categories>
      </filter>
    </fixedStorageSettings>
    
    <!-- DEFAULT SETTINGS: Players CAN modify these -->
    <defaultStorageSettings>
      <priority>Important</priority>
      <filter>
        <!-- Allowed categories -->
        <categories>
          <li>Apparel</li>
        </categories>
        <!-- Disallowed subcategories (removes from allowed list) -->
        <disallowedCategories>
          <li>ApparelUtility</li>
          <li>Weapon</li>
        </disallowedCategories>
      </filter>
    </defaultStorageSettings>
  </building>

  <surfaceType>Item</surfaceType>
  <canOverlapZones>false</canOverlapZones>
  <comps>
    <li Class="CompProperties_Styleable" />
  </comps>
  <inspectorTabs>
    <li>ITab_Storage</li>
    <li>ITab_ContentsOutfitStand</li>
  </inspectorTabs>
  <uiOrder>2050</uiOrder>
</ThingDef>
```

### OutfitStand Implementation

```xml
<ThingDef ParentName="OutfitStandBase">
  <defName>Building_OutfitStand</defName>
  <label>outfit stand</label>
  <description>A small display that showcases a single outfit. Saves space and can be used to quickly change clothing.</description>
  <thingClass>Building_OutfitStand</thingClass>
  <costStuffCount>35</costStuffCount>
  <graphicData>
    <graphicClass>Graphic_Multi</graphicClass>
    <texPath>Things/Building/OutfitStand/OutfitStand_MenuIcon</texPath>
    <drawSize>(1.3, 1.3)</drawSize>
  </graphicData>

  <building>
    <!-- Groups this building with other outfit stands for UI convenience -->
    <storageGroupTag>OutfitStand</storageGroupTag>
    
    <!-- Override parent's fixedStorageSettings with special filters -->
    <fixedStorageSettings>
      <filter>
        <!-- Special RimWorld filter: prevents child-only apparel -->
        <specialFiltersToDisallow>
          <li>AllowChildOnlyApparel</li>
        </specialFiltersToDisallow>
      </filter>
    </fixedStorageSettings>
  </building>
</ThingDef>
```

### Key Vanilla Settings Explained

| Setting | Purpose |
|---------|---------|
| **fixedStorageSettings** | Locked filter - players CANNOT change |
| **defaultStorageSettings** | Base settings - players CAN modify |
| **disallowNotEverStorable** | Prevents storing invalid items (corpses, etc) |
| **categories** | List of allowed ThingCategory or ThingCategoryDef |
| **disallowedCategories** | Blocks specific subcategories from allowed list |
| **storageGroupTag** | Groups multiple buildings in storage UI |
| **ignoreStoredThingsBeauty** | If true, items don't affect room beauty |
| **Inherit="False"** | Override parent categories completely |
| **specialFiltersToDisallow** | Special filters (ApparelUtility, ChildOnlyApparel, etc) |
| **priority** | Default storage priority (Important, Normal, Low) |

### ShipCargoTerminal Applied Settings

Your mod implements `fixedStorageSettings` with `disallowNotEverStorable="true"` and allowed categories - this matches the vanilla approach of a locked, player-proof storage filter.

---

## References
- **Adaptive Storage Framework**: https://github.com/bbradson/Adaptive-Storage-Framework
- **LWM's Deep Storage**: Industry standard for advanced storage
- **RimWorld Core Code**: StorageSettings, Building_Storage classes
- **Your Project**: SaveOurShipTranslation (ShipStorageTerminal implementation)

