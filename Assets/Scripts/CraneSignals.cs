using System;
public static class CraneSignals
{
    // направление: -1..1
    public static event Action<float> OnMoveUpDown;     // positive = up, negative = down
    public static event Action<float> OnMoveEastWest;   // positive = east, negative = west
    public static event Action<float> OnMoveNorthSouth; // positive = north (forward), negative = south (back)

    public static void MoveUpDown(float v) => OnMoveUpDown?.Invoke(v);
    public static void MoveEastWest(float v) => OnMoveEastWest?.Invoke(v);
    public static void MoveNorthSouth(float v) => OnMoveNorthSouth?.Invoke(v);
}

