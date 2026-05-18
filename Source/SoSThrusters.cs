using RimWorld; // PowerTrader, CompPowerTrader
using SaveOurShip2; // CompEngineTrail, CompProps_EngineTrail
using UnityEngine; // Vector, Mathf, Color
using Verse; // Graphic, GraphicDatabase, ShaderDatabase

namespace SaveOurCat
{
    public class CompProps_NuclearThrusterSmall : CompProps_EngineTrail
    {
        public CompProps_NuclearThrusterSmall() // COMP NUCLEAR
        {
            compClass = typeof(Comp_NuclearThrusterSmall);
        }
    }

    public class CompProps_IonEngineTrail : CompProps_EngineTrail
    {
        public float powerConsumptionPerThrust = 200f;

        public CompProps_IonEngineTrail() // COMP ION/ENERGY
        {
            compClass = typeof(Comp_IonEngineTrailEnergy);
        }
    }

    public class Comp_IonEngineTrailEnergy : CompEngineTrail
    {
        private Graphic trailGraphicIon;
        private CompPowerTrader powerTrader;
        private float cachedDrawHeight = 15.5f;
        private int lastUpdateTick = -10;

        private static readonly Vector3[] Offsets =
        {
            new Vector3(0, 0, -5.5f),
            new Vector3(-5.5f, 0, 0),
            new Vector3(0, 0, 5.5f),
            new Vector3(5.5f, 0, 0)
        };

        public CompProps_IonEngineTrail IonProps
        {
            get { return props as CompProps_IonEngineTrail; }
        }

        private Graphic TrailGraphicIon
        {
            get
            {
                if (trailGraphicIon == null)
                {
                    trailGraphicIon = GraphicDatabase.Get(
                        typeof(Graphic_Multi),
                        "Things/Building/Ship/Ship_Engine_Trail_Ion",
                        ShaderDatabase.MoteGlow,
                        new Vector2(7, 16.5f),
                        Color.white,
                        Color.white);
                }
                return trailGraphicIon;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerTrader = parent.TryGetComp<CompPowerTrader>();
        }

        public override void PostDraw()
        {
            if (active && IonProps != null && IonProps.energy)
            {
                // Обновляется вычисление каждый 3 кадр.тик вместо 1 кадра.тика. Повышает производительность в 3 раза, визуально упрощение скрыто более медленным движением.
                int currentTick = Find.TickManager.TicksGame;
                if (currentTick - lastUpdateTick >= 4)
                {
                    cachedDrawHeight = 15.5f;
                    cachedDrawHeight += 0.4f * Mathf.Cos(currentTick / 8f);       // Косинус лево-право анимация ускорителя

                    lastUpdateTick = currentTick;
                }

                TrailGraphicIon.drawSize = new Vector2(7, cachedDrawHeight);

                var offset = Offsets[parent.Rotation.AsInt];
                TrailGraphicIon.Draw(
                    new Vector3(
                        parent.DrawPos.x + offset.x,
                        parent.DrawPos.y + 1f,
                        parent.DrawPos.z + offset.z),
                    parent.Rotation,
                    parent);
                return;
            }

            base.PostDraw();
        }

        public override void CompTick()
        {
            base.CompTick();

            if (!active || powerTrader == null || IonProps == null) return;

            float thrust = IonProps.preciseThrust != 0
                ? IonProps.preciseThrust
                : IonProps.thrust;

            powerTrader.PowerOutput =
                -IonProps.powerConsumptionPerThrust * thrust;
        }
    }

    [StaticConstructorOnStartup]
    public class Comp_NuclearThrusterSmall : CompEngineTrail
    {
        private static Graphic trailGraphicNuclear = GraphicDatabase.Get(
            typeof(Graphic_Multi),
            "Things/Building/Ship/NuclearEngineTrailSmall",
            ShaderDatabase.MoteGlow,
            new Vector2(7, 26.5f),
            Color.white,
            Color.white);

        private static Vector3[] offsetL = { 
            new Vector3(0, 0, -11f), 
            new Vector3(-11f, 0, 0), 
            new Vector3(0, 0, 11f), 
            new Vector3(11f, 0, 0) 
        };

        public override void PostDraw()
        {
            if (active && !Props.reactionless && !Props.energy)
            {
                Vector2 drawSize = new Vector2(7, 26.5f + 0.5f * Mathf.Cos(Find.TickManager.TicksGame / 4));
                trailGraphicNuclear.drawSize = drawSize;
                trailGraphicNuclear.Draw(
                    new Vector3(
                        parent.DrawPos.x + offsetL[parent.Rotation.AsInt].x,
                        parent.DrawPos.y + 1f,
                        parent.DrawPos.z + offsetL[parent.Rotation.AsInt].z),
                    parent.Rotation,
                    parent);
                return;
            }

            base.PostDraw();
        }
    }
}
