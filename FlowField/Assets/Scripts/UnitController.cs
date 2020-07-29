﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.FlowField {
	public class UnitController : MonoBehaviour
	{
		public GridController gridController;
		public GameObject unitPrefab;
		public int numUnits;
		public float moveSpeed;

		private List<GameObject> unitsInGame;

		private void Awake()
		{
			unitsInGame = new List<GameObject>();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SpawnUnits();
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				DestroyUnits();
			}
		}

		private void FixedUpdate()
		{
			if(gridController.sceneGrid == null) { return; }
			foreach(GameObject unit in unitsInGame)
			{
				Node nodeBelow = gridController.sceneGrid.GetNodeFromWorldPos(unit.transform.position);
				Vector3 moveDirection = new Vector3(nodeBelow.bestDirection.Vector.x, 0, nodeBelow.bestDirection.Vector.y);
				Rigidbody unitRB = unit.GetComponent<Rigidbody>();
				unitRB.velocity = moveDirection * moveSpeed;
			}
		}

		private void SpawnUnits()
		{
			Vector2Int gridSize = gridController.gridSize;
			float nodeRadius = gridController.nodeRadius;
			Vector2 maxSpawnPos = new Vector2(gridSize.x * nodeRadius * 2 + nodeRadius, gridSize.y * nodeRadius * 2 + nodeRadius);
			int colMask = LayerMask.GetMask("Impassible", "Units");
			Vector3 newPos = new Vector3();
			for (int i = 0; i < numUnits; i++)
			{
				GameObject newUnit = Instantiate(unitPrefab);
				newUnit.transform.parent = transform;
				unitsInGame.Add(newUnit);
				do
				{
					newPos = new Vector3(UnityEngine.Random.Range(0, maxSpawnPos.x), 0, UnityEngine.Random.Range(0, maxSpawnPos.y));
					newUnit.transform.position = newPos;
				}
				while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0);
			}
		}

		private void DestroyUnits()
		{
			foreach(GameObject go in unitsInGame)
			{
				Destroy(go);
			}
			unitsInGame.Clear();
		}
	}
}