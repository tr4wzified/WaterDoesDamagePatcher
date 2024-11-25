using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace WaterDoesDamage;

public class Program
{
    public static Task<int> Main(string[] args)
    {
        return SynthesisPipeline.Instance
            .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
            .SetTypicalOpen(GameRelease.SkyrimSE, "WaterDoesDamagePatcher.esp")
            .Run(args);
    }

    public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
    {
        foreach(var water in state.LoadOrder.PriorityOrder.WinningOverrides<IWaterGetter>())
        {
            if (water.Flags != null && !water.Flags.Value.HasFlag(Water.Flag.CausesDamage))
            {
                var damageWater = state.PatchMod.Waters.GetOrAddAsOverride(water);
                damageWater.Flags |= Mutagen.Bethesda.Skyrim.Water.Flag.CausesDamage;
            }
        }
    }
}