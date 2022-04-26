using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using Proyecto26;

public class DatabaseHandler
{
    private static readonly string databaseURL = AppGlobal.GetDatabaseURL();
    private static readonly string playersTable = AppGlobal.GetPlayersTable();
    private static readonly List<string> levelTables = new List<string> {
        AppGlobal.GetEasyTable(),
        AppGlobal.GetMediumTable(),
        AppGlobal.GetHardTable()
    };

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostPlayerCallback();
    public delegate void GetPlayerCallback(Player player);
    public delegate void GetTopPlayersCallback(Dictionary<string, Player> players);

    public static void GetPlayer(Player player, string playerId, int levelIndex, GetPlayerCallback callback)
    {
        RestClient.Get<Player>($"{databaseURL}{levelTables[levelIndex]}/{playerId}.json").Then(player => { 
            callback(player);
        }).Catch(err => {
            
            PostPlayer(player, playerId, levelIndex, () => {});
        });
    }

    public static void PostPlayer(Player player, string playerId, int levelIndex, PostPlayerCallback callback)
    {
        RestClient.Put<Player>($"{databaseURL}{levelTables[levelIndex]}/{playerId}.json", player).Then(response => {
            callback(); 
        });
    }

    public static void GetTopPlayers(int limit, int levelIndex, GetTopPlayersCallback callback)
    {
        RestClient.Get($"{databaseURL}{levelTables[levelIndex]}.json?orderBy=\"score\"&limitToLast={limit}").Then(response =>
        {
            var responseJson = response.Text;

            var data = fsJsonParser.Parse(responseJson);
            var records = new Dictionary<string, Player>();

            if (!data.IsNull) {
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(Dictionary<string, Player>), ref deserialized);

                records = deserialized as Dictionary<string, Player>;
            }

            callback(records);
        }).Catch(err => {
            Debug.Log(err);
        });
    }
}
