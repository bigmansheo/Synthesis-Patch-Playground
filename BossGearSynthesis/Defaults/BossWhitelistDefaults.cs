using Mutagen.Bethesda.Plugins;
using FK = Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Defaults for the curated boss whitelist: generic encounter templates by EditorID prefix,
/// race FormKeys for slot-less bosses, and base jewelry items used as ring/amulet fallbacks.
///
/// Race / armor / mod-key FormKeys are sourced from Mutagen.Bethesda.FormKeys.SkyrimSE so
/// they are verified at compile time. The previous version of this file had hand-typed
/// FormIDs that did not resolve to any record (e.g. an old DragonRace 0x013746 — actual
/// 0x012E82 — and an old GoldRing 0x000877 — actual 0x01CF2B), which silently broke
/// race-based and jewelry-fallback matching.
///
/// All defaults here are load-order independent:
/// - EditorID prefixes are case-insensitive string matches.
/// - FormKeys are anchored to a plugin filename, so they resolve to the same
///   record regardless of where the plugin sits in the load order.
/// - Trusted mod keys are filename matches.
/// </summary>
public static class BossWhitelistDefaults
{
    /// <summary>
    /// Case-insensitive EditorID prefixes for vanilla generic encounter bosses on the curated list.
    /// Covers Draugr Overlord/Wight Lord/Scourge Lord/Deathlord/Death Overlord, Bandit Chief,
    /// Master/Volkihar/Nightmaster Vampires, Forsworn Briarheart, Hagraven, Reaver Lord,
    /// Master/Arch Necromancer/Conjurer/Pyromancer/Electromancer, Falmer Shadowmaster/Warmonger,
    /// Spriggan Matron, Dwarven Centurion, Dragon Priest. Dragons are handled by race instead.
    /// </summary>
    public static readonly IReadOnlyList<string> EditorIdPrefixes = new List<string>
    {
        // Draugr boss tiers
        "EncDraugrOverlord",
        "EncDraugrWightLord",
        "EncDraugrScourgeLord",
        "EncDraugrDeathlord",
        "EncDraugrDeathOverlord",

        // Dragon Priests (generic - named ones are in NamedUniqueAllowlist)
        "EncDragonPriest",

        // Bandits
        "EncBanditChief",

        // Vampires
        "EncMasterVampire",
        "EncVampireMaster",
        "EncVolkiharMaster",
        "EncNightmasterVampire",

        // Forsworn
        "EncForswornBriarheart",
        "EncBriarheart",

        // Hagravens
        "EncHagraven",

        // Reavers (Dragonborn DLC)
        "EncReaverLord",
        "EncReaverBoss",

        // Mages
        "EncWarlockMaster",
        "EncWarlockArch",
        "EncNecromancerMaster",
        "EncNecromancerArch",
        "EncConjurerMaster",
        "EncConjurerArch",
        "EncPyromancerMaster",
        "EncPyromancerArch",
        "EncElectromancerArch",
        "EncCryomancerArch",

        // Falmer
        "EncFalmerShadowmaster",
        "EncFalmerWarmonger",

        // Spriggans
        "EncSprigganMatron",

        // Dwemer
        "EncDwarvenCenturion",
        "DwarvenCenturion",

        // Werewolves and other beast bosses
        "EncWerewolfBoss",
    };

    /// <summary>
    /// Race FormKeys for slot-less bosses that should still get a fallback ring/amulet.
    /// Sourced from Mutagen.Bethesda.FormKeys.SkyrimSE so the FormIDs are guaranteed correct.
    /// </summary>
    public static readonly IReadOnlyList<FormKey> Races = new List<FormKey>
    {
        FK.Skyrim.Race.DragonRace.FormKey,
        FK.Skyrim.Race.DragonPriestRace.FormKey,
        FK.Skyrim.Race.DwarvenCenturionRace.FormKey,
        FK.Skyrim.Race.HagravenRace.FormKey,
        FK.Skyrim.Race.SprigganRace.FormKey,
    };

    /// <summary>Vanilla JewelryRingGold (the basic gold ring) — default base for ring fallbacks.</summary>
    public static readonly FormKey GoldRing = FK.Skyrim.Armor.JewelryRingGold.FormKey;

    /// <summary>Vanilla JewelryNecklaceGold (the basic gold necklace) — default base for amulet fallbacks.</summary>
    public static readonly FormKey GoldNecklace = FK.Skyrim.Armor.JewelryNecklaceGold.FormKey;

    /// <summary>
    /// Plugin filenames that the unique-flag boss heuristic is allowed to fire on
    /// when the default whitelist is enabled. Anything else (mod-added named NPCs)
    /// is ignored unless explicitly added to one of the allowlists.
    /// </summary>
    public static readonly IReadOnlyList<string> TrustedUniqueModKeys = new List<string>
    {
        "Skyrim.esm",
        "Update.esm",
        "Dawnguard.esm",
        "HearthFires.esm",
        "Dragonborn.esm",
        "ccBGSSSE001-Fish.esm",
        "ccBGSSSE025-AdvDSGS.esm",
        "ccBGSSSE037-Curios.esl",
        "ccQDRSSE001-SurvivalMode.esl",
    };
}
