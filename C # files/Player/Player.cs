using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Job {
	public int Health;
	public int maxHealth;
	public int Mana;
	public int maxMana;
	public int Attack;
	public int Ranged;
	public int Magic;
	public int Defense;
	public int Speed;
	public string name;
}

public class Character {
	public Job job;
	public int Level = 1;
	public int Experience = 0;
	public int NextLevelExp = 100;
	public int LevelupPoints = 0;
	public List<Skill> skills = new List<Skill>();
	public bool leveledUp = false;
	public Status charStatus = Status.Normal;
	public GameObject charMesh;
}

public enum Status { Normal, Paralyzed, Sleep, Blind};

public class Player : MonoBehaviour {

	public Character player;

	public Job sunWarrior;
	public Job starMage;
	public Job skyRouge;

	void JobStats () {
		sunWarrior = new Job();
		sunWarrior.Health = 300;
		sunWarrior.Mana = 200;
		sunWarrior.Attack = 70;
		sunWarrior.Ranged = 40;
		sunWarrior.Magic = 20;
		sunWarrior.Defense = 50;
		sunWarrior.Speed = 45;
		sunWarrior.name = "Sun Warrior";

		starMage = new Job();
		starMage.Health = 200;
		starMage.Mana = 300;
		starMage.Attack= 30;
		starMage.Ranged = 30;
		starMage.Magic = 90;
		starMage.Defense = 20;
		starMage.Speed = 40;
		starMage.name = "Star Mage";

		skyRouge = new Job();
		skyRouge.Health = 250;
		skyRouge.Mana = 250;
		skyRouge.Attack = 40;
		skyRouge.Ranged = 50;
		skyRouge.Magic = 30;
		skyRouge.Defense = 30;
		skyRouge.Speed = 50;
		skyRouge.name = "Sky Rouge";
	}

	public void PlayerJob(jobNum j) {
		JobStats ();
		player = new Character();
		player.charMesh = GameObject.FindWithTag("Player");

		if(j == jobNum.SW)
		player.job = sunWarrior;
		if(j == jobNum.SM)
			player.job = starMage;
		if(j == jobNum.SR)
			player.job = skyRouge;

		player.job.maxHealth = player.job.Health;
		player.job.maxMana = player.job.Mana;
	}
	
	float distToGround;

	public Texture Bar;
	public Texture Health;
	public Texture Mana;
	public static bool PLAYERSET = false;

	public GUISkin KOSSkin;
		public enum jobNum {SW = 0,SM = 1,SR = 2};
		public jobNum jobNumber;

	public PlayerMove moveScript;
	public CameraScript camScript;
	public Actions ascript;
	public Battle bscript;
	public Inventory iscript;

	public List<Skill> starmageSkills = new List<Skill>();
	public List<Skill> sunwarriorSkills = new List<Skill>();
	public List<Skill> skyrougeSkills = new List<Skill>();

	private int guiLevelPoints = 0;

	void SetPlayer () {
		distToGround = collider.bounds.extents.y;
		JobStats ();
		PlayerJob(jobNumber);

		moveScript = GetComponent<PlayerMove>();
		camScript = GetComponent<CameraScript>();
		ascript = GameObject.FindWithTag("BattleManager").GetComponent<Actions>();
		bscript = GameObject.FindWithTag("BattleManager").GetComponent<Battle>();
		iscript = GetComponent<Inventory>();

		defineStarMageSkills();
		defineSunWarriorSkills();
		defineSkyRougeSkills();

		if(player.job == sunWarrior) {
			foreach(Skill s in sunwarriorSkills)
				player.skills.Add(s);
		}
		if(player.job == skyRouge) {
			foreach(Skill s in skyrougeSkills)
				player.skills.Add(s);
		}
		if(player.job == starMage) {
			foreach(Skill s in starmageSkills)
				player.skills.Add(s);
		}

		PLAYERSET = true;
	}

