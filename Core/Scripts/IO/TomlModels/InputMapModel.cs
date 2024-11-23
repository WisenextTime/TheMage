using System.Collections.Generic;

namespace TheMage.Core.Scripts.IO.TomlModels;

public class InputMapModel
{
	// ReSharper disable once FieldCanBeMadeReadOnly.Global
	// ReSharper disable once UnassignedField.Global
	public Dictionary<string, List<string>> InputMap { get; set; }
}