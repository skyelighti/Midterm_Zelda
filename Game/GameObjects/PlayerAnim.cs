using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public partial class Player
{
    //animation logic belongs in here
    public void EnterIdle() { }
    public void UpdateIdle(GameTime gameTime) { }
    public void ExitIdle() { }

    public void EnterMoving() { }
    public void UpdateMoving(GameTime gameTime) { }
    public void ExitMoving() { }

    public void EnterAttack() { }
    public void UpdateAttack(GameTime gameTime) { }
    public void ExitAttack() { }
}
