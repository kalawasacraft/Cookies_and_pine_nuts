// Create cs file AppGlobal and copy this code
// In realtime database of Firebase create these tables:
/*
    players: {
        anonimo0: {
            score: 0
        },
        .
        .
        .
        anonimo9: {
            score: 0
        }
    }

    test_players: {
        anonimo0: {
            score: 0
        },
        .
        .
        .
        anonimo9: {
            score: 0
        }
    }
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppGlobalExample
{
    private static bool inProduction = false; // false: testing,  true: production
    private const string projectId = "projectId";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
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
