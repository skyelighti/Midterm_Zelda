using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase.Assets;
using System.Diagnostics;
namespace GameBase;

public class GameObject
{
    public Transform transform { get; }

    public bool IsActive { get; private set; } = true;
    //readonly mainly for pooling
    public string name { get; protected set; }
    // identifier 
    public Sprite sprite { get; protected set; }

    public Scene ownerScene { get; protected set; }
    public bool ActiveOnSceneEnter = true;
    public Layer layer { get; protected set; } = Layer.Default; 
    private readonly List<IComponent> components = new();


    public bool spriteVisible = true;
    float invisibilityTimer = 0f;
    public GameObject(string spriteName, Scene owner, string name = null)
    {
        this.name = name;
        sprite = Services.Sprites.CreateSprite(spriteName);
        ownerScene = owner;

        var trans = new TransformComponent(Vector2.Zero, 0, new Vector2(64, 64), this);
        transform = trans.transform;
        components.Add(trans);

    }


    public virtual void Update(GameTime gameTime)
    {
        //whereever handles updates should check if each gameobject is active before updating them
        //collision handler
        foreach (IComponent component in components)
        {
            if (component.IsActive)
            {
                component.Update(gameTime);
            }
        }

        if (!spriteVisible)
        {
            invisibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invisibilityTimer <= 0)
            {
                spriteVisible = true;
            }
        }
        if (!IsActive) { return; }
        sprite.Update(gameTime);
        //anims!!
    }
    public void UpdateName(string newName)
    {
        //updates gameobject name
        name = newName;
    }
    public void UpdateSprite(Sprite newSprite)
    {
        //updates gameobject sprite
        newSprite.transform.scale = sprite.transform.scale;
        sprite = newSprite;

    }
    public void UpdateLocationX(float x)
    {
        //updates gameobject location x direction
        transform.position = new Vector2(x, transform.position.Y);
        //reduces update calls? 
    }
    public void UpdateLocationY(float y)
    {
        //updates gameobject location y direction
        transform.position = new Vector2(transform.position.X, y);
    }
    public void UpdateLocation(float x, float y)
    {
        //updates gameobject location x and y direction
        transform.position = new Vector2(x, y);
    }
    public void UpdateSize(int x, int y)
    {
        transform.scale = new Vector2(x, y);
        //updates gameobject pixel size
        sprite.transform.scale.X = (float)x / sprite.Region.Width;
        sprite.transform.scale.Y = (float)y / sprite.Region.Height;

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive && spriteVisible)
        {
            sprite.Draw(spriteBatch, transform.position);
        }
        // first rect draws location and size, second rectangle is spritesheet location and size, color is tinting

        //should the object that inherits from this handle draw? or should the base handle?
    }

    public void SetActive(bool state)
    {
        IsActive = state;
    }
    public void HideFor(float seconds)
    {
        invisibilityTimer = seconds;
        spriteVisible = false;
    }
    public void AddComponent(IComponent c)
    {
        if (c.owner == this && !components.Contains(c))
        {
            components.Add(c);
        }
        else {
            Debug.WriteLine("component owner is not correct, or already added.");
        }
    }
    public T GetComponent<T>() where T : class
    {
        foreach (IComponent component in components)
        {
            if (component is T match)
            {
                return match;
            }
        }

        return null;
    }
    public List<T> GetComponents<T>() where T : class
    {
        List<T> c = new();
        foreach (IComponent component in components)
        {
            if (component is T match)
            {
                c.Add(match);
            }
        }

        return c;
    }
}
