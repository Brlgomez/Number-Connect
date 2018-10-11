public static class LeaderboardIDs
{
#if (NO_GPGS)
    public const string winsEasy = "wins_easy";
    public const string winsMedium = "wins_medium";
    public const string winsHard = "wins_hard";
    public const string winsExpert = "wins_expert";
    public const string winsEasyPlus = "wins_easy_plus";
    public const string winsMediumPlus = "wins_medium_plus";
    public const string winsHardPlus = "wins_hard_plus";
    public const string winsExpertPlus = "wins_expert_plus";
    public const string bestTimeEasy = "best_time_easy";
    public const string bestTimeMedium = "best_time_medium";
    public const string bestTimeHard = "best_time_hard";
    public const string bestTimeExpert = "best_time_expert";
    public const string bestTimeEasyPlus = "best_time_easy_plus";
    public const string bestTimeMediumPlus = "best_time_medium_plus";
    public const string bestTimeHardPlus = "best_time_hard_plus";
    public const string bestTimeExpertPlus = "best_time_expert_plus";
#else
    public const string winsEasy = "";
    public const string winsMedium = "";
    public const string winsHard = "";
    public const string winsExpert = "";
    public const string winsEasyPlus = "";
    public const string winsMediumPlus = "";
    public const string winsHardPlus = "";
    public const string winsExpertPlus = "";
    public const string bestTimeEasy = "";
    public const string bestTimeMedium = "";
    public const string bestTimeHard = "";
    public const string bestTimeExpert = "";
    public const string bestTimeEasyPlus = "";
    public const string bestTimeMediumPlus = "";
    public const string bestTimeHardPlus = "";
    public const string bestTimeExpertPlus = "";
#endif
}
