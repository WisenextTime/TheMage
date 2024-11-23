using System.Collections.Generic;

namespace TheMage.Core.Scripts.Elements;

public struct Element(string name, string color)
{
	public string Name = name;
	public string Color = color;
	public Dictionary<string, float> Modifiers = new();
}