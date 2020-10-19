using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

namespace ENBLightPatcher
{

    public class Program
    {
        public static int Main(string[] args)
        {
            return SynthesisPipeline.Instance.Patch<ISkyrimMod, ISkyrimModGetter>(
                args: args,
                patcher: RunPatch,
                new UserPreferences()
                {
                    ActionsForEmptyArgs = new RunDefaultPatcher()
                    {
                        IdentifyingModKey = "WaterDoesDamage.esp",
                        TargetRelease = GameRelease.SkyrimSE
                    }
                }
            );
        }

        public static void RunPatch(SynthesisState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach(var water in state.LoadOrder.PriorityOrder.WinningOverrides<Mutagen.Bethesda.Skyrim.IWaterGetter>())
            {
                if (water.Flags != null && !water.Flags.Value.HasFlag(Water.Flag.CausesDamage))
                {
                    var damageWater = state.PatchMod.Waters.GetOrAddAsOverride(water);
                    damageWater.Flags |= Mutagen.Bethesda.Skyrim.Water.Flag.CausesDamage;
                }
                else continue;
            }
        }
    }
}
