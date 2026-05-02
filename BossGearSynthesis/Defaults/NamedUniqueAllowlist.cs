using Mutagen.Bethesda.Plugins;
using FK = Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Vanilla + AE/CC named-unique bosses that always get the unique-flat magnitude.
///
/// Every entry below resolves to a real NPC record because the symbol comes from
/// Mutagen.Bethesda.FormKeys.SkyrimSE — the package is auto-generated from the
/// shipped Skyrim/DLC plugins, so a typo would not compile. (The previous version
/// of this file used hand-typed FormIDs that did not resolve to anything.)
///
/// A handful of vanilla bosses are missing from the FormKeys package and are
/// listed under "// TODO: not in FormKeys package" — adding them as raw FormKeys
/// without external verification would silently re-introduce the dangling-link
/// problem this rewrite was meant to fix.
/// </summary>
public static class NamedUniqueAllowlist
{
    public static readonly IReadOnlySet<FormKey> FormKeys = new HashSet<FormKey>
    {
        // === Skyrim.esm dragon priests ===
        FK.Skyrim.Npc.dunShearpointKrosisDragonPriest.FormKey,    // Krosis      (Shearpoint)
        FK.Skyrim.Npc.MG07LabyrinthianDragonPriest.FormKey,        // Morokei     (Labyrinthian)
        FK.Skyrim.Npc.dunRagnOtar.FormKey,                         // Otar the Mad (Ragnvald)
        FK.Skyrim.Npc.DunForelhostDragonPriestRahgot.FormKey,      // Rahgot      (Forelhost)
        FK.Skyrim.Npc.dunVolskyggeDragonPriest01.FormKey,          // Volsung     (Volskygge)
        FK.Skyrim.Npc.dunSkuldafnNahkriin.FormKey,                 // Nahkriin    (Skuldafn)
        FK.Skyrim.Npc.dunValthumeHevnoraak.FormKey,                // Hevnoraak   (Valthume)
        // TODO: Vokun (HighGate Ruins) — not in FormKeys package.
        // Konahrik does not have an NPC record (mask drops from a leveled priest).

        // === Dragonborn DLC dragon priests + Karstaag + Miraak ===
        FK.Dragonborn.Npc.DLC2AcolyteAhzidal.FormKey,              // Ahzidal     (Kolbjorn / Miraak's Temple)
        FK.Dragonborn.Npc.DLC2AcolyteDukaan.FormKey,               // Dukaan      (White Ridge / Miraak's Temple)
        FK.Dragonborn.Npc.DLC2AcolyteZahkriisos.FormKey,           // Zahkriisos  (Bloodskal / Miraak's Temple)
        FK.Dragonborn.Npc.DLC2Miraak.FormKey,                       // Miraak
        FK.Dragonborn.Npc.DLC2dunKarstaag.FormKey,                  // Karstaag
        // TODO: Vahlok the Jailor (Vahlok's Tomb) — not in FormKeys package.

        // === Dawnguard.esm ===
        FK.Dawnguard.Npc.DLC1Harkon.FormKey,                       // Lord Harkon
        FK.Dawnguard.Npc.DLC1Vingalmo.FormKey,
        FK.Dawnguard.Npc.DLC1Orthjolf.FormKey,
        FK.Dawnguard.Npc.DLC1AlthadanVyrthur.FormKey,              // Arch-Curate Vyrthur (Darkfall Cave)

        // === Skyrim.esm misc named ===
        FK.Skyrim.Npc.MS06Potema.FormKey,                          // Potema
        FK.Skyrim.Npc.Madanach.FormKey,

        // === Vanilla quest bosses (extending coverage to the user's whitelist) ===

        // Bandit / Forsworn / Reaver
        FK.Skyrim.Npc.dunMistwatchFjola.FormKey,                   // Fjola              (Mistwatch)
        FK.Skyrim.Npc.Linwe.FormKey,                                // Linwe              (Uttering Hills Cave)
        FK.Skyrim.Npc.dunCragslaneButcher.FormKey,                 // Butcher            (Cragslane Cavern)
        FK.Skyrim.Npc.dunCrackedTuskGhunzul.FormKey,               // Ghunzul            (Cracked Tusk Keep)
        FK.Skyrim.Npc.dunBrokenOarHargar.FormKey,                  // Captain Hargar     (Broken Oar Grotto)
        FK.Skyrim.Npc.Telrav.FormKey,                              // Telrav             (Nilheim)
        FK.Skyrim.Npc.dunRebelsCairnLvlDraugrBossRedEagle.FormKey, // Red Eagle          (Rebel's Cairn)
        FK.Skyrim.Npc.dunHalldirsBoss.FormKey,                     // Halldir            (Halldir's Cairn)

        // Draugr / quest
        FK.Skyrim.Npc.dunAnsilvundLuahAlSkaven.FormKey,            // Lu'ah Al-Skaven    (Ansilvund)
        FK.Skyrim.Npc.dunFolgunthur_MikrulGauldurson.FormKey,      // Mikrul Gauldurson  (Folgunthur)
        FK.Skyrim.Npc.dunGeirmundSigdis.FormKey,                   // Sigdis Gauldurson  (Geirmund's Hall)
        FK.Skyrim.Npc.JyrikGauldurson.FormKey,                     // Jyrik Gauldurson   (Saarthal)
        FK.Skyrim.Npc.MQ305Olaf.FormKey,                           // Olaf One-Eye       (Dead Men's Respite — Sovngarde fight)
        FK.Skyrim.Npc.dunForsakenCaveCuralmil.FormKey,             // Curalmil           (Forsaken Cave)
        FK.Skyrim.Npc.DunVolunruudBoss.FormKey,                    // Kvenel the Tongue  (Volunruud)

        // Vampires / necromancers / mages / quest bosses
        FK.Skyrim.Npc.dunMovarthVampireBoss.FormKey,               // Movarth Piquine    (Movarth's Lair)
        FK.Skyrim.Npc.MercerFrey.FormKey,                          // Mercer Frey        (Irkngthand)
        FK.Skyrim.Npc.Ancano.FormKey,                              // Ancano             (College of Winterhold)
        FK.Skyrim.Npc.DA13Orchendor.FormKey,                       // Orchendor          (Bthardamz)
        FK.Skyrim.Npc.dunFrostmereCryptPaleLady.FormKey,           // The Pale Lady      (Frostmere Crypt)
        FK.Skyrim.Npc.Drascua.FormKey,                              // Drascua            (Dead Crone Rock)
        FK.Skyrim.Npc.dunDarklightSilvia.FormKey,                  // Silvia             (Darklight Tower)
        FK.Skyrim.Npc.MG03Caller.FormKey,                          // The Caller         (Fellglow Keep)
        FK.Skyrim.Npc.ValsVeran.FormKey,                           // Vals Veran         (Hillgrund's Tomb)
        FK.Skyrim.Npc.TitusMedeII.FormKey,                         // Emperor Titus Mede II (passive)
        FK.Skyrim.Npc.C06KodlaksGhost.FormKey,                     // Kodlak's Wolf Spirit (Ysgramor's Tomb)

        // Dragons (also covered by race; listed here for unique-flat magnitude)
        FK.Skyrim.Npc.dunLabyrinthianUndeadDragon.FormKey,         // Skeletal Dragon (Labyrinthian)
        FK.Skyrim.Npc.Odahviing.FormKey,
        FK.Skyrim.Npc.Paarthurnax.FormKey,
        FK.Skyrim.Npc.AlduinBase.FormKey,                          // Alduin

        // Dragonborn quest bosses
        FK.Dragonborn.Npc.DLC2EbonyWarrior.FormKey,                // Ebony Warrior      (Last Vigil)
        FK.Dragonborn.Npc.DLC2dunHaknir.FormKey,                   // Haknir Death-Brand (Gyldenhul Barrow)

        // === TODO: not in Mutagen.Bethesda.FormKeys.SkyrimSE 3.4.0 ===
        // The following vanilla quest bosses do not appear as NPC symbols in the
        // FormKeys package and need their FormIDs sourced from another canonical
        // reference (UESP, xEdit) before being added — guessing would silently
        // re-introduce dangling links.
        //
        //   Skyrim.esm:
        //     - Krev the Skinner       (Gallows Rock)
        //     - Yngol's Shade          (Yngol Barrow)
        //     - Vighar                 (Bloodlet Throne)
        //     - Champion of Boethiah   (Knifepoint Ridge)
        //     - Ritual Master          (Wolfskull Cave)
        //     - Northwatch Interrogator(Northwatch Keep)
        //     - Sebastian Lort         (Rimerock Burrow)
        //     - First Mate             (Dainty Sload)
        //     - Sild the Warlock       (Rannveig's Fast)
        //     - Kornalus               (Harmugstahl)
        //     - Petra                  (Blind Cliff Cave)
        //     - Drelas                 (Drelas' Cottage)
        //     - Hajvarr Iron-Hand      (White River Watch)
        //     - Warlord Gathrik        (Ironbind Barrow)
        //     - Mirmulnir              (Western Watchtower)
        //     - Rigel Strong-Arm       (Pinewatch)
        //     - Vokun                  (HighGate Ruins, dragon priest)
        //   Dawnguard.esm:
        //     - Minorne                (Ruunvald Excavation)
        //     - Venarus Vulpin         (Redwater Den)
        //   Dragonborn.esm:
        //     - Vahlok the Jailor      (Vahlok's Tomb, dragon priest)
    };

    /// <summary>Ahzidal's NPC FormKey, used by the Ahzidal-special pathway.</summary>
    public static readonly FormKey Ahzidal = FK.Dragonborn.Npc.DLC2AcolyteAhzidal.FormKey;

    /// <summary>
    /// Vanilla Fortify Enchanting MGEF used as Ahzidal's extra-effect default.
    /// EnchRobesFortifyEnchantingConstantSelf is the apparel constant-self variant
    /// (the same one used on master mage robes).
    /// </summary>
    public static readonly FormKey FortifyEnchantingMgef =
        FK.Skyrim.MagicEffect.EnchRobesFortifyEnchantingConstantSelf.FormKey;
}
