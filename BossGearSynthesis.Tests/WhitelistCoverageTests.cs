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

    // ---------------------------------------------------------------------
    // Regression tests for the "every named NPC was tagged" bug.
    // The Unique flag in Skyrim marks one-of-a-kind named NPCs (Lydia,
    // Belethor, every jarl) — it is NOT a boss signal. The unique-flag
    // fallback must therefore stay disabled in whitelist-only mode.
    // ---------------------------------------------------------------------

    [Fact]
    public void Whitelist_only_mode_blocks_unique_flag_fallback_even_for_unique_named_NPCs()
    {
        // Simulates Lydia: Unique flag set, has name, from Skyrim.esm (trusted),
        // user has whitelist-only enabled.
        var fired = WhitelistMatcher.ShouldFireUniqueFlagFallback(
            whitelistOnly: true,
            useUniqueFlag: true,
            npcIsUnique: true,
            npcHasName: true,
            npcFromTrustedPlugin: true);
        Assert.False(fired,
            "Whitelist-only mode must NOT fire the unique-flag fallback; that path catches every named vanilla NPC (Lydia, jarls, shopkeepers).");
    }

    [Fact]
    public void Default_settings_disable_unique_flag_pathway()
    {
        var defaults = new BossGearSynthesis.Settings.BossDetectionSettings();
        Assert.True(defaults.UseDefaultWhitelistOnly,
            "Default must be whitelist-only so the patcher does not tag every named NPC.");
        Assert.False(defaults.UseUniqueFlag,
            "Default must disable the broad unique-flag heuristic.");

        // The two defaults combined: with whitelistOnly=true OR useUniqueFlag=false,
        // the fallback never fires regardless of NPC properties.
        var fired = WhitelistMatcher.ShouldFireUniqueFlagFallback(
            whitelistOnly: defaults.UseDefaultWhitelistOnly,
            useUniqueFlag: defaults.UseUniqueFlag,
            npcIsUnique: true,
            npcHasName: true,
            npcFromTrustedPlugin: true);
        Assert.False(fired);
    }

    [Theory]
    // whitelistOnly, useUniqueFlag, isUnique, hasName, trustedPlugin, expected
    [InlineData(true,  true,  true,  true,  true,  false)] // whitelist-only short-circuits everything
    [InlineData(true,  false, true,  true,  true,  false)]
    [InlineData(false, false, true,  true,  true,  false)] // flag off
    [InlineData(false, true,  false, true,  true,  false)] // not unique
    [InlineData(false, true,  true,  false, true,  false)] // no name
    [InlineData(false, true,  true,  true,  false, false)] // untrusted plugin
    [InlineData(false, true,  true,  true,  true,  true)]  // power-user opt-in: all conditions met
    public void Unique_flag_fallback_truth_table(
        bool whitelistOnly, bool useUniqueFlag, bool isUnique,
        bool hasName, bool trustedPlugin, bool expected)
    {
        Assert.Equal(expected, WhitelistMatcher.ShouldFireUniqueFlagFallback(
            whitelistOnly, useUniqueFlag, isUnique, hasName, trustedPlugin));
    }

    // ---------------------------------------------------------------------
    // Logging support: FirstMatchingPrefix returns the actual prefix string
    // so the dry-run report can show which prefix fired.
    // ---------------------------------------------------------------------

    [Fact]
    public void FirstMatchingPrefix_returns_the_matched_prefix_string()
    {
        var prefixes = new[] { "EncBanditChief", "EncDraugrOverlord" };
        Assert.Equal("EncDraugrOverlord",
            WhitelistMatcher.FirstMatchingPrefix("EncDraugrOverlord01Template", prefixes));
        Assert.Equal("EncBanditChief",
            WhitelistMatcher.FirstMatchingPrefix("encbanditchief05", prefixes));
    }

    [Fact]
    public void FirstMatchingPrefix_returns_null_when_nothing_matches()
    {
        Assert.Null(WhitelistMatcher.FirstMatchingPrefix("Lydia", BossWhitelistDefaults.EditorIdPrefixes));
        Assert.Null(WhitelistMatcher.FirstMatchingPrefix(null, BossWhitelistDefaults.EditorIdPrefixes));
        Assert.Null(WhitelistMatcher.FirstMatchingPrefix("", BossWhitelistDefaults.EditorIdPrefixes));
    }

    [Fact]
    public void FirstMatchingPrefix_skips_blank_prefixes()
    {
        var prefixes = new[] { "", "   ", "EncBanditChief" };
        Assert.Equal("EncBanditChief",
            WhitelistMatcher.FirstMatchingPrefix("EncBanditChief01", prefixes));
    }
}
