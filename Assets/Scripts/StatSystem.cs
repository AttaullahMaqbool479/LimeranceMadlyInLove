using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem : MonoBehaviour
{
    public int HP;
    public int ATK;
    public float JMP;
    public float SPD;
    public Text HPtext;
    public Text ATKtext;
    public Text JMPtext;
    public Text SPDtext;

    void Reset()
    {
        var player = FindObjectOfType<PlayerController>();
        HP = player.GetMaxHP();
        ATK = player.GetATKforce();
        JMP = player.GetJMPforce();
        SPD = player.GetSpeed();
    }

    public void AddHP()
    {
        HP += 20;
        UpdateText();
    }

    public void AddATK()
    {
        ATK += 20;
        UpdateText();
    }

    public void AddJMP()
    {
        JMP += 0.1f;
        UpdateText();
    }

    public void AddSPD()
    {
        SPD += 0.1f;
        UpdateText();
    }

    public void UpdateText()
    {
        HPtext.text = "HEALTH: " + HP.ToString();
        ATKtext.text = "ATTACK: " + ATK.ToString();
        JMPtext.text = "JUMP: " + JMP.ToString("0.0");
        SPDtext.text = "SPEED: " + SPD.ToString("0.0");

        UpdateCharacter();
    }

    void Start()
    {
        Reset();
        HPtext.text = "HEALTH: " + HP.ToString();
        ATKtext.text = "ATTACK: " + ATK.ToString();
        JMPtext.text = "JUMP: " + JMP.ToString();
        SPDtext.text = "SPEED: " + SPD.ToString();
    }

    private void UpdateCharacter()
    {
        FindObjectOfType<PlayerController>().SetMaxHealth(HP);
        FindObjectOfType<PlayerController>().SetAttackForce(ATK);
        FindObjectOfType<PlayerController>().SetJumpForce(JMP);
        FindObjectOfType<PlayerController>().SetSpeed(SPD);
    }
}
