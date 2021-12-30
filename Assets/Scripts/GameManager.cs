using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string PLAYER_ID_PRE = "Player ";
    [System.Obsolete]
    private static Dictionary<string, PlayerInformation> playerDictionary = new Dictionary<string, PlayerInformation>();

    [System.Obsolete]
    public static void RegisterPlayer(string netId, PlayerInformation player) {
        string ID = PLAYER_ID_PRE + netId;
        playerDictionary.Add(ID, player);
        player.transform.name = ID;
    }

    [System.Obsolete]
    public static void RemovePlayer(string netId) {
        playerDictionary.Remove(netId);
    }

    [System.Obsolete]
    public static PlayerInformation getPlayer(string netId) {
        return playerDictionary[netId];
    }

    [System.Obsolete]
    public static void AnyoneDead(string netId) {
        foreach (string id in playerDictionary.Keys) {
            playerDictionary[id].GoodGame(id != netId);
        }
    }
}
