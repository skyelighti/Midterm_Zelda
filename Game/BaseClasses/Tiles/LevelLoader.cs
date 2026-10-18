using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GameBase.Tiles;

public static class LevelLoader
{
    public static LevelData LoadLevelData(ContentManager content, string filename) {
        string filePath = Path.Combine(content.RootDirectory, filename);
        
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;
                Dictionary<string, Tileset> tileSetDict = new();
                foreach (XElement set in root.Elements("Tileset")) {
                    string n = (string)set.Element("name");
                    int tWidth = (int)set.Element("tileWidth");
                    int tHeight = (int)set.Element("tileHeight");
                    TextureRegion s = Services.Sprites.GetRegion((string)set.Element("sprite"));
                    
                    Tileset tileSet = new Tileset(n, s, tWidth, tHeight);
                    //creates the base tileset


                    //configs the base tileset, i should maybe seperate this out, so that multiple tilesets can be loaded
                    foreach (int solid in ReadTileIds(root.Element("SolidTiles"), tileSet.Count)) {
                        tileSet.SetSolid(solid, true);
                    }
                    foreach (int trigger in ReadTileIds(root.Element("TriggerTiles"), tileSet.Count))
                    {
                        tileSet.SetTrigger(trigger,true);
                    }
                    foreach (XElement l in root.Elements("TileLayers").Elements("Layer")) {
                        string layerName = (string)l.Attribute("name");
                        Layer layer = Enum.Parse<Layer>(layerName);
                        foreach (int id in ReadTileIds(l, tileSet.Count))
                        {
                            tileSet.SetLayer(id, layer);
                        }
                    }
                    tileSetDict[n] = tileSet;

                }
                Tilemap map = ReadLayer(tileSetDict, root.Element("Tiles"));


                List<SpawnInfo> spawns = ReadSpawns(root, map.Columns, map.Rows);
                LevelData lData = new LevelData(map,spawns);
            }
        }
        return null;
    }

    public static List<SpawnInfo> ReadSpawns(XElement root, int columns, int rows)
    {
        //makes the spawninfos from the xml and puts them all in a list
        List<SpawnInfo> spawns = new();
        foreach (XElement spawn in root.Elements("Spawns"))
        {
            //i should make soemthing for constructing spawns specifically
            string t = (string)spawn.Attribute("type");
            int column = int.Parse((string)spawn.Element("column"));
            int height = int.Parse((string)spawn.Element("height"));
            var s = new SpawnInfo(t, column, height);

        }
        return spawns;
    }
    private static Tilemap ReadLayer(Dictionary<string, Tileset> tileset, XElement element)
    {
        //determines the rows and columns, populating the tileset based off the xml.
        //specifically seperated from load level data if i want to layer multiple tilemaps
        string[] rows = element.Value.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int rowCount = rows.Length;
        int columnCount = rows[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

        Tilemap map = new Tilemap(tileset, columnCount, rowCount, tileset.Values.First().TileWidth, tileset.Values.First().TileHeight);
        return map;
    }


    private static IEnumerable<int> ReadTileIds(XElement element, int tileCount)
    {
        //used for solid, and trigger tiles
        //creates a list when returning
        if (element == null)
            yield break;

        string[] values = element.Value.Split(new[] { ' ', '\t', '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);

        foreach (string value in values)
        {
            int tileID = int.Parse(value);   
            yield return tileID + 1;
        }

    }
}
