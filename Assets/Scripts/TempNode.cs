using UnityEngine;

public class TempNode : MonoBehaviour
{
    public class TempNodeValues
    {
        public int value;
        public Vector2 position;

        public TempNodeValues (Vector2 pos, int val)
        {
            position = pos;
            value = val;
        }
    }
}
