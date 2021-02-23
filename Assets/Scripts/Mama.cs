using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class Mama : MonoBehaviour {

		[Header( "Obj Refs" )]
		public GameObject ShadowCasterBody;

		private Rigidbody rb;

		private void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		public void ToggleShadowCasterBody() {
			ShadowCasterBody.SetActive( !ShadowCasterBody.activeSelf );
		}

		public void ReceiveMouseInput( Vector3 mouseInput ) {
			if( rb.velocity.magnitude * Vector3.Dot( mouseInput.normalized, rb.velocity.normalized ) < MamaCon.Instance.MaxMamaVelocity ) {
				rb.AddForce( mouseInput.x * Time.deltaTime * MamaCon.Instance.MamaSpeedForce, 0, mouseInput.z * Time.deltaTime * MamaCon.Instance.MamaSpeedForce, ForceMode.Force );
			}
		}
	}
}