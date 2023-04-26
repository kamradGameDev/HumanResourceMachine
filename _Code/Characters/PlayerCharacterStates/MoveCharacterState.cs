using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character/MoveStateCharacter", fileName = "NewMoveStateChacter", order = 57)]

internal class MoveCharacterState : StateCharacterBase
{
    public Transform TargetMovable { get; private set; }

    [SerializeField] private float speedMove;

    internal override void Init<T>(T target)
    {
        //на данный момент целями двидения могут быть только предметы, поэтому это прописано так
        ItemBase targetMove =  target as ItemBase;

        TargetMovable = targetMove.transform.Find("TargetMove");
    }

    internal override bool Process(CharacterBase character)
    {

        if(Vector2.Distance(character.transform.position, TargetMovable.position) <= .1f)
            return true;

        Vector3 direction = TargetMovable.transform.position - character.transform.position;

        character.transform.Translate(direction * speedMove * ScrollSpeedGame.instance.GetGameSpeed() * Time.fixedDeltaTime);
        //direction = direction.normalized * speedMove * Time.fixedDeltaTime;
        //character.Rb2D.MovePosition(character.transform.position + direction);    
        return false;
    }
}
