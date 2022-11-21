using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public void AddInInventory(GameObject ob);
    public void RemoveFromInventory(GameObject ob);
}
