using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Items;
public abstract class Item : GameObject, IInteractable
{
    public ItemInfo Info { get; protected set; }
    public string ID { get; protected set; }

    public bool CanInteract { get; protected set; } = true;

    public HitboxComponent hitbox;


    //should i have a data container for items? like a scriptable object? 


    public Item(string spriteName, Scene owner, string name = null) : base(spriteName, owner, name)
    {
        layer = Layer.Interactable;

        var hb = new HitboxComponent(GetComponent<TransformComponent>(), this);
        hitbox = hb;
        AddComponent(hb);
        hb.CollisionStay += OnCollision;
        hb.CollisionEnter += OnCollisionEnter;
        hb.CollisionExit += OnCollisionExit;
    }

    public abstract void Interact(GameObject player);

    public virtual void OnCollision(ICollidable col)
    {
    }
    public virtual void OnCollisionEnter(ICollidable col)
    {
    }
    public virtual void OnCollisionExit(ICollidable col)
    {
    }

}
