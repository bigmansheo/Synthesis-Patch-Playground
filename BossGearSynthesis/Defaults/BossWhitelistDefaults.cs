using Mutagen.Bethesda.Plugins;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Defaults for the curated boss whitelist: generic encounter templates by EditorID prefix,
/// race FormKeys for slot-less bosses, and base jewelry items used as ring/amulet fallbacks.
///
/// All defaults here are load-order independent:
/// - EditorID prefixes are case-insensitive string matches that work regardless of plugin position.
/// - FormKeys are anchored to a plugin filename (e.g. "Skyrim.esm"), so they resolve to the same
///   record in the user's load order regardless of where the plugin sits.
/// - Trusted mod keys are matched by filename, not by load-order index.
/// </summary>
public static class BossWhitelistDefaults
{
    private static readonly ModKey Skyrim = ModKey.FromNameAndExtension("Skyrim.esm");
    private static readonly ModKey Dawnguard = ModKey.FromNameAndExtension("Dawnguard.esm");
    private static readonly ModKey Dragonborn = ModKey.FromNameAndExtension("Dragonborn.esm");

    private static FormKey K(ModKey m, uint id) => new(m, id);

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
    /// Includes Dragon, DwarvenCenturion, Hagraven, and Spriggan races.
    /// </summary>
    public static readonly IReadOnlyList<FormKey> Races = new List<FormKey>
    {
        K(Skyrim, 0x013746), // DragonRace
        K(Skyrim, 0x0131F4), // DragonPriestRace
        K(Skyrim, 0x0131EE), // DwarvenCenturionRace
        K(Skyrim, 0x013204), // HagravenRace
        K(Skyrim, 0x0131F0), // SprigganRace
    };

    /// <summary>Vanilla GoldRing armor record - default base for ring fallbacks.</summary>
    public static readonly FormKey GoldRing = K(Skyrim, 0x000877);

    /// <summary>Vanilla GoldNecklace armor record - default base for amulet fallbacks.</summary>
    public static readonly FormKey GoldNecklace = K(Skyrim, 0x000824);

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
