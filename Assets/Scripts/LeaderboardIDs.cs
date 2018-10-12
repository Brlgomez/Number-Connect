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
    public const string winsEasy = GPGSIds.leaderboard_wins__easy;
    public const string winsMedium = GPGSIds.leaderboard_wins__medium;
    public const string winsHard = GPGSIds.leaderboard_wins__hard;
    public const string winsExpert = GPGSIds.leaderboard_wins__expert;
    public const string winsEasyPlus = GPGSIds.leaderboard_wins__easy_2;
    public const string winsMediumPlus = GPGSIds.leaderboard_wins__medium_2;
    public const string winsHardPlus = GPGSIds.leaderboard_wins__hard_2;
    public const string winsExpertPlus = GPGSIds.leaderboard_wins__expert_2;
    public const string bestTimeEasy = GPGSIds.leaderboard_best_time__easy;
    public const string bestTimeMedium = GPGSIds.leaderboard_best_time__medium;
    public const string bestTimeHard = GPGSIds.leaderboard_best_time__hard;
    public const string bestTimeExpert = GPGSIds.leaderboard_best_time__expert;
    public const string bestTimeEasyPlus = GPGSIds.leaderboard_best_time__easy_2;
    public const string bestTimeMediumPlus = GPGSIds.leaderboard_best_time__medium_2;
    public const string bestTimeHardPlus = GPGSIds.leaderboard_best_time__hard_2;
    public const string bestTimeExpertPlus = GPGSIds.leaderboard_best_time__expert_2;
#endif
}
