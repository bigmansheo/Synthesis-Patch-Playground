using Mutagen.Bethesda.Plugins;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Curated list of named vanilla bosses drawn from the
/// "Bosses (Skyrim)" reference page on the Elder Scrolls wiki.
/// When the curated-only setting is on, ONLY these NPCs (plus any
/// the user adds via the explicit allowlist) are eligible for patching.
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
        K(Skyrim, 0x07C01B), // Konahrik

        // ---- Dragonborn DLC dragon priests ----
        K(Dragonborn, 0x02402F), // Ahzidal
        K(Dragonborn, 0x024030), // Dukaan
        K(Dragonborn, 0x024031), // Zahkriisos
        K(Dragonborn, 0x01E2DF), // Miraak

        // ---- Named dragons (Skyrim.esm) ----
        K(Skyrim, 0x000F811C), // Alduin
        K(Skyrim, 0x00043E97), // Paarthurnax
        K(Skyrim, 0x00076D17), // Odahviing
        K(Skyrim, 0x00079920), // Mirmulnir
        K(Skyrim, 0x000B53FB), // Sahloknir
        K(Skyrim, 0x000FE825), // Vuljotnaak
        K(Skyrim, 0x000FE82A), // Nahagliiv
        K(Skyrim, 0x000FE828), // Viinturuth
        K(Skyrim, 0x000FE829), // Vulthuryol
        K(Skyrim, 0x000FE827), // Naaslaarum
        K(Skyrim, 0x000FE826), // Voslaarum
        K(Skyrim, 0x000FE82B), // Krosulhah
        K(Skyrim, 0x000FE82C), // Numinex (display)
        // ---- Dragonborn DLC dragons ----
        K(Dragonborn, 0x01D466), // Sahrotaar
        K(Dragonborn, 0x01D467), // Kruziikrel
        K(Dragonborn, 0x01D468), // Relonikiv
        K(Dragonborn, 0x01CD81), // Karstaag
        // ---- Dawnguard dragon ----
        K(Dawnguard, 0x002CA17), // Durnehviir

        // ---- Dawnguard vampires ----
        K(Dawnguard, 0x0027B0), // Harkon
        K(Dawnguard, 0x002B74), // Vingalmo
        K(Dawnguard, 0x002B75), // Orthjolf
        K(Dawnguard, 0x015D54), // Lord Vamp / Volkihar named (placeholder)

        // ---- Skyrim.esm major named bosses ----
        K(Skyrim, 0x0240B7), // Potema (Wolf Queen)
        K(Skyrim, 0x0238D9), // Madanach (Forsworn King)
        K(Skyrim, 0x01B07A), // Mercer Frey
        K(Skyrim, 0x01C3A6), // Sinding (werewolf)
        K(Skyrim, 0x064FFD), // Movarth Piquine
        K(Skyrim, 0x04D246), // Arondil
        K(Skyrim, 0x06D4E5), // Lu'ah Al-Skaven
        K(Skyrim, 0x09FE61), // Sild the Warlock
        K(Skyrim, 0x0AA0A1), // Malyn Varen (ghost)
        K(Skyrim, 0x0E4A72), // Krev the Skinner
        K(Skyrim, 0x05F84C), // Curalmil
        K(Skyrim, 0x0240D9), // King Olaf One-Eye (ghost)
        K(Skyrim, 0x0966FF), // Hajvarr Iron-Hand
        K(Skyrim, 0x0AA01D), // Petra (hagraven)
        K(Skyrim, 0x09B2BD), // Drascua (hagraven)
        K(Skyrim, 0x05DBA7), // Melka (hagraven)
        K(Skyrim, 0x036620), // Moira (hagraven)
        K(Skyrim, 0x05A49F), // Captain Hargar (ghost)
        K(Skyrim, 0x09BAC0), // Lord Geirmund
        K(Skyrim, 0x0EE5C2), // Salma (necromancer, optional)
    };

    /// <summary>Ahzidal's NPC FormKey, used by the Ahzidal-special pathway.</summary>
    public static readonly FormKey Ahzidal = K(Dragonborn, 0x02402F);

    /// <summary>Vanilla Fortify Enchanting MGEF (used as Ahzidal's extra-effect default).</summary>
    public static readonly FormKey FortifyEnchantingMgef = K(Skyrim, 0x107A4D);

    // ---- Default fallback jewelry bases for slot-less enemies (e.g. dragons) ----
    /// <summary>Vanilla Gold Ring (Skyrim.esm).</summary>
    public static readonly FormKey GoldRing = K(Skyrim, 0x0877D9);
    /// <summary>Vanilla Gold Necklace (Skyrim.esm).</summary>
    public static readonly FormKey GoldNecklace = K(Skyrim, 0x08F0EC);
}
