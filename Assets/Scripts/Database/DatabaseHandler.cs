using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using Proyecto26;

public class DatabaseHandler
{
    private static readonly string databaseURL = AppGlobal.GetDatabaseURL();
    private static readonly string playersTable = AppGlobal.GetPlayersTable();

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostPlayerCallback();
    public delegate void GetPlayerCallback(Player player);
    public delegate void GetTopPlayersCallback(Dictionary<string, Player> players);

    public static void GetPlayer(Player player, string playerId, GetPlayerCallback callback)
    {
        RestClient.Get<Player>($"{databaseURL}{playersTable}/{playerId}.json").Then(player => { 
            callback(player);
        }).Catch(err => {
            
            PostPlayer(player, playerId, () => {});
        });
    }

    public static void PostPlayer(Player player, string playerId, PostPlayerCallback callback)
    {
        RestClient.Put<Player>($"{databaseURL}{playersTable}/{playerId}.json", player).Then(response => {
            callback(); 
        });
    }

    public static void GetTopPlayers(int limit, GetTopPlayersCallback callback)
    {
        RestClient.Get($"{databaseURL}{playersTable}.json?orderBy=\"score\"&limitToLast={limit}").Then(response =>
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
