public enum GameMode
{
    PvP,
    PvCPU
}

public static class MatchConfig
{
    public static GameMode gameMode;

    public static CharacterData player1Character;
    public static CharacterData player2Character;

    public static StageData stage;

    // Later useful, not required yet
    public static bool player1OnLeft = true;
}
