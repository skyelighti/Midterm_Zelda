using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public partial class Player : GameObject
{
    enum PlayerAnimationStates { 
        Idle,
        Moving,
        Attacking
    
    }
    public HitboxComponent hitbox;
    public MovementComponent moveC;
    public int speed { get; private set; } = 100;
    StateMachine animState = new();
    public Player(string spriteName, Scene owner, string name = null) : base(spriteName, owner, name)
    {
        animState.SetNumOfStates(3);
        animState.SetStateFunctions((int)PlayerAnimationStates.Idle, EnterIdle, UpdateIdle, ExitIdle);
        animState.SetStateFunctions((int)PlayerAnimationStates.Moving, EnterMoving, UpdateMoving, ExitMoving);
        animState.SetStateFunctions((int)PlayerAnimationStates.Attacking, EnterAttack, UpdateAttack, ExitAttack);

        AnimatedSprite s = Services.Sprites.CreateAnimatedSprite("placeholder", "placeholder");
        spriteC.UpdateSprite(s);

        hitbox = new HitboxComponent(transformC, this);
        hitbox.CollisionEnter += OnCollisionEnter;
        hitbox.CollisionStay += OnCollisionStay;
        hitbox.CollisionExit += OnCollisionExit;
        AddComponent(hitbox);

        moveC = new PlayerMovementComponent(speed, this);
        AddComponent(moveC);

        Services.Input.InputMapChanged += OnInputMapChanged;
        Services.Input.ActionKeyDown += OnKeyDown;
        Services.Input.ActionPressed += OnKeyPressed;
        Services.Input.ActionReleased += OnKeyReleased;

    }
    public override void Update(GameTime gameTime)
    {
        animState.UpdateState(gameTime);
        base.Update(gameTime);
    }

    public void OnCollisionEnter(ICollidable col) { 
    
    }

    public void OnCollisionStay(ICollidable col) { 
    
    }

    public void OnCollisionExit(ICollidable col) { 
    
    }
    public void OnKeyDown(GameAction action) { 
    
    }

    public void OnKeyPressed(GameAction action) {
    }

    public void OnKeyReleased(GameAction action) { 
    
    }

    public void OnInputMapChanged(InputActionMap map) {
        if (map != InputActionMap.Gameplay) {
            //diable anything thats gameplay only
            moveC.SetActive(false);
            //theoretically should still be able to access values
        }
    }
}
