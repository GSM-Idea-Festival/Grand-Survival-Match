using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSerialization : MonoBehaviour
{
    public static readonly byte[] mem = new byte[4 * 3];
    public static short SerializeRanking(StreamBuffer outStream, object targetObject)
    {
        RankingData ranking = (RankingData)targetObject;

        lock (mem)
        {
            byte[] bytes = mem;
            int index = 0;

            Protocol.Serialize(Convert.ToInt16(ranking.id), bytes, ref index);
            Protocol.Serialize(ranking.kill, bytes, ref index);
            Protocol.Serialize(ranking.death, bytes, ref index);
            outStream.Write(bytes, 0, 4 * 3);
        }

        return 4 * 3;
    }

    public static object DeserializeRanking(StreamBuffer inStream, short length)
    {
        RankingData ranking = new RankingData();

        lock (mem)
        {
            inStream.Read(mem, 0, 4 * 3);
            int index = 0;

            Protocol.Deserialize(out ranking.id, mem, ref index);
            Protocol.Deserialize(out ranking.kill, mem, ref index);
            Protocol.Deserialize(out ranking.death, mem, ref index);
        }
        return ranking;
    }
}
