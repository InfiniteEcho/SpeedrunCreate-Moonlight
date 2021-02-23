using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class Spiderling : MonoBehaviour {


		private NavMeshAgent brain;

		private void Awake() {
			brain = GetComponent<NavMeshAgent>();
		}

		private void Start() {
			FindCoverCoroutine = FindCover();

			findCoverWFS = new WaitForSeconds( ShadowCon.Instance.ShadowCheckPeriod );

			StartCoroutine( FindCoverCoroutine );
		}

		private ShadowSamplePoint closestShadedPoint;
		private float closestShadedPointSqrDist;
		private float currSPSqrDist;

		private WaitForSeconds findCoverWFS;
		private IEnumerator FindCoverCoroutine;
		private IEnumerator FindCover() {

			while( true ) {

				closestShadedPoint = null;
				closestShadedPointSqrDist = Mathf.Infinity;

				for( int i = 0; i < ShadowCon.Instance.ShadedSSPLen; i++ ) {
				//foreach( ShadowSamplePoint sssp in ShadowCon.Instance.ShadedSSPs ) {
					currSPSqrDist = ( transform.position - ShadowCon.Instance.ShadedSSPs[i].transform.position ).sqrMagnitude;
					if( currSPSqrDist < closestShadedPointSqrDist ) {
						closestShadedPointSqrDist = currSPSqrDist;
						closestShadedPoint = ShadowCon.Instance.ShadedSSPs[i];
					}
				}

				if( closestShadedPoint != null ) {
					brain.SetDestination( closestShadedPoint.transform.position );
					brain.speed = SpiderlingCon.Instance.FullSpeed;
				} else {
					//brain.SetDestination( transform.position );
					brain.speed = SpiderlingCon.Instance.StoppedSpeed;
				}

				yield return findCoverWFS;
			}
		}
	}
}