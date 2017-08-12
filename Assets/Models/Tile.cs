﻿using UnityEngine;
using System.Collections;
using System;

public class Tile {

	Action<Tile> cbTileChanged;
	

	TileType _type = TileType.Empty;

	public TileType Type {
		get {
			return _type;
		}
		set {
			if (_type != value) {
				_type = value;
				//Call the callback and let things know we've changed
				if (cbTileChanged != null)
					cbTileChanged(this);
			}
		}
	}

	Inventory inventory;
	public Furniture furniture {
		get;
		protected set;
	}

	public World world {
		get;
		protected set;
	}
	int x, y;

	public Job pendingFurnitureJob;

	public int X {
		get {
			return x;
		}
	}

	public int Y {
		get {
			return y;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> class.
	/// </summary>
	/// <param name="world">A World instance.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile (World world, int x, int y) {
		this.world = world;
		this.x = x;
		this.y = y;
	}

	/// <summary>
	/// Registers the tile type changed callback.
	/// </summary>
	public void RegisterTileTypeChangedCallback (Action<Tile> callback) {
		//It behaves like an array, you can call this function multiple times
		this.cbTileChanged += callback;
	}

	/// <summary>
	/// Unregisters the tile type changed callback.
	/// </summary>
	public void UnregisterTileTypeChangedCallback (Action<Tile> callback) {
		this.cbTileChanged -= callback;
	}

	public bool PlaceObject (Furniture objInstance) {
		if (objInstance == null) {
			//We are uninstalling whatever was here before.
			furniture = null;
			return true;
		}

		//objInstance isn't null
		if (furniture != null) {
			Debug.LogError("Trying to assign a furniture to a tile that already has one!");
			return false;
		}

		furniture = objInstance;
		return true;
	}
		
	public bool IsNeighbour(Tile tile, bool diagOkay = false) {
		if (diagOkay) 
			return Mathf.Abs(this.X - tile.X) <= 1 && Mathf.Abs(this.Y - tile.Y) <= 1;
		else
			return (this.X == tile.X && Mathf.Abs(this.Y - tile.Y) <= 1) || (this.Y == tile.Y && Mathf.Abs(this.X - tile.X) <= 1);
	}
}

public enum TileType {
	Empty,
	Floor}
;

