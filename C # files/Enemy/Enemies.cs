using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy {
	public int Health;
	public int MaxHealth;
	public int Damage;
	public int Speed;
	public int Defense;
	public string name;
	public int index;
	public int Level;
	public GameObject enemyGO;
	public bool battled;
	public Status enemyStatus = Status.Normal;

	public enum type {Melee, Ranged, Magic};
	public type etype;
	public bool supportEnemies;
}

public class Enemies : MonoBehaviour {
	private Battle bscript;
	public Enemy thisEnemy = new Enemy();
	public int Level = 1;

	public Villian thisVillian = new Villian();
	public EnemyStats escript;
	public bool destroyOnDefeat = true;
	public enum enemyList {genericEnemy, Boss1, Boss2};
	public enemyList enemyType = enemyList.genericEnemy;

	public static bool BATTLING_BOSS1 = false;
	public static bool BATTLING_BOSS2 = false;
	
	void Start () {
		SetEnemyStats();

		if(escript.isMelee) {
			thisEnemy.etype = Enemy.type.Melee;
		}
		if(escript.isMagic) {
			thisEnemy.etype = Enemy.type.Magic;
		}
		if(escript.isRanged) {
			thisEnemy.etype = Enemy.type.Ranged;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.collider.gameObject.CompareTag("Player") && !Battle.inBattle && !thisEnemy.battled) {
			if(enemyType == enemyList.Boss1) {
				BATTLING_BOSS1 = true;
			}
			else if(enemyType == enemyList.Boss2) {
				BATTLING_BOSS2 = true;
			}
			else
				BATTLING_BOSS1 = false;
			bscript.enemyWorldObject = gameObject;
			bscript.destroyWorldObject = destroyOnDefeat;

			bscript.AddEnemies(thisVillian);
			bscript.EnterBattle(gameObject);
		}
	}

	public void SetEnemyStats() {
		escript = GetComponent<EnemyStats>();
		bscript = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<Battle>();
		thisVillian.enemy = thisEnemy;
		thisEnemy.enemyGO = (GameObject)Resources.Load ("Enemies/"+gameObject.name);
		thisEnemy.name = gameObject.name;
		thisEnemy.Health = escript.Health;
		thisEnemy.MaxHealth = escript.Health;
		thisEnemy.Speed = escript.Speed;
		thisEnemy.Damage = escript.Damage;
		thisEnemy.Defense = escript.Defense;
		thisEnemy.Level = Level;
		thisEnemy.supportEnemies = escript.hasSupport;
	}
}
