using RimWorld;
using SaveOurShip2;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SaveOurCat
{
    public class CompProps_IonEngineTrail : CompProps_EngineTrail
    {
        public float powerConsumptionPerThrust = 200f;

        public CompProps_IonEngineTrail()
        {
            compClass = typeof(Comp_IonEngineTrailEnergy);
        }
    }

    public class Comp_IonEngineTrailEnergy : CompEngineTrail
    {
        private Graphic trailGraphicIon;

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
                    trailGraphicIon = GraphicDatabase.Get(typeof(Graphic_Multi), "Things/Building/Ship/Ship_Engine_Trail_Ion", ShaderDatabase.MoteGlow, new Vector2(7, 16.5f), Color.white, Color.white);
                }
                return trailGraphicIon;
            }
        }

        public override void PostDraw()
        {
            if (active && IonProps != null && IonProps.energy)
            {
                Vector3[] offset = { new Vector3(0, 0, -5.5f), new Vector3(-5.5f, 0, 0), new Vector3(0, 0, 5.5f), new Vector3(5.5f, 0, 0) };
                TrailGraphicIon.drawSize = new Vector2(7, 15.5f + 0.5f * Mathf.Cos(Find.TickManager.TicksGame / 4));
                TrailGraphicIon.Draw(new Vector3(parent.DrawPos.x + offset[parent.Rotation.AsInt].x, parent.DrawPos.y + 1f, parent.DrawPos.z + offset[parent.Rotation.AsInt].z), parent.Rotation, parent);
                return;
            }

            base.PostDraw();
        }

        public override void CompTick()
        {
            base.CompTick();
            var powerTrader = parent.TryGetComp<CompPowerTrader>();
            if (powerTrader == null || IonProps == null)
            {
                return;
            }

            if (active)
            {
                float thrust = IonProps.preciseThrust != 0 ? IonProps.preciseThrust : IonProps.thrust;
                powerTrader.PowerOutput = -IonProps.powerConsumptionPerThrust * thrust;
            }
        }
    }

}
