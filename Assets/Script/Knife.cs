using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ItemBase
{
    public override void Active()
    {
        Inventory._instance.Add(_item);
    }
}
