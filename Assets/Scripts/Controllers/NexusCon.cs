using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class NexusCon : MonoBehaviour {

		public static NexusCon Instance;



		//private List<Nexus> SceneNexuses = new List<Nexus>();


		[Header( "Obj Refs" )]
		public GameObject NexusPrefab;
		public GameObject WebSegmentPrefab;
		public Transform SegmentParent;

		public GameObject ToGenOn;

		[Header( "Editor Obj Refs" )]
		public GameObject ToCreateConnectionsOnChildren;

		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}

		private void Start() {

			CreateSegments( FindObjectsOfType<Nexus>().ToList() );

		}

		public void CreateSegmentsOnChildrenInEditor() {

			if( ToCreateConnectionsOnChildren != null )
				CreateSegments( ToCreateConnectionsOnChildren.GetComponentsInChildren<Nexus>().ToList() );

		}

		public void CreateSegments( List<Nexus> sceneNexuses ) {

			GameObject segParent = new GameObject( "Segment Parent" );
			segParent.transform.SetParent( sceneNexuses[0].transform.parent, false );
			segParent.transform.localPosition = Vector3.zero;


			foreach( Nexus nexus in sceneNexuses ) {
				foreach( Nexus.CnxnInfo cnxn in nexus.Cnxns ) {
					if( cnxn.Status == Nexus.CnxnStatus.Unconnected ) {
						switch( cnxn.Type ) {
							case Nexus.CnxnType.Web:
#if UNITY_EDITOR
								GameObject newSeg = (GameObject)PrefabUtility.InstantiatePrefab( WebSegmentPrefab, segParent.transform );
#else
								GameObject newSeg = Instantiate( WebSegmentPrefab, segParent.transform );
#endif
								newSeg.transform.position = nexus.transform.position;
								newSeg.transform.LookAt( cnxn.ConnectedTo.transform );
								float dist = Vector3.Distance( nexus.transform.position, cnxn.ConnectedTo.transform.position );
								newSeg.transform.localScale = new Vector3(
									newSeg.transform.localScale.x,
									newSeg.transform.localScale.y,
									dist * ( 1 / newSeg.transform.lossyScale.z ) );

								//float angleDiff = Vector3.SignedAngle( newSeg.transform.up, Vector3.forward, Vector3.right );
								//newSeg.transform.localRotation = Quaternion.Euler( new Vector3(
								//	newSeg.transform.localRotation.eulerAngles.x,
								//	newSeg.transform.localRotation.eulerAngles.y,
								//	angleDiff
								//	) );

								newSeg.transform.rotation = Quaternion.FromToRotation( newSeg.transform.up, -Vector3.forward ) * newSeg.transform.rotation;

								Segment newSegSeg = newSeg.GetComponent<Segment>();
								newSegSeg.Parents = new Nexus[] { nexus, cnxn.ConnectedTo };
								newSegSeg.MyCnxnType = cnxn.Type;

								cnxn.Status = Nexus.CnxnStatus.Connected;
								cnxn.Segment = newSeg.GetComponent<Segment>();
								Nexus.CnxnInfo childCnxn = cnxn.ConnectedTo.Cnxns.Find(
									cnxnInfo => cnxnInfo.ConnectedTo == nexus
									);
								childCnxn.Status = Nexus.CnxnStatus.Connected;
								childCnxn.Segment = newSeg.GetComponent<Segment>();
								break;
						}
					}
				}
			}
		}

		public enum GenShape {
			None = 0,
			Spiral,
			Spokes
		}

		public void GenNexuses( GenShape toGen, int numSpirals, int numSpokes, float diameter, float centreDiameter ) {

			//GameObject parent = Selection.activeGameObject;

			foreach( Nexus toDel in ToGenOn.GetComponentsInChildren<Nexus>() )
				DestroyImmediate( toDel.gameObject );
			foreach( Segment toDel in ToGenOn.GetComponentsInChildren<Segment>() ) {
				DestroyImmediate( toDel.gameObject );
			}
			
			List<Nexus> createdNexuses = new List<Nexus>();

			float halfDia = diameter / 2;

			float currentDist = centreDiameter / 2;
			float distInc = ( halfDia - ( centreDiameter / 2 ) ) / ( numSpirals * numSpokes );
			float pi2 = Mathf.PI * 2;

			Nexus prevNexus = null;

			switch( toGen ) {
				case GenShape.Spiral:

					GameObject NexusParent = new GameObject( "NexusParent" );
					NexusParent.transform.SetParent( ToGenOn.transform, false );
					NexusParent.transform.localPosition = Vector3.zero;
					NexusParent.AddComponent<WebBase>();

#if UNITY_EDITOR
					Nexus centreNexus = ((GameObject)PrefabUtility.InstantiatePrefab( NexusPrefab, ToGenOn.transform )).GetComponent<Nexus>();
#else
					Nexus centreNexus = Instantiate( NexusPrefab, ToGenOn.transform ).GetComponent<Nexus>();
#endif
					centreNexus.transform.localPosition = Vector3.zero;

					Dictionary<int, Nexus> prevSpokeNexuses = new Dictionary<int, Nexus>();

					for( int i = 0; i < numSpokes; i++ )
						prevSpokeNexuses[i] = centreNexus;

					for( int currSpiral = 0; currSpiral < numSpirals; currSpiral++ ) {
						for( int currSpoke = 0; currSpoke < numSpokes; currSpoke++ ) {
							float s = Mathf.Sin( ( pi2 / numSpokes ) * currSpoke );
							float c = Mathf.Cos( ( pi2 / numSpokes ) * currSpoke );

#if UNITY_EDITOR
							Nexus nexus = ((GameObject)PrefabUtility.InstantiatePrefab( NexusPrefab, NexusParent.transform )).GetComponent<Nexus>();
#else
							Nexus nexus = Instantiate( NexusPrefab, NexusParent.transform ).GetComponent<Nexus>();
#endif
							nexus.transform.localPosition = ( new Vector3( s + c, c - s, 0 ) ) * currentDist;

							if( prevNexus != null ) {
								ConnectNexuses( prevNexus, nexus );
							}
							prevNexus = nexus;

							if( prevSpokeNexuses.ContainsKey( currSpoke ) )
								ConnectNexuses( prevSpokeNexuses[currSpoke], nexus );
							prevSpokeNexuses[currSpoke] = nexus;

							createdNexuses.Add( nexus );
							currentDist += distInc;
						}
					}
					break;
				case GenShape.Spokes:
					break;
			}

			CreateSegments( createdNexuses );
		}

		public void ConnectNexuses( Nexus first, Nexus second ) {
			Nexus.CnxnInfo firstCnxn = new Nexus.CnxnInfo();
			Nexus.CnxnInfo secondCnxn = new Nexus.CnxnInfo();

			firstCnxn.ConnectedTo = second;
			secondCnxn.ConnectedTo = first;
			firstCnxn.Status = secondCnxn.Status = Nexus.CnxnStatus.Unconnected;
			firstCnxn.Type = secondCnxn.Type = Nexus.CnxnType.Web;

			first.Cnxns.Add( firstCnxn );
			second.Cnxns.Add( secondCnxn );
		}

	}
}