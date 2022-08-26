using System.Collections.Generic;
using UnityEngine;

internal static class Game
{
    static List<int> completedLevels = new List<int>(10);

    internal static void MarkLevelComplete(int levelID)
    {
        if(completedLevels.Contains(levelID) == false)
            completedLevels.Add(levelID);
    }

    internal static int CountCompletedLevels() => completedLevels.Count;
}
