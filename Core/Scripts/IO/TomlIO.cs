using Godot;
using TheMage.Core.Scripts.IO.TomlModels;
using Tomlyn;

namespace TheMage.Core.Scripts.IO;

public static class TomlIO
{
	private const string DefaultInputMapPath = "res://Core/Assets/Settings/InputMap.toml";
	private static readonly TomlModelOptions DefaultOption = new()
	{
		ConvertPropertyName = s => s
	};
	public static InputMapModel GetDefaultInputMap()
	{
		var sourceText = FileAccess.Open(DefaultInputMapPath, FileAccess.ModeFlags.Read).GetAsText();
		return Toml.Parse(sourceText).ToModel<InputMapModel>(options: DefaultOption);
	}
	public static InputMapModel GetCustomInputMap()
	{
		const string path = "user://Settings/InputMap.toml";
		if (!FileAccess.FileExists(path))
		{
			var dir = DirAccess.Open("user://");
			if (!dir.DirExists("Settings")) dir.MakeDir("Settings");
			DirAccess.CopyAbsolute(DefaultInputMapPath, path);
		}
		var sourceText = FileAccess.Open(path, FileAccess.ModeFlags.Read).GetAsText();
		return Toml.Parse(sourceText).ToModel<InputMapModel>(options: DefaultOption);
	}

	public static void SaveCustomInputMap(InputMapModel inputMapModel)
	{
		const string path = "user:///Settings/InputMap.toml";
		if (!DirAccess.DirExistsAbsolute(path.GetBaseDir()))
		{
			var dir = DirAccess.Open("user://");
			if (dir.DirExists("Settings")) dir.MakeDir("Settings");
		}
		var text = Toml.FromModel(inputMapModel, options: DefaultOption);
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		file.StoreString(text);
	}
}