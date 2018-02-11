using UnityEngine;

public static class InputUtils
{
    public static bool ValidMove(InputProperty input)
    {
        return ValidMove(input.Move);   
    }

    public static bool ValidMove(Vector2 move)
    {
        float tolerance = 0.01f;

        return (move.x < -tolerance || move.x > tolerance) &&
               (move.y < -tolerance || move.y > tolerance);
    }
}