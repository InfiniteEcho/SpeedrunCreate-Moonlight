using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class ShadowCon : MonoBehaviour {

		public static ShadowCon Instance;

		[Range( 0, 1 )]
		public float ShadowCoveredThresh = .75f;
		[Range( 0, 1 )]
		public float InvalidRedThresh = .75f;

		public bool HaveInitializedSSPs = false;
		public ShadowSamplePoint[] SSPs;
		public ShadowSamplePoint[] ShadedSSPs;
		public int ShadedSSPLen = 0;

		[Range( 0, 1 )]
		public float ShadowCheckPeriod = .1f;

		[Header( "Obj Refs" )]
		public Camera ShadowCheckCamera;

		public GameObject DebugRenderTexQuad;
		public GameObject DebugShadowTexQuad;
		public ShadowSamplePoint DebugSSP;


		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}

		Texture2D shadowCheckTex;
		bool doNeedToInitShadowCheckTex = true;
		RenderTexture shadowRT;
		private IEnumerator ShadowCheckCoroutineObj;

		private void Start() {
			shadowRT = new RenderTexture( ShadowCheckCamera.pixelWidth, ShadowCheckCamera.pixelHeight, 0 );
			ShadowCheckCamera.targetTexture = shadowRT;

			foreach( Renderer rend in DebugRenderTexQuad.GetComponentsInChildren<Renderer>() ) {
				rend.sharedMaterial.mainTexture = shadowRT;
			}

			if( !HaveInitializedSSPs )
				InitSSPs();
			ShadowCheckCoroutineWFS = new WaitForSeconds( ShadowCheckPeriod );
			ShadowCheckCoroutineObj = ShadowCheckCoroutine();
			StartCoroutine( ShadowCheckCoroutineObj );
			
		}


		Vector3 sspPixel;
		public void InitSSPs() {
			SSPs = FindObjectsOfType<ShadowSamplePoint>();

			for( int i = 0; i < SSPs.Length; i++ ) {
			//foreach( ShadowSamplePoint ssp in SSPs ) {
				sspPixel = ShadowCheckCamera.WorldToScreenPoint( SSPs[i].transform.position );
				SSPs[i].Idx = i;
				SSPs[i].ScreenPointX = Mathf.FloorToInt( sspPixel.x );
				SSPs[i].ScreenPointY = Mathf.FloorToInt( sspPixel.y );
			}

			ShadedSSPs = new ShadowSamplePoint[SSPs.Length];

			HaveInitializedSSPs = true;
		}


		private WaitForSeconds ShadowCheckCoroutineWFS;
		public IEnumerator ShadowCheckCoroutine() {
			while( true ) {

				PerformShadowCheck();

				yield return ShadowCheckCoroutineWFS;

			}
		}

		public void InitShadowCheckTex( int width, int height ) {
			shadowCheckTex = new Texture2D( width, height );

			doNeedToInitShadowCheckTex = false;
		}

		Color sspPixelColor;
		public void PerformShadowCheck() {

			RenderTexture normalRT = RenderTexture.active;
			RenderTexture.active = ShadowCheckCamera.targetTexture;

			ShadowCheckCamera.Render();

			if( doNeedToInitShadowCheckTex ) 
				InitShadowCheckTex( ShadowCheckCamera.targetTexture.width, ShadowCheckCamera.targetTexture.height );

			shadowCheckTex.ReadPixels( new Rect( 0, 0, ShadowCheckCamera.targetTexture.width, ShadowCheckCamera.targetTexture.height ), 0, 0 );
			shadowCheckTex.Apply();

			// Debug
			foreach( Renderer rend in DebugShadowTexQuad.GetComponentsInChildren<Renderer>() ) {
				rend.sharedMaterial.mainTexture = shadowCheckTex;
			}

			RenderTexture.active = normalRT;


			ShadedSSPLen = 0;

			foreach( ShadowSamplePoint ssp in SSPs ) {

				sspPixelColor = shadowCheckTex.GetPixel( ssp.ScreenPointX, ssp.ScreenPointY );

				ssp.SampledColor = sspPixelColor;
				if( sspPixelColor.r > InvalidRedThresh )
					ssp.ShadowCoverage = 0;
				else
					ssp.ShadowCoverage = 1 - sspPixelColor.g;

				if( ssp.IsShadowCovered ) {
					ShadedSSPs[ShadedSSPLen] = ssp;
					ShadedSSPLen++;
				}
			}


			//Debug.LogFormat( "Begin Time: {0}, End Time: {1}", beginTime, Time.time );
			//Debug.LogFormat( "Time taken: {0}", ( Time.time - beginTime ).ToString( "G" ) );


			//sspPixel = ShadowCheckCamera.WorldToScreenPoint( DebugSSP.transform.position );
			////Debug.LogFormat( "SSP: {0}", sspPixel );

			//Color scColor = shadowCheckTex.GetPixel( Mathf.FloorToInt( sspPixel.x ), Mathf.FloorToInt( sspPixel.y ) );
			//Debug.Log( scColor.ToString( "0.00" ) );

			//DebugSSP.ShadowCoverage = 1 - scColor.g;

			//Debug.LogFormat( "SC: {0}, isCovered: {1}", DebugSSP.ShadowCoverage, DebugSSP.IsShadowCovered );


		}

		public void PerformShadowCheck( int mouseX, int mouseY ) {
			PerformShadowCheck();

			//Debug.Log( shadowCheckTex.GetPixel( mouseX, mouseY ).ToString( "0.00" ) );
		}

	}
}