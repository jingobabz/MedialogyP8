﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileID;
    public int[] wallArray; //[0] = north, [1] = east, [2]= south, [3] = west. 1 = traversable, 0 = not traversable

    void Awake()
    {
        wallArray = new int[] { 0, 0, 0, 0 };
        tileID = 0;
    }

    private void SetIDFromArray()
    {
        tileID = 8 * wallArray[3] + 4 * wallArray[2] + 2 * wallArray[1] + wallArray[0];
        SetMaterial(tileID, 'p');
    }

    // Direction specifies which side we want to traverse through,
    // the opposite side of that on the "to" tile is always dir-2,
    // and to prevent out of bounds we take the mod(4) of it.
    public static bool IsTraversable(Tile from, Tile to, int direction)
    {
        int oppositeDirection = Mod(direction - 2, 4);
        return (from.wallArray[direction] == to.wallArray[oppositeDirection]);
    }

    public void SetWall(int direction, int val)
    {
        wallArray[direction] = val;
        SetIDFromArray();
    }

    public void OpenWall(int direction)
    {
        SetWall(direction, 1);
    }

    public void CloseWall(int direction)
    {
        SetWall(direction, 0);
    }

    public static void ConnectTiles(Tile from, Tile to, int direction)
    {
        int oppositeDirection = Mod(direction - 2, 4);
        from.OpenWall(direction);
        to.OpenWall(oppositeDirection);
    }

    public static void DisconnectTiles(Tile from, Tile to, int direction)
    {
        int oppositeDirection = Mod(direction - 2, 4);
        from.CloseWall(direction);
        to.CloseWall(oppositeDirection);
    }

    public void SetIDForDebugAndDeleteThisMethodLater(int i)
    {
        tileID = i;
    }


    // Could be done with an event and a listener so the functions themselves
    // wouldn't have to call setmaterial but it could react to changes
    public void SetMaterial(int materialID, char materialType)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = Resources.Load<Material>(materialType + materialID.ToString());
    }

    public void SetArrayFromID()
    {
        int temp = tileID;
        for (int i = 3; i >= 0; i--)
        {
            wallArray[i] = temp / (int)Mathf.Pow(2, i);
            temp %= (int)Mathf.Pow(2, i);
        }
        SetMaterial(tileID, 'p');
    }

    public void SetWallArray(int[] a)
    {
        wallArray = a;
        SetIDFromArray();
    }

    public int[] GetWallArray()
    {
        return wallArray;
    }

    public void SetTileID(int i)
    {
        tileID = i;
        SetArrayFromID();
    }

    public int GetTileID()
    {
        return tileID;
    }

    //Modulo helper function cause the one in C# is actaully a remainder so it would give negative values
    public static int Mod(int n, int divisor)
    {
        return ((n %= divisor) < 0) ? n + divisor : n;
    }
}
