using UnityEngine;
using System.Collections;

/// <summary>
/// Class that handles specific formations of waves for levels
/// </summary>
public class Wave {
    public string[] wave;
    public Wave()
    {
        int limit = 20;
        wave = new string[limit];
        for (int i = 0; i < limit; i++)
        {
            wave[i] = "Monster";
            if(i%2 == 0)
                wave[i] = "Demon";
            if(i%3 == 0)
                wave[i] = "Worm";
        }
    }
}
