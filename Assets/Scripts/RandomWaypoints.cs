using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWaypoints : MonoBehaviour {

	public Transform waypointRoot;
	public int col = 13;
	public int row = 19;

	public Transform[] blocks;
	public float blockRadius = 1f;

	public int minWaypoints;
	public int maxWaypoints = 4;

	public bool setToFirstWaypoint;
	public int stopIndex;
	public int maxUpdates = 20;

	private SimpleWalk[] _walks;
	private List<Transform> _startPoints = new List<Transform>(50);

	private void Awake() {
		Application.targetFrameRate = 60;
		_walks = FindObjectsOfType<SimpleWalk>();
		foreach (var walk in _walks) {
			var num = Random.Range(minWaypoints, maxWaypoints + 1);
			walk.waypoints = new Transform[num];
			for (var i = 0; i < num; i++) {
				var x = Random.Range(0, col);
				var y = Random.Range(0, row);
				var tf = GetTransform(x, y);
				if (i == 0 && _startPoints.Contains(tf)) i--;
				else if (CheckBlocked(tf.position)) i--;
				else walk.waypoints[i] = tf;
			}

			if (setToFirstWaypoint && num > 0) walk.transform.position = walk.waypoints[0].position;
		}
		
		_startPoints.Clear();
	}

	private Transform GetTransform(int x, int y) {
		var child = waypointRoot.GetChild(x);
		return child.GetChild(y);
	}

	private bool CheckBlocked(Vector3 position) {
		foreach (var block in blocks) {
			if ((block.position - position).sqrMagnitude <= blockRadius) return true;
		}

		return false;
	}

	private void Update() {
		var counter = 0;
		for (var i = stopIndex; i < _walks.Length; i++) {
			var walk = _walks[i];
			if (!walk) {
				_walks[i] = null;
				continue;
			}

			counter++;
			walk.EfficientUpdate();
			if (counter >= maxUpdates) {
				stopIndex = i + 1;
				return;
			}
		}

		stopIndex = 0;
	}
}
