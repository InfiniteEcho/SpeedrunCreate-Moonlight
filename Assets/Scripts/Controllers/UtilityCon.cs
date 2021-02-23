using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class UtilityCon : MonoBehaviour {

		[Header( "Obj Refs" )]
		public Material ToSetMat;
		public GameObject ToSetChildRenderers;

		public void SetAllChildRenderers() {
			foreach( Renderer rend in ToSetChildRenderers.GetComponentsInChildren<Renderer>() ) {
				rend.material = ToSetMat;
			}
		}
	}
}