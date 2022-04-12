using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppGlobalExample
{
    private static bool inProduction = false; // false: testing,  true: production
    private const string projectId = "projectId";
    private const string projectName = "projectName";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/{projectName}/";
    private static readonly string playersTable = (inProduction ? "players" : "test_players");

    public static string GetDatabaseURL()
    {
        return databaseURL;
    }

    public static string GetPlayersTable()
    {
        return playersTable;
    }
}
