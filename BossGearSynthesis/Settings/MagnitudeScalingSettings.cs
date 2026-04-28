using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public enum BossTier
{
    Mook = 1,
    Lesser = 2,
    Standard = 3,
    Elite = 4,
    Apex = 5,
    Unique = 99,
}

public class TierMagnitude
{
    [SynthesisSettingName("Tier")]
    public BossTier Tier;

    [SynthesisSettingName("Base magnitude")]
    public float Base;

    [SynthesisSettingName("Per-level coefficient")]
    public float LevelCoef;

    [SynthesisSettingName("Floor")]
    public float Floor;

    [SynthesisSettingName("Ceiling")]
    public float Ceiling;
}

public enum TierMatcherKind
{
    Faction,
    Race,
    Keyword,
}

public class TierClassificationRule
{
    [SynthesisSettingName("Matcher")]
    public TierMatcherKind Matcher;

    [SynthesisSettingName("Faction (if Matcher=Faction)")]
    public FormLink<IFactionGetter> Faction = new();

    [SynthesisSettingName("Race (if Matcher=Race)")]
    public FormLink<IRaceGetter> Race = new();

    [SynthesisSettingName("Keyword (if Matcher=Keyword)")]
    public FormLink<IKeywordGetter> Keyword = new();

    [SynthesisSettingName("Min level (0 = no min)")]
    public int MinLevel;

    [SynthesisSettingName("Max level (0 = no max)")]
    public int MaxLevel;

    [SynthesisSettingName("Tier")]
    public BossTier Tier;
}

public class MagnitudeScalingSettings
{
    [SynthesisSettingName("Per-tier magnitudes")]
    public List<TierMagnitude> Tiers = new()
    {
        new() { Tier = BossTier.Mook,     Base = 8,  LevelCoef = 0.20f, Floor = 6,  Ceiling = 14 },
        new() { Tier = BossTier.Lesser,   Base = 12, LevelCoef = 0.20f, Floor = 10, Ceiling = 18 },
        new() { Tier = BossTier.Standard, Base = 16, LevelCoef = 0.20f, Floor = 14, Ceiling = 22 },
        new() { Tier = BossTier.Elite,    Base = 20, LevelCoef = 0.30f, Floor = 18, Ceiling = 28 },
        new() { Tier = BossTier.Apex,     Base = 24, LevelCoef = 0.30f, Floor = 22, Ceiling = 32 },
    };

    [SynthesisSettingName("Tier classification rules")]
    [SynthesisTooltip("Walked top-to-bottom; first match wins.")]
    public List<TierClassificationRule> ClassificationRules = new();

    [SynthesisSettingName("Default tier (no rule matched)")]
    public BossTier DefaultTier = BossTier.Standard;

    [SynthesisSettingName("Named bump")]
    [SynthesisTooltip("Magnitude bump for non-allowlisted unique-named bosses.")]
    public float NamedBump = 2f;

    [SynthesisSettingName("Unique flat magnitude")]
    [SynthesisTooltip("Flat magnitude for bosses in the named-unique allowlist.")]
    public float UniqueFlatMagnitude = 40f;

    [SynthesisSettingName("Secondary effect scale")]
    [SynthesisTooltip("Multiplier applied to Effect 2 magnitude (smaller secondary feel).")]
    public float SecondaryEffectScale = 0.85f;

    [SynthesisSettingName("Health attribute multiplier")]
    public float HealthMultiplier = 2.0f;

    [SynthesisSettingName("Magicka attribute multiplier")]
    public float MagickaMultiplier = 2.0f;

    [SynthesisSettingName("Stamina attribute multiplier")]
    public float StaminaMultiplier = 2.0f;
}
