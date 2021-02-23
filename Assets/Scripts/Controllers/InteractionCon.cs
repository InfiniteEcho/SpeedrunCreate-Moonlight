using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class InteractionCon : MonoBehaviour {

		public static InteractionCon Instance;


		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}

		private void Start() {

			Cursor.lockState = CursorLockMode.Locked;

		}


		private void Update() {

			//if( Input.GetMouseButtonDown( 0 ) ) {
			//	Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
			//	int walkableLayerMask = 1 << 6;
			//	//int renderSurfaceMask = 1 << 7;

			//	if( Physics.Raycast( mouseRay, out RaycastHit walkableSpotHit, Mathf.Infinity, walkableLayerMask ) ) {
			//		foreach( Spiderling s in FindObjectsOfType<Spiderling>() ) {
			//			s.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = walkableSpotHit.point;
			//		}
			//	}


			//	ShadowCon.Instance.PerformShadowCheck( Mathf.FloorToInt( Input.mousePosition.x ), Mathf.FloorToInt( Input.mousePosition.y ) );
			//}

			//if( Input.GetKeyDown( KeyCode.S ) ) {
			//	Cursor.lockState = CursorLockMode.Locked;
			//}
			//if( Input.GetKeyUp( KeyCode.S ) ) {
			//	Cursor.lockState = CursorLockMode.None;
			//}

			if( Input.GetMouseButton( 0 ) ) {

				if( Input.GetKey( KeyCode.A ) ) {

				}
				if( Input.GetKey( KeyCode.D ) ) {

				}
				if( Input.GetKey( KeyCode.S ) ) {

				}
			}

			if( Input.GetKey( KeyCode.S ) ) {
				//Debug.Log( Input.GetAxis( "Mouse X" ) );

				Vector3 mouseInput = Vector3.right * Mathf.Clamp( Input.GetAxis( "Mouse X" ), -5, 5 ) + Vector3.forward * Mathf.Clamp( Input.GetAxis( "Mouse Y" ), -5, 5 );

				MamaCon.Instance.Mama.ReceiveMouseInput( mouseInput );
			}

			if( Input.GetMouseButtonDown( 1 ) ) {

				if( Input.GetKey( KeyCode.A ) ) {

				}
				if( Input.GetKey( KeyCode.D ) ) {

				}
				if( Input.GetKey( KeyCode.S ) ) {
					MamaCon.Instance.Mama.ToggleShadowCasterBody();
				}

			}
		}
	}
}