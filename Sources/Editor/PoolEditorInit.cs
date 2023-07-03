using System.Linq;
using UnityEditor;

namespace InsaneOne.PerseidsPooling
{
	[InitializeOnLoad]
	public sealed class AddDefineSymbols : Editor
	{
		static readonly string[] defines = new string[] { "PERSEIDS_POOLING" };
		
		static AddDefineSymbols()
		{
			var group = EditorUserBuildSettings.selectedBuildTargetGroup;
			var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
			var allDefines = definesString.Split(';').ToList();
			
			allDefines.AddRange(defines.Except(allDefines));

			var definesStr = string.Join(";", allDefines.ToArray());
			
			PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesStr);
		}
	}
}