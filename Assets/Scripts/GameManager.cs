using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string PLAYER_ID_PRE = "Player ";
    [System.Obsolete]
    private static Dictionary<string, PlayerInformation> playerDictionary = new Dictionary<string, PlayerInformation>();

    private static Vector3[] listPostions = {new Vector3(-32, 3, -18), new Vector3(32, 3, 18)};

    [System.Obsolete]
    public static Vector3 RegisterPlayer(string netId, PlayerInformation player) {
        string ID = PLAYER_ID_PRE + netId;
        playerDictionary.Add(ID, player);
        player.transform.name = ID;
        return listPostions[playerDictionary.Count - 1];
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
            playerDictionary[id].RpcGoodGame(id != netId);
        }
    }

    [System.Obsolete]
    public static PlayerInformation getPlayerByIndex(int index) {
        int i = 0;
        foreach (string id in playerDictionary.Keys)
        {
            if (index == i) return playerDictionary[id];
            i++;
        }
        return null;
    }
}
