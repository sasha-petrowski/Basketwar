using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private Team _team;

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
        if (character.Team == _team)
        {
            Team otherTeam = (Team)(((int)_team + 1) % 2);
            GameManager.Instance.AddScore(1, otherTeam);

            if (GameManager.Instance.Overtime) 
            {
                GameManager.Instance.WinGame(otherTeam);
            }
        }
        character.OnGoal();
    }
}
