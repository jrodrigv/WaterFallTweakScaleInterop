
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waterfall;


namespace WaterFallTweakScaleInterop
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Interop : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.onVesselLoaded.Add(RescaleWaterfall);
            GameEvents.onVesselCreate.Add(RescaleWaterfall);

            foreach (Vessel v in FlightGlobals.Vessels)
            {
                RescaleWaterfall(v);
            }
            Debug.Log("WaterFallTweakScaleInterop: Ready!");
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
                

                foreach (var moduleWaterfallFx in (enginePart.Modules.GetModules<ModuleWaterfallFX>()))
                {
                    moduleWaterfallFx.useRelativeScaling = true;
                
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
