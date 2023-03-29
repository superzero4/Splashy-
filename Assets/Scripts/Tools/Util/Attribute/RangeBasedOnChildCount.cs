using UnityEngine;

public class RangeBasedOnChildCount : PropertyAttribute
{
    private int _maxOffset;
    public RangeBasedOnChildCount(int max = 0)
    {
        _maxOffset = max;
    }

    public int MaxOffset { get => _maxOffset; }
}
