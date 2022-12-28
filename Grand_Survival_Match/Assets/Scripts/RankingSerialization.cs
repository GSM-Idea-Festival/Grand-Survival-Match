using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSerialization : MonoBehaviour
{
    public static readonly byte[] mem = new byte[8 + 6];
    public static short SerializeRanking(StreamBuffer outStream, object targetObject)
    {
        RankingData ranking = (RankingData)targetObject;

        lock (mem)
        {
            byte[] bytes = mem;
            int index = 0;

            Protocol.Serialize(Convert.ToInt16(ranking._name), bytes, ref index);
            Protocol.Serialize(ranking.kill, bytes, ref index);
            Protocol.Serialize(ranking.death, bytes, ref index);
            outStream.Write(bytes, 0, 8 + 6);
        }

        return 8 + 6;
    }

    public static object DeserializeRanking(StreamBuffer inStream, short length)
    {
        RankingData ranking = new RankingData();
        int a;

        lock (mem)
        {
            inStream.Read(mem, 0, 8 + 6);
            int index = 0;

            Protocol.Deserialize(out a, mem, ref index);
            Protocol.Deserialize(out ranking.kill, mem, ref index);
            Protocol.Deserialize(out ranking.death, mem, ref index);
        }
        ranking._name = a.ToString();
        return ranking;
    }
}
