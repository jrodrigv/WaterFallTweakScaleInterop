
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waterfall;


namespace WaterFallTweakScaleInterop
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Interop : MonoBehaviour
    {
        List<Vessel> toBeScaled = new List<Vessel>();
        bool init = false;
        private void Start()
        {
            GameEvents.onVesselLoaded.Add(RescaleWaterfall);
            GameEvents.onVesselCreate.Add(RescaleWaterfall);
            Debug.Log("WaterFallTweakScaleInterop: Ready!");
        }


        protected void LateUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;

            if (!init)
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    RescaleWaterfall(v);
                }
            }
        }

        private void RescaleWaterfall(Vessel data)
        {
          
            var engineParts = data.parts.Where(x => x.Modules.Contains<ModuleEngines>());

            foreach (var enginePart in engineParts)   
            {
   
                if (enginePart.Modules.GetModules<TweakScale.TweakScale>()[0] is not { } tweakScaleModule)
                {

                    continue;
                }
                
                float scaleMultiplier = tweakScaleModule.currentScale / tweakScaleModule.defaultScale;

                foreach (var moduleWaterfallFx in (enginePart.Modules.GetModules<ModuleWaterfallFX>()))
                {
                
                    foreach (var template in moduleWaterfallFx.Templates)
                    {
                   
                        template.scale *= scaleMultiplier;

                        foreach (var waterfallEffect in template.allFX)
                        {
                            waterfallEffect.ApplyTemplateOffsets(template.position, template.rotation, template.scale);
                            init = true;
                        }
                    }
                }

            }
        }

        private void OnDestroy()
        {
            GameEvents.onVesselLoaded.Remove(RescaleWaterfall);
            GameEvents.onVesselCreate.Remove(RescaleWaterfall);
        }
    }
}
