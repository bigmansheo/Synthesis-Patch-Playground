using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.BossDetection;

/// <summary>Why a particular NPC was selected as a boss. Used for logging + debugging.</summary>
public enum MatchReason
{
    ExplicitAllow,
    NamedUniqueAllow,
    Race,
    Keyword,
    EditorIdPrefix,
    UniqueFlag,
}

/// <summary>
/// Result of <see cref="BossSelector.Select"/>: the NPC plus the matcher
/// that flagged it and a short detail string (e.g. the matched prefix or
/// race form key) so the dry-run report can show exactly what fired.
/// </summary>
public record BossMatch(INpcGetter Npc, MatchReason Reason, string Detail);
