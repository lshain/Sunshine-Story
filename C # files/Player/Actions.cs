using UnityEngine;
using System.Collections;

public class Skill {
	public int damage;
	public string name;
	public string description;
	public int initiativeCost = 0;
	public int manaCost = 0;
	public bool unlocked = false;
	public int unlockLevel;
	
	private bool adjusted = false;
	private int defenseI = 0;
	
	public int Attack(int attack, int defense) {
		int damage = attack - defense;
		if(damage < 5) {
			damage = 5;
		}
		return damage;
	}

	public void Defend(bool defending, bool attacked, int defense) {
		while(!attacked) {
			if(defending && !adjusted) {
				defenseI = defense;
				int defenseD = defense + defense/2;
				defense = defenseD;
			}
			if(!defending) {
				defense = defenseI;
			}
			else if(adjusted && attacked) {
				defense = defenseI;
				defending = false;
			}
		}
	}
}

public class Action {
	public Skill skill;
	public bool perform;
}

public class Actions : MonoBehaviour {

	public Skill Attack = new Skill();
	public Skill Defend = new Skill();

	public void setSkill(Skill s, string n, string d, int icost, int mcost) {
		s.name = n;
		s.description = d;
		s.initiativeCost = icost;
		s.manaCost = mcost;
	}
	
	void Start () {
		setSkill (Attack,"Attack","A basic attack",0,0);
		setSkill (Defend, "Defend", "Guard against an enemy attack",0,0);
	}

	void Update () {
	
	}
}
