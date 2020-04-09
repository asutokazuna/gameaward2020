/*
 * @file	NamedArrayAttribute.cs
 * @brief   
 *
 * @author	Kota Nakagami
 * @date1	2020/04/06(月)
 *
 * @version	1.00
 */


using UnityEngine;



public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}


// EOF