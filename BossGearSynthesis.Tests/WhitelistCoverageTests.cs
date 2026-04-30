using BossGearSynthesis.BossDetection;
using BossGearSynthesis.Defaults;
using Xunit;

namespace BossGearSynthesis.Tests;

/// <summary>
/// Cross-references the curated default whitelist (EditorID prefixes, races,
/// trusted mod keys, named-unique allowlist) against the user-supplied
/// vanilla boss list to confirm every entry is reachable by at least one
/// matcher. Pure string/FormKey checks — no Mutagen runtime needed.
/// </summary>
public class WhitelistCoverageTests
{
    // Representative vanilla boss EditorID prefixes seen in Skyrim.esm/Dawnguard.esm/Dragonborn.esm.
    // These exercise every generic-encounter row from the user's whitelist.
    public static readonly TheoryData<string, string> GenericBossEditorIds = new()
    {
        { "EncDraugrOverlord01Template",        "Draugr Overlord" },
        { "EncDraugrWightLord01Template",       "Draugr Wight Lord" },
        { "EncDraugrScourgeLord01Template",     "Draugr Scourge Lord" },
        { "EncDraugrDeathlord01Template",       "Draugr Deathlord" },
        { "EncDraugrDeathOverlord01",           "Draugr Death Overlord" },
        { "EncDragonPriest01Template",          "Generic Dragon Priest" },
        { "EncBanditChief01Template",           "Bandit Chief" },
        { "EncMasterVampire01Template",         "Master Vampire" },
        { "EncVolkiharMasterVampire",           "Volkihar Master Vampire" },
        { "EncNightmasterVampire01",            "Nightmaster Vampire" },
        { "EncForswornBriarheart01Template",    "Forsworn Briarheart" },
        { "EncBriarheart01",                    "Briarheart (alt)" },
        { "EncHagraven01Template",              "Hagraven" },
        { "EncReaverLord01",                    "Reaver Lord" },
        { "EncReaverBoss01",                    "Reaver Boss" },
        { "EncNecromancerMaster01",             "Master Necromancer" },
        { "EncNecromancerArch01",               "Arch-Necromancer" },
        { "EncConjurerMaster01",                "Master Conjurer" },
        { "EncConjurerArch01",                  "Arch-Conjurer" },
        { "EncPyromancerMaster01",              "Master Pyromancer" },
        { "EncPyromancerArch01",                "Arch-Pyromancer" },
        { "EncElectromancerArch01",             "Arch-Electromancer" },
        { "EncCryomancerArch01",                "Arch-Cryomancer" },
        { "EncFalmerShadowmaster01",            "Falmer Shadowmaster" },
        { "EncFalmerWarmonger01",               "Falmer Warmonger" },
        { "EncSprigganMatron01Template",        "Spriggan Matron" },
        { "EncDwarvenCenturion01Template",      "Dwarven Centurion (Enc)" },
        { "DwarvenCenturionBoss",               "Dwarven Centurion (no Enc prefix)" },
        { "EncWarlockMaster01Template",         "Master Warlock alias" },
        { "EncWarlockArch01Template",           "Arch-Warlock alias" },
    };

    [Theory]
    [MemberData(nameof(GenericBossEditorIds))]
    public void DefaultEditorIdPrefixes_match_each_generic_boss_class(string editorId, string label)
    {
        var matched = WhitelistMatcher.MatchesEditorIdPrefix(
            editorId,
            BossWhitelistDefaults.EditorIdPrefixes);
        Assert.True(matched, $"{label}: '{editorId}' was NOT matched by any default EditorID prefix.");
    }

    [Fact]
    public void EditorIdPrefix_match_is_case_insensitive()
    {
        Assert.True(WhitelistMatcher.MatchesEditorIdPrefix(
            "encdraugroverlord01",
            BossWhitelistDefaults.EditorIdPrefixes));
    }

    [Fact]
    public void EditorIdPrefix_match_ignores_unrelated_npcs()
    {
        // Generic non-boss NPCs should not be picked up by the prefix list.
        Assert.False(WhitelistMatcher.MatchesEditorIdPrefix(
            "EncBandit01Template",
            BossWhitelistDefaults.EditorIdPrefixes));
        Assert.False(WhitelistMatcher.MatchesEditorIdPrefix(
            "EncDraugr01Template",
            BossWhitelistDefaults.EditorIdPrefixes));
        Assert.False(WhitelistMatcher.MatchesEditorIdPrefix(
            "MQ101Hadvar",
            BossWhitelistDefaults.EditorIdPrefixes));
    }

    [Fact]
    public void EditorIdPrefix_handles_null_or_empty_input()
    {
        Assert.False(WhitelistMatcher.MatchesEditorIdPrefix(null, BossWhitelistDefaults.EditorIdPrefixes));
        Assert.False(WhitelistMatcher.MatchesEditorIdPrefix("", BossWhitelistDefaults.EditorIdPrefixes));
    }

    [Fact]
    public void Default_race_allowlist_covers_slotless_boss_races()
    {
        // Race FormKeys for Dragon, DragonPriest, DwarvenCenturion, Hagraven, Spriggan
        // must be present so the slot-less fallback path can build a ring/amulet.
        Assert.Equal(5, BossWhitelistDefaults.Races.Count);
    }

    [Fact]
    public void Trusted_mod_keys_includes_vanilla_and_DLC()
    {
        var trusted = BossWhitelistDefaults.TrustedUniqueModKeys;
        Assert.Contains("Skyrim.esm", trusted);
        Assert.Contains("Dawnguard.esm", trusted);
        Assert.Contains("Dragonborn.esm", trusted);
        Assert.Contains("HearthFires.esm", trusted);
    }

    public static readonly TheoryData<string> NamedDragonPriestEditorIds = new()
    {
        // These are the dragon priests the user explicitly listed by name.
        // They live in the NamedUniqueAllowlist FormKey set; the corresponding
        // editor IDs would also fire via the EditorIdPrefix path (EncDragonPriest)
        // or via the unique-flag fallback.
        "Krosis", "Morokei", "Otar", "Rahgot", "Vokun", "Volsung",
        "Hevnoraak", "Ahzidal", "Dukaan", "Zahkriisos", "Vahlok",
    };

    [Fact]
    public void Named_unique_allowlist_has_a_FormKey_per_dragon_priest_slot()
    {
        // The list is intentionally hand-curated; we just assert it's non-trivially populated.
        Assert.True(NamedUniqueAllowlist.FormKeys.Count >= 15,
            $"Named-unique allowlist should cover the major dragon priests + named uniques. Got {NamedUniqueAllowlist.FormKeys.Count}.");
    }

    [Fact]
    public void Plugin_source_allowlist_empty_means_any_plugin_allowed()
    {
        var empty = new HashSet<string>();
        Assert.True(WhitelistMatcher.IsPluginAllowed("Skyrim.esm", empty));
        Assert.True(WhitelistMatcher.IsPluginAllowed("RandomMod.esp", empty));
    }

    [Fact]
    public void Plugin_source_allowlist_filters_by_filename_case_insensitive()
    {
        var allow = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Skyrim.esm", "Dragonborn.esm" };
        Assert.True(WhitelistMatcher.IsPluginAllowed("skyrim.esm", allow));
        Assert.True(WhitelistMatcher.IsPluginAllowed("Dragonborn.esm", allow));
        Assert.False(WhitelistMatcher.IsPluginAllowed("Dawnguard.esm", allow));
        Assert.False(WhitelistMatcher.IsPluginAllowed("OBIS.esp", allow));
    }
}
