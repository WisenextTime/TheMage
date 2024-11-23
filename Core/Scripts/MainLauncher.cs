using TheMage.Core.Scripts.Input;

namespace TheMage.Core.Scripts;

public static class MainLauncher
{
	public static void Launch()
	{
		InputMap.PreloadInputMap();
	}
}