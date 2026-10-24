using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameBase;

public class CaveTradeScene : Scene
{
    private readonly Tilemap ground;
    List<ICollidable> tileColliders = new();
    public IReadOnlyList<SpawnInfo> Spawns { get; }
    //

    private readonly Player player;
    private Vector2 playerSpawn;
    //public IReadOnlyList<SpawnInfo> Spawns { get; }
    // references to a tilemap & player if we using either
    public CaveTradeScene(ContentManager content) : base(content)
    {
        //load level here. 
        LevelData level = LevelLoader.LoadLevelData(content, "LoadInfo/TileMap2.xml");
        ground = level.Map;

        Spawns = level.spawns;
        //ground.Scale = new Vector2(3, 3);

        //foreach spawn in spawns: get location, and spawn type here
        //resettiilecolliders
        foreach (var col in ground.CreateColliders()) {
            AddCollider(col);
        }

        foreach (SpawnInfo spawn in Spawns) {
            switch (spawn.Type)
            {
                case ("Player"):
                    Vector2 spawnl = new Vector2(spawn.Column * ground.CellWidth, spawn.Row * ground.CellHeight);
                    var p = new Player("placeholder", this);
                    p.UpdateLocation(spawnl.X, spawnl.Y);
                    player = p;
                    playerSpawn = spawnl;
                    break;
            }
        }
    }
    public override void EnterScene()
    {
        base.EnterScene();
    }   
    public override void Update(GameTime gameTime)
    {
        ground.Update(gameTime);
        base.Update(gameTime);
    }
    public override void ExitScene()
    {
        base.ExitScene();
    }

    public void ResetLevel() { 
    //if level requires a hard reset
    }

    public void OnDestroy() { 
        //maybe not implement iobject yet
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        ground.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}
