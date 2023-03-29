using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindOnObject : PropertyAttribute
{
    public int depthIndex;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 or no parameter if you want to find only on object,+1 if you want to look in parents and -1 if you want to look reccursively in childrens</param>
    public FindOnObject(int index = 0)
    {
        depthIndex = index;
    }
}
