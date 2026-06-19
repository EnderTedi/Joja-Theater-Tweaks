using HarmonyLib;
using JetBrains.Annotations;
using JojaTheaterTweaks.Util;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JojaTheaterTweaks;

[UsedImplicitly]
public class JojaTheaterTweaks : Mod
{
    public static LogUtil Log { get; private set; } = null!;

    public new static IModHelper Helper { get; private set; } = null!;

    private static Harmony Harmony { get; set; } = null!;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Entry(IModHelper helper)
    {
        Helper = helper;
        Log = new(Monitor);
        Harmony = new(ModManifest.UniqueID);

        //Helper.Events.Player.Warped += OnPlayerWarped;
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnPlayerWarped(object? sender, WarpedEventArgs e)
    {
        if (!HelperFuncs.IsJojaMartComplete())
            return;


        if (e.NewLocation.Name == "Town" && !Utility.doesMasterPlayerHaveMailReceivedButNotMailForTomorrow("ccMovieTheater"))
        {
            var town = Game1.RequireLocation<Town>("Town");
            town.showDestroyedJoja();
            town.crackOpenAbandonedJojaMartDoor();
        }
        else if (e.NewLocation.Name == "AbandonedJojaMart")
        {
            var mart = Game1.RequireLocation<AbandonedJojaMart>("AbandonedJojaMart");

            mart.characters.RemoveWhere(c => c is Junimo);
        }
    }

}