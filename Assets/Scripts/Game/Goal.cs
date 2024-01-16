using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Team Team;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out CharacterReference reference))
        {
            if (reference.Character.TryGetComponent(out CharacterGrab grab) && grab.Grabbed != null)
            {
                ScoreCharacter(grab.Grabbed);
            }

            ScoreCharacter(reference.Character);

        }
    }

    private void ScoreCharacter(Character character)
    {
        if (character.Team == Team)
        {
            GameManager.Instance.AddScore(1, Team);
        }
        character.OnGoal();
    }
}
