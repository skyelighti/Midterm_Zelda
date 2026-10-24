using FlatRedBall.Glue.StateInterpolation;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public partial class SceneManager
{
    public SceneManager(ContentManager content)
    {
        Instance = this;
        SceneM.SetNumOfStates(3);
        //SceneDict.Add(SceneID.StartScene, new StartScene(content));
        SceneDict.Add(SceneID.GameScene, new GameScene(content));
        SceneDict.Add(SceneID.CaveSwordScene, new CaveSwordScene(content));
        SceneDict.Add(SceneID.CaveTradeScene, new CaveTradeScene(content));
        //SceneDict.Add(SceneID.EndScene, new EndScene(content));
        //SceneM.SetStateFunctions(0, SceneDict[SceneID.StartScene].EnterScene, SceneDict[SceneID.StartScene].Update, SceneDict[SceneID.StartScene].ExitScene);
        SceneM.SetStateFunctions(0, SceneDict[SceneID.GameScene].EnterScene, SceneDict[SceneID.GameScene].Update, SceneDict[SceneID.GameScene].ExitScene);
        SceneM.SetStateFunctions(1, SceneDict[SceneID.CaveSwordScene].EnterScene, SceneDict[SceneID.CaveSwordScene].Update, SceneDict[SceneID.CaveSwordScene].ExitScene);
        SceneM.SetStateFunctions(2, SceneDict[SceneID.CaveTradeScene].EnterScene, SceneDict[SceneID.CaveTradeScene].Update, SceneDict[SceneID.CaveTradeScene].ExitScene);
        //SceneM.SetStateFunctions(3, SceneDict[SceneID.EndScene].EnterScene, SceneDict[SceneID.EndScene].Update, SceneDict[SceneID.EndScene].ExitScene);
        //init stateMachine
        SceneM.SetState((int)SceneID.GameScene);
    }
}