	void FixedUpdate () {
		if(!PLAYERSET) {
			SetPlayer();
		}
		if(!Battle.inBattle) {
		}
		DontDestroyOnLoad(gameObject);

		if(player.job.Health > player.job.maxHealth)
			player.job.Health = player.job.maxHealth;
		if(player.job.Mana > player.job.maxMana)
			player.job.Mana = player.job.maxMana;

		if(player.job.Health < 0)
			player.job.Health = 0;
		if(player.job.Mana < 0)
			player.job.Mana = 0;

		if(player.Experience >= player.NextLevelExp) {
			player.Experience -= player.NextLevelExp;
			player.Level ++;
			player.leveledUp = true;
		}
		LevelUp();
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void OnGUI() {
		GUI.skin = KOSSkin;

		if(player.job.Health >= 0) {
			//Player Experience
			GUI.DrawTexture( new Rect(10,Screen.height* 17/20, Screen.width/4 + 20 ,20), Bar);
			GUI.DrawTexture( new Rect(10,Screen.height* 17/20, Screen.width/4 * guiLevelPoints/(player.NextLevelExp*10) + 20,20), Mana);
			GUI.Label( new Rect(10,Screen.height* 17/20 - 5, Screen.width/4+ 20,40),"Experience");
			if(guiLevelPoints < player.Experience*10 && !Battle.inBattle)
				guiLevelPoints ++;
			if(player.Experience*10 < guiLevelPoints && !Battle.inBattle) {
				guiLevelPoints ++;
				if(guiLevelPoints >= 1000) {
					guiLevelPoints = 0;
					player.LevelupPoints += 5;
					player.job.maxHealth += 10;
					player.job.maxMana += 10;
					player.job.Health = player.job.maxHealth;
					player.job.Mana = player.job.maxMana;
				}
			}

			//Player Health
			GUI.DrawTexture( new Rect(10,Screen.height* 18/20, Screen.width/4 + 20 ,20), Bar);
			GUI.DrawTexture( new Rect(10,Screen.height* 18/20, Screen.width/4 * player.job.Health/player.job.maxHealth + 20,20),Health);
			GUI.Label( new Rect(10,Screen.height* 18/20 - 5, Screen.width/4+ 20,40),"Health: " + player.job.Health + "/" + player.job.maxHealth );

			//Player Mana
			GUI.DrawTexture( new Rect(10,Screen.height* 19/20, Screen.width/4 + 20,20), Bar);
			GUI.DrawTexture( new Rect(10,Screen.height* 19/20, Screen.width/4 * player.job.Mana/player.job.maxMana + 20,20), Mana);
			GUI.Label( new Rect(10,Screen.height* 19/20 - 5, Screen.width/4 + 20,40), "Mana: " + player.job.Mana + "/" + player.job.maxMana);
		}
	}

	public void LevelUp() {
		if(player.leveledUp == true) {
			Debug.Log("Leveled Up");
			player.leveledUp = false;
		}
	}

	//SKILLS
	void defineStarMageSkills() {
		Skill drain = new Skill();
		ascript.setSkill(drain,"Drain","Vampirically restore health and mana",2,10);
		starmageSkills.Add(drain);
		
		Skill sleep = new Skill();
		ascript.setSkill(sleep, "Sleep","Lull a single target to sleep",1,10);
		starmageSkills.Add(sleep);
		
		Skill release = new Skill();
		ascript.setSkill(release,"Release","Raze all enemies, raise all stats",1,50);
		starmageSkills.Add(release);
	}

	//STAR MAGE SKILL FUNCTIONS
	public void drain () {
		if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense)/2; 
			player.job.Health += ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense)/2;
			player.job.Mana += ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense)/2;
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense)/2;
			player.job.Health += ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense)/2;
			player.job.Mana += ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense)/2;
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense)/2;
			player.job.Health += ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense)/2;
			player.job.Mana += ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense)/2;
		}
		if(player.job.Health > player.job.maxHealth)
			player.job.Health = player.job.maxHealth;
		if(player.job.Mana > player.job.maxMana)
			player.job.Mana = player.job.maxMana;
	}

	public void sleep () {
		bscript.selectedVillian.enemy.enemyStatus = Status.Sleep;
	}

	public void release () {
		foreach(Villian v in bscript.villians) {
			if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense) * 3; 
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense) * 3;
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense) * 3;
			} 
		}
		player.job.Attack = (int) (player.job.Attack*1.05);
		player.job.Defense = (int) (player.job.Defense*1.05);
		player.job.Ranged = (int) (player.job.Ranged*1.05);
		player.job.Magic = (int) (player.job.Magic*1.05);
		player.job.Speed = (int) (player.job.Speed*1.05);
	}

	
	void defineSunWarriorSkills() {
		Skill flare = new Skill();
		ascript.setSkill(flare,"Sun Flare", "Blind all enemies",2,10);
		sunwarriorSkills.Add(flare);
		
		Skill nova = new Skill();
		ascript.setSkill(nova, "Nova", "Raise all stats",2,10);
		sunwarriorSkills.Add(nova);
		
		Skill gravity = new Skill();
		ascript.setSkill(gravity,"Gravity", "A devastating punch with stat lowering recoil",1,10);
		//5x Damage
		sunwarriorSkills.Add(gravity);
	}

	//SUN WARRIOR SKILL FUNCTIONS
	public void flare() {
		foreach(Villian v in bscript.villians){
			v.enemy.enemyStatus = Status.Blind;
			if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense)/2; 
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense)/2;
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
				v.enemy.Health -= ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense)/2;
			} 
		}
	}

	public void nova() {
		player.job.Attack = (int) (player.job.Attack*1.1);
		player.job.Defense = (int) (player.job.Defense*1.1);
		player.job.Ranged = (int) (player.job.Ranged*1.1);
		player.job.Magic = (int) (player.job.Magic*1.1);
		player.job.Speed = (int) (player.job.Speed*1.1);
	}

	public void gravity() {
		if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense) * 5; 
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense) * 5;
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense) * 5;
		} 
		player.job.Attack = (int) (player.job.Attack*0.95);
		player.job.Defense = (int) (player.job.Defense*0.95);
		player.job.Ranged = (int) (player.job.Ranged*0.95);
		player.job.Magic = (int) (player.job.Magic*0.95);
		player.job.Speed = (int) (player.job.Speed*0.95);
	}

	
	void defineSkyRougeSkills() {
		Skill fog = new Skill();
		ascript.setSkill(fog, "Fog", "Increase speed but lower defense",1,10);
		skyrougeSkills.Add(fog);
		
		Skill thunder = new Skill();
		ascript.setSkill(thunder, "Thunder", "Deal massive damage and paralyze a single target",2,10);
		skyrougeSkills.Add(thunder);
		
		Skill rainstorm = new Skill();
		ascript.setSkill(rainstorm,"Rain Storm", "Increase offense but lower defense",2,10);
		skyrougeSkills.Add(rainstorm);
	}

	//SKY ROUGE SKILL FUNCTIONS
	public void fog() {
		player.job.Speed = (int) (player.job.Speed*1.5);
		player.job.Defense = (int) (player.job.Defense*0.9);
	}

	public void thunder() {
		if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Attack,bscript.selectedVillian.enemy.Defense) * 3; 
			bscript.selectedVillian.enemy.enemyStatus = Status.Paralyzed;
			bscript.selectedVillian.paralyzeCounter += 3;
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Ranged,bscript.selectedVillian.enemy.Defense) * 3;
			bscript.selectedVillian.enemy.enemyStatus = Status.Paralyzed;
			bscript.selectedVillian.paralyzeCounter += 3;
		}
		if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
			bscript.selectedVillian.enemy.Health -= ascript.Attack.Attack (player.job.Magic,bscript.selectedVillian.enemy.Defense) * 3;
			bscript.selectedVillian.enemy.enemyStatus = Status.Paralyzed;
			bscript.selectedVillian.paralyzeCounter += 3;
		}
	}

	public void rainstorm() {
		player.job.Attack = (int) (player.job.Attack*1.2);
		player.job.Ranged = (int) (player.job.Ranged*1.2);
		player.job.Magic = (int) (player.job.Magic*1.2);
		player.job.Defense = (int) (player.job.Defense*0.95);
	}

	public void executeSkill(Job j, int skillIndex) {
		if(j == sunWarrior) {
			if(skillIndex == 0)
				flare();
			else if (skillIndex == 1)
				nova();
			else
				gravity();
		}
		if(j == skyRouge) {
			if(skillIndex == 0)
				fog ();
			else if (skillIndex == 1)
				thunder();
			else
				rainstorm();
		}
		if(j == starMage) {
			if(skillIndex == 0)
				drain();
			else if (skillIndex == 1)
				sleep ();
			else
				release();
		}
	}
}
