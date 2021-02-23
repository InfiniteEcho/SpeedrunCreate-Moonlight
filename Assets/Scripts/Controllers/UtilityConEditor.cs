#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	[CustomEditor( typeof( UtilityCon ) )]
	public class UtilityConEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if( GUILayout.Button( "Set All Child Renderer Materials" ) ) {
				foreach( Object target in targets ) {
					( (UtilityCon)target ).SetAllChildRenderers();
				}
			}
		}
	}
}
#endif