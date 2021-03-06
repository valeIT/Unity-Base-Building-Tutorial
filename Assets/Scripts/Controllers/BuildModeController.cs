﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour {


	bool buildModeIsObjects = false;
	TileType buildModeTile = TileType.Floor;
	string buildModeObjectType;

	// Use this for initialization
	void Start () {
	}

	public void DoBuild (Tile t) {
		if (t == null)
			return;
		if (buildModeIsObjects) {

			//This instantly builds the furniture:
			//WorldController.Instance.World.PlaceFurniture(buildModeObjectType, t);

			// Can we build the furniture in the selected tile?
			//Run the ValidPlacement function!

			string furnitureType = buildModeObjectType;

			if (WorldController.Instance.world.IsFurniturePlacementValid(furnitureType, t)
			     && t.pendingFurnitureJob == null) {
				//This tile is valid for this furniture
				//Create a job for it to be build

				Job j = new Job(t, furnitureType, (theJob) => {
					WorldController.Instance.world.PlaceFurniture(furnitureType, theJob.tile);
					t.pendingFurnitureJob = null;
				});

				//FIXME: I dont't like having to manually and explicitly set flags that prevent conflicts. It's too easy to forget to set/clear them!
				t.pendingFurnitureJob = j;

				j.RegisterJobCancelCallback((theJob) => {
					theJob.tile.pendingFurnitureJob = null;
				});

				//Add a job to the queue
				WorldController.Instance.world.jobQueue.Enqueue(j);
			}


		} else
			t.Type = buildModeTile;
		
	}

	public void SetMode_BuildFloor () {
		buildModeTile = TileType.Floor;
		buildModeIsObjects = false;
	}

	public void SetMode_Bulldoze () {
		buildModeTile = TileType.Empty;
		buildModeIsObjects = false;
	}

	public void SetMode_BuildFurniture (string objectType) {
		//Wall is not a Tile! Wall is a Furniture
		buildModeIsObjects = true;
		buildModeObjectType = objectType;
	}

	public void DoPathfindingTest() {
		WorldController.Instance.world.SetupPathfindingExample();
	}

	public void DisplayPathfindingDebug() {
		WorldController.Instance.world.InvalidateTileGraph();
		new Path_TileGraph(WorldController.Instance.world, true);
	}
}
