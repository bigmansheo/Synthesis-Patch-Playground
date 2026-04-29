using Mutagen.Bethesda.Plugins;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Curated list of vanilla + DLC named-unique bosses, modeled after
/// https://elderscrolls.fandom.com/wiki/Bosses_(Skyrim) .
/// In strict mode this is the canonical set the patcher will touch.
/// Users can extend the list at runtime via the Synthesis settings UI.
/// </summary>
public static class NamedUniqueAllowlist
{
    private static readonly ModKey Skyrim = ModKey.FromNameAndExtension("Skyrim.esm");
    private static readonly ModKey Dawnguard = ModKey.FromNameAndExtension("Dawnguard.esm");
    private static readonly ModKey Dragonborn = ModKey.FromNameAndExtension("Dragonborn.esm");

    private static FormKey K(ModKey m, uint id) => new(m, id);

    public static readonly IReadOnlySet<FormKey> FormKeys = new HashSet<FormKey>
    {
        // ---- Dragon Priests (Skyrim.esm) ----
        K(Skyrim, 0x07C014), // Krosis
        K(Skyrim, 0x089523), // Morokei
        K(Skyrim, 0x07C018), // Otar
        K(Skyrim, 0x07C016), // Rahgot
        K(Skyrim, 0x07C015), // Vokun
        K(Skyrim, 0x07C019), // Volsung
        K(Skyrim, 0x07C01A), // Nahkriin
        K(Skyrim, 0x07C017), // Hevnoraak
        K(Skyrim, 0x07C01B), // Konahrik (placeholder; ID may vary)

        // ---- Dragonborn DLC dragon priests ----
        K(Dragonborn, 0x02402F), // Ahzidal
        K(Dragonborn, 0x024030), // Dukaan
        K(Dragonborn, 0x024031), // Zahkriisos
        K(Dragonborn, 0x01E2DF), // Miraak

        // ---- Dawnguard vampires ----
        K(Dawnguard, 0x0027B0), // Harkon
        K(Dawnguard, 0x002B74), // Vingalmo
        K(Dawnguard, 0x002B75), // Orthjolf

        // ---- Named dragons (have no body/biped slot - jewelry fallback) ----
        K(Skyrim, 0x027CA1),     // Mirmulnir (Western Watchtower)
        K(Skyrim, 0x0F811C),     // Sahloknir (Kynesgrove)
        K(Skyrim, 0x0465A5),     // Odahviing
        K(Skyrim, 0x01A6B7),     // Paarthurnax
        K(Skyrim, 0x0477EE),     // Alduin
        K(Skyrim, 0x0F811A),     // Vulthuryol (Blackreach)
        K(Dawnguard, 0x016649),  // Durnehviir

        // ---- Other notable named bosses ----
        K(Dragonborn, 0x01CD81), // Karstaag
        K(Skyrim, 0x0240B7),     // Potema (Wolf Queen)
        K(Skyrim, 0x0238D9),     // Madanach (Forsworn King)
    };

    /// <summary>Ahzidal's NPC FormKey, used by the Ahzidal-special pathway.</summary>
    public static readonly FormKey Ahzidal = K(Dragonborn, 0x02402F);

    /// <summary>Vanilla Fortify Enchanting MGEF (used as Ahzidal's extra-effect default).</summary>
    public static readonly FormKey FortifyEnchantingMgef = K(Skyrim, 0x107A4D);

    // ---- Default base items for the jewelry fallback (dragons etc.) ----
    /// <summary>Vanilla Silver Ring - default base for slot-less ring fallback.</summary>
    public static readonly FormKey FallbackRing = K(Skyrim, 0x01CF1B);

    /// <summary>Vanilla Gold Necklace - default base for slot-less amulet fallback.</summary>
    public static readonly FormKey FallbackAmulet = K(Skyrim, 0x10089D);
}
