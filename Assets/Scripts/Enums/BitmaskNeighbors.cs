using System;
[Flags]
public enum BitmaskNeighbors
{
    //Binary value
    None = 0,//00000000
    NE = 1,  //00000001
    E = 2,   //00000010
    SE = 4,  //00000100
    S = 8,   //00001000
    SW = 16, //00010000
    W = 32,  //00100000
    NW = 64, //01000000
    TOP = 128,
    BOTTOM = 256,
    All = NE | E | SE | S | SW | W | NW | TOP | BOTTOM
}