using Mutagen.Bethesda.Plugins;

namespace BossGearSynthesis.Defaults;

/// <summary>Vanilla + AE/CC named-unique bosses that always get the unique-flat magnitude.</summary>
public static class NamedUniqueAllowlist
{
    private static readonly ModKey Skyrim = ModKey.FromNameAndExtension("Skyrim.esm");
    private static readonly ModKey Dawnguard = ModKey.FromNameAndExtension("Dawnguard.esm");
    private static readonly ModKey Dragonborn = ModKey.FromNameAndExtension("Dragonborn.esm");

    private static FormKey K(ModKey m, uint id) => new(m, id);

    public static readonly IReadOnlySet<FormKey> FormKeys = new HashSet<FormKey>
    {
        // Dragon Priests (Skyrim.esm)
        K(Skyrim, 0x07C014), // Krosis
        K(Skyrim, 0x089523), // Morokei
        K(Skyrim, 0x07C018), // Otar
        K(Skyrim, 0x07C016), // Rahgot
        K(Skyrim, 0x07C015), // Vokun
        K(Skyrim, 0x07C019), // Volsung
        K(Skyrim, 0x07C01A), // Nahkriin
        K(Skyrim, 0x07C017), // Hevnoraak
        K(Skyrim, 0x07C01B), // Konahrik (placeholder; ID may vary)

        // Dragonborn DLC dragon priests
        K(Dragonborn, 0x02402F), // Ahzidal
        K(Dragonborn, 0x024030), // Dukaan
        K(Dragonborn, 0x024031), // Zahkriisos
        K(Dragonborn, 0x01E2DF), // Miraak

        // Dawnguard
        K(Dawnguard, 0x0027B0), // Harkon
        K(Dawnguard, 0x002B74), // Vingalmo
        K(Dawnguard, 0x002B75), // Orthjolf

        // Misc named
        K(Dragonborn, 0x01CD81), // Karstaag
        K(Skyrim, 0x0240B7),     // Potema (queen of wolves)
        K(Skyrim, 0x0238D9),     // Madanach (forsworn king)
    };

    /// <summary>Ahzidal's NPC FormKey, used by the Ahzidal-special pathway.</summary>
    public static readonly FormKey Ahzidal = K(Dragonborn, 0x02402F);

    /// <summary>Vanilla Fortify Enchanting MGEF (used as Ahzidal's extra-effect default).</summary>
    public static readonly FormKey FortifyEnchantingMgef = K(Skyrim, 0x107A4D);
}
