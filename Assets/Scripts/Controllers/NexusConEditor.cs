#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	[CustomEditor( typeof( NexusCon ) )]
	public class NexusConEditor : Editor {

		int numSpirals = 1;
		int numSpokes = 15;
		float diameter = .2f;
		float centreDiameter = .01f;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if( GUILayout.Button( "Gen Cnxns On Children" ) ) {
				foreach( Object target in targets ) {
					( (NexusCon)target ).CreateSegmentsOnChildrenInEditor();
				}
			}

			GUILayout.Label( "Gen" );
			numSpirals = EditorGUILayout.IntField( "Num spirals:", numSpirals );
			numSpokes = EditorGUILayout.IntField( "Num spokes:", numSpokes );
			diameter = EditorGUILayout.FloatField( "Diameter:", diameter );
			centreDiameter = EditorGUILayout.FloatField( "Centre Diameter:", centreDiameter );

			if( GUILayout.Button( "Gen Spiral" ) ) {
				foreach( Object target in targets ) {
					( (NexusCon)target ).GenNexuses( NexusCon.GenShape.Spiral, numSpirals, numSpokes, diameter, centreDiameter );
				}
			}
		}
	}
}
#endif