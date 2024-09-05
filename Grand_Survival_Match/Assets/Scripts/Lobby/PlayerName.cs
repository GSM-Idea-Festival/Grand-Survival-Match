using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//LobbyManager ���� �÷��̾��̸��� ĳ���͵�Ӵٿ� ���̺� �����ϱ� ���� �����������̳�
public class PlayerName : MonoBehaviour
{
    public Text nameText;
    public Text characterText;

    public void SetCharacter(int characterName)
    {
        switch (characterName)
        {
            case 0:
                GameManager.MyCharacterType = CharacterType.Knight;
                break;
            case 1:
                GameManager.MyCharacterType = CharacterType.Wizard;
                break;
            case 2:
                GameManager.MyCharacterType = CharacterType.SpearMan;
                break;
            case 3:
                GameManager.MyCharacterType = CharacterType.Assassin;
                break;
            case 4:
                GameManager.MyCharacterType = CharacterType.Assassin;
                break;

        }
    }
}
