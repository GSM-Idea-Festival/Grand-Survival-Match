using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWithTimeSerialization : MonoBehaviour
{
    public static readonly byte[] mem = new byte[4 + 6];
    public static short SerializeRanking(StreamBuffer outStream, object targetObject)
    {
        BuffWithTime data = (BuffWithTime)targetObject;

        lock (mem)
        {
            byte[] bytes = mem;
            int index = 0;

            Protocol.Serialize(Convert.ToInt16(data.buff), bytes, ref index);
            Protocol.Serialize(data.time, bytes, ref index);
            outStream.Write(bytes, 0, 4 + 6);
        }

        return 4 + 6;
    }

    public static object DeserializeRanking(StreamBuffer inStream, short length)
    {
        BuffWithTime data = new BuffWithTime();
        int a;

        lock (mem)
        {
            inStream.Read(mem, 0, 4 + 6);
            int index = 0;

            Protocol.Deserialize(out a, mem, ref index);
            Protocol.Deserialize(out data.time, mem, ref index);
            data.buff = (Buff)a;
        }
        return data;
    }
}
