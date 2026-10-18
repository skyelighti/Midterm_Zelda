
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameBase.Dialogue;

public class NPC : GameObject, IInteractable
{
    //other interfaces like iinteractable can also be implemented here

    public bool CanInteract { get; protected set; } = true;
    public string ID => name;

    public Character character = Character.NPC1;
    readonly List<DialogueInfo> dialogueInfoList;
    public HitboxComponent hitbox;


    public NPC(string spriteName, Scene owner, NPCInfo info, string name = null) : base(spriteName, owner, name)
    {
        //update sprites, animated sprites and then size in here
        character = info.character;
        dialogueInfoList = new();
        layer = Layer.Interactable;

        var hb = new HitboxComponent(GetComponent<TransformComponent>(), this);
        hitbox = hb;
        AddComponent(hb);
        hb.CollisionStay += OnCollision;
        hb.CollisionEnter += OnCollisionEnter;
        hb.CollisionExit += OnCollisionExit;

        foreach (string id in info.dialogueIDOrder)
        {
            dialogueInfoList.Add(Services.GameDef.dialogueById[id]);
        }

    }

    public void OnCollision(ICollidable col)
    {
    }
    public void OnCollisionEnter(ICollidable col)
    {
    }
    public void OnCollisionExit(ICollidable col)
    {
    }

    public void Interact(GameObject player)
    {
        if (CanInteract)
        {
            EventSystem.NPCInteract(character);
            EventSystem.Interact(ID);
            int index = Services.GamePersistence.GetDialogueIndex(character);
            if (index < dialogueInfoList.Count)
            {
                Services.Dialogue.DialogueSetup(dialogueInfoList[index]);
            }
        }
    }

}
