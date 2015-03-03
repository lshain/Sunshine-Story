using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Hero {
	public Character player;
	public bool turn = false;
	public GameObject heroGameObject;
	public int position;
	public float turnGuage;
	public bool battleClone;
	public int initiative = 0;
	public int maxInitiative = 3;
	public bool frontRow;
}

public class Villian {
	public Enemy enemy;
	public bool turn = false;
	public int position;
	public float turnGuage;
	public bool frontRow;
	public int paralyzeCounter = 0;
}

public class Battle : MonoBehaviour {
	private Player pscript;
	public static bool inBattle = false;
	public int turnGuageMax;
	public GUISkin KOSSkin;
	
	public class Position {
		public int number;
		public int location;
		public Vector3 position;
		public string name;
		public int index;
	}
	public List<Position> battlePositions = new List<Position>();
	public void setPositions() {
		//creates the battle positions
		for(int i =0; i < 12; i++) {
			Position p = new Position();
			battlePositions.Add(p);
			p.index =  battlePositions.IndexOf(p);
		}
		//Sets the battle position locations
		int x = 0;
		for(int j =0; j < 4; j++) {
			for(int k =0; k < 3; k++) {
				battlePositions[x].position = new Vector3(j*5,1,k*5);
				x++;
			}
		}
	}
	
	public Hero mainChar = new Hero();
	public List<Enemy> enemies = new List<Enemy>();
	public List<Villian> villians = new List<Villian>();
	public List<Villian> defeatedVillians = new List<Villian>();
	public Villian noVillian = new Villian();
	
	private bool showSkills;
	private bool showItems;
	private Rect ActionWin =  new Rect(44, 37,170, 240);
	private Rect SkillWin = new Rect(235,37,175,300);
	private Rect ItemWin = new Rect(235,37,175,300);
	private Rect SpeedWin = new Rect(44,287,170,60);
	
	public Camera cam;
	
	private Actions ascript;
	private Inventory iscript;
	private AudioManager audscript;
	private PlayerMove pmscript;
	
	public Texture Bar;
	public Texture Health;
	public Texture Mana;
	public Texture Init;
	
	private bool defending = false;
	private bool attacked = false;
	private bool dadjusted = false;
	private int defenseI;
	private Job unalteredJobStats = new Job();
	public GameObject DefenseMesh;
	private GameObject TempDefMesh;
	
	public Villian selectedVillian;
	public GameObject KnockOut_X;
	private GameObject TempKnockOut_X;
	private List<GameObject> TempKO_X = new List<GameObject>();
	public GameObject Target_Arrow;
	private GameObject OldArrow;
	private GameObject NewArrow;

	private float battleSpeed = 1.0f;

	public GameObject enemyWorldObject;
	public bool destroyWorldObject;

	void setBattle () {
		mainChar.heroGameObject = new GameObject();
		mainChar.player = new Character();
		noVillian.enemy = new Enemy();
		noVillian.enemy.enemyGO = new GameObject();
		setPositions();
	}

	void findScripts() {
		pscript = GameObject.FindWithTag("Player").GetComponent<Player>();
		iscript = GameObject.FindWithTag("Player").GetComponent<Inventory>();
		pmscript = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
		ascript = GetComponent<Actions>();
		audscript = GetComponent<AudioManager>();
	}

	void spawnCharacter(int x, bool fr) {
		Destroy (mainChar.heroGameObject);
		mainChar.heroGameObject = GameObject.FindGameObjectWithTag("Player");
		GameObject clone = Instantiate(mainChar.heroGameObject, battlePositions[x].position, transform.rotation) as GameObject;
		clone.tag = "Untagged";
		Destroy (clone.GetComponent<Inventory>());
		Destroy (clone.GetComponent<Player>());
		Destroy (clone.GetComponent<PlayerMove>());
		Destroy (clone.GetComponent<CharacterController>());
		clone.name = mainChar.heroGameObject.name;
		clone.transform.rotation = Quaternion.LookRotation(Vector3.zero);
		clone.layer = 9;

		foreach(Transform child in clone.transform) {
			child.gameObject.layer = 9;
		}
		mainChar.heroGameObject = clone;
		mainChar.position = x;
		mainChar.frontRow = fr;
		//mainChar.heroGameObject.collider.isTrigger = true;
	}

	public void AddEnemies(Villian v) {
		enemies.Add(v.enemy);
		v.enemy.index = enemies.IndexOf(v.enemy);
		villians.Add (v);
	}

	public void EnterBattle(GameObject enemy) {
		Menu.GAME_PAUSED = true;
		spawnCharacter(1, false); //!frontrow
		placeEnemy(villians[0], enemy, 7);
		selectedVillian = villians[0];

		if(villians[0].enemy.supportEnemies) {
			GameObject support1 = new GameObject();
			support1 = Resources.Load ("Enemies/Support/" + enemy.name + " support 1") as GameObject;
			Enemies escript = support1.GetComponent<Enemies>();
			escript.SetEnemyStats();
			AddEnemies(escript.thisVillian);
			placeEnemy (villians[1], support1, 9);

			GameObject support2 = new GameObject();
			support2 = Resources.Load ("Enemies/Support/" + enemy.name + " support 2") as GameObject;
			Enemies escript2 = support2.GetComponent<Enemies>();
			escript2.SetEnemyStats();
			AddEnemies(escript2.thisVillian);
			placeEnemy (villians[2], support2, 11);
		}


		mainChar.player = pscript.player;
		unalteredJobStats.Attack = pscript.player.job.Attack;
		unalteredJobStats.Ranged = pscript.player.job.Ranged;
		unalteredJobStats.Magic = pscript.player.job.Magic;
		unalteredJobStats.Defense = pscript.player.job.Defense;
		unalteredJobStats.Speed = pscript.player.job.Speed;
		inBattle = true;
		cam.cullingMask = (1 << LayerMask.NameToLayer("Battle"));
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().enabled = false;
		cam.GetComponent<CameraScript>().enabled = false;
		//SET CAMERA POSITION FOR BATTLE
		cam.transform.position = new Vector3(-16f,8.071196f,0f);
		cam.transform.localEulerAngles = new Vector3(22.89324f,90f,0f);
		if(Enemies.BATTLING_BOSS1 || Enemies.BATTLING_BOSS2)
			audscript.BossMusic();
		else 
			audscript.BattleMusic();

		TargetingArrow();
	}
	
	public void placeEnemy(Villian v, GameObject enemy, int i) {
		v.enemy.enemyGO = enemy;
		GameObject clone = Instantiate(enemy, battlePositions[i].position, transform.rotation) as GameObject;
		clone.name = v.enemy.enemyGO.name;
		clone.layer = 9;
		clone.transform.GetChild(1).gameObject.layer = 9;
		v.enemy.enemyGO = clone;
		v.position = i;
	}

	void OnGUI() {
		GUI.skin = KOSSkin;
		if(GameState.STATE == GameState.gameState.gameover) {
			GUI.Box(new Rect(Screen.width * 7/16, Screen.height * 6/16, Screen.width * 1/8, Screen.height * 1/16),"GAME OVER");
			//Draw texture for game over
			if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 7/16, Screen.width * 1/8, Screen.height * 1/16),"Continue?")) {
				ExitBattle();
				inBattle = false;
				GameObject.FindWithTag("Player").transform.position = AreaSwitch.LAST_SPAWN;
				pscript.player.job.Health = pscript.player.job.maxHealth * 9/10;
				pscript.player.job.Mana = pscript.player.job.maxMana * 9/10;
				audscript.WorldMusic();
				battleSpeed = 1.0f;
				GameState.STATE = GameState.gameState.game;
				//Save health at on enter?
			}
			if(GUI.Button (new Rect(Screen.width * 7/16, Screen.height * 8/16, Screen.width * 1/8, Screen.height * 1/16),"Quit?")) {
				Application.Quit();
			}
		}
		if(inBattle) {
			ActionWin = GUI.Window(3, ActionWin, actionWin, "ACTIONS");
			SpeedWin = GUI.Window (6,SpeedWin,speedWin,"BATTLE SPEED");

			GUI.Box(new Rect(10, Screen.height* 16/20 - 20, 250, 40)," ");
			GUI.Box(new Rect(10, Screen.height* 16/20 - 20, 90, 40),"Initiative:");
			for(int i = 0; i < mainChar.initiative; i++) {
				GUI.DrawTexture( new Rect(110 + i*50,Screen.height* 16/20 - 10, 40,20),Init);
			}

			GUI.DrawTexture( new Rect(Screen.width*3/4 - 40,Screen.height* 18/20, Screen.width/4 + 20 ,20), Bar);
			GUI.DrawTexture( new Rect(Screen.width*3/4 - 40,Screen.height* 18/20, Screen.width/4 * selectedVillian.enemy.Health/selectedVillian.enemy.MaxHealth + 20,20),Health);

			GUI.DrawTexture( new Rect(10,Screen.height* 17/20, Screen.width/4 + 20 ,20), Bar);
			GUI.DrawTexture( new Rect(10,Screen.height* 17/20, Screen.width/4 * mainChar.turnGuage/100 + 20,20),Init);

			GUI.DrawTexture( new Rect(Screen.width*3/4 - 40,Screen.height* 17/20, Screen.width/4 + 20 ,20), Bar);
			GUI.DrawTexture( new Rect(Screen.width*3/4 - 40,Screen.height* 17/20, Screen.width/4 * selectedVillian.turnGuage/100 + 20,20),Init);

			GUI.Box (new Rect(Screen.width * 3/4 - 40, Screen.height * 16/20 - 20, Screen.width/16, 40), "A");
			GUI.Box (new Rect(Screen.width * 13/16 - 30, Screen.height * 16/20 - 20, Screen.width/8, 40), selectedVillian.enemy.name);
			GUI.Box (new Rect(Screen.width * 15/16 - 20, Screen.height * 16/20 - 20, Screen.width/16, 40), "D");

			GUI.Box( new Rect(Screen.width*3/4 - 40,Screen.height* 19/20, Screen.width/4 + 20 ,30), "Status: " + selectedVillian.enemy.enemyStatus.ToString());;

			if(showSkills) {
				SkillWin = GUI.Window(4,SkillWin,skillWin, "SKILLS");
			}
			if(showItems) {
				ItemWin = GUI.Window (5,ItemWin,itemWin, "ITEMS");
			}

		}
	}
	void actionWin(int windowID) {

		if(GUI.Button(new Rect(10, 25,150,30), "Attack") && GameState.STATE != GameState.gameState.gameover) {
			bool pa = false;
			int i = RowDamageModifier(pa,selectedVillian);
			if(mainChar.initiative > 0) {
			if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
			selectedVillian.enemy.Health -= ascript.Attack.Attack (mainChar.player.job.Attack,selectedVillian.enemy.Defense)/i; 
				}
			if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
				selectedVillian.enemy.Health -= ascript.Attack.Attack (mainChar.player.job.Ranged,selectedVillian.enemy.Defense)/i; 
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
				selectedVillian.enemy.Health -= ascript.Attack.Attack (mainChar.player.job.Magic,selectedVillian.enemy.Defense)/i; 
			}
			mainChar.initiative --;
			}
		}

		if(GUI.Button(new Rect(10, 60,150, 30), "Defend") && GameState.STATE != GameState.gameState.gameover) {
			if(!defending && mainChar.initiative > 0) {
				defending = true;
				mainChar.initiative--;
				TempDefMesh = Instantiate(DefenseMesh) as GameObject;
				TempDefMesh.transform.position = new Vector3(mainChar.heroGameObject.transform.position.x + 2,
				                                             mainChar.heroGameObject.transform.position.y,
				                                             mainChar.heroGameObject.transform.position.z);
			}
		}

		string advret = " ";
		if(mainChar.position == 1)
			advret = "Advance";
		if(mainChar.position == 4)
			advret = "Retreat";
		if(GUI.Button(new Rect(10, 95,150,30), advret) && GameState.STATE != GameState.gameState.gameover) {
			if(mainChar.initiative > 0) {
				if(mainChar.position == 1) {
					mainChar.heroGameObject.transform.position = battlePositions[4].position;
					mainChar.position = 4;
					mainChar.initiative--;
					mainChar.frontRow = true;
				}
				else if(mainChar.position == 4) {
					mainChar.heroGameObject.transform.position = battlePositions[1].position;
					mainChar.position = 1;
					mainChar.initiative--;
					mainChar.frontRow = false;
				}
			}
		}

		if(GUI.Button(new Rect(10, 130,150, 30), "Skills") && GameState.STATE != GameState.gameState.gameover) {
			showSkills = !showSkills;
			showItems = false;
		}

		if(GUI.Button(new Rect(10, 165,150, 30), "Items") && GameState.STATE != GameState.gameState.gameover) {
			showItems = !showItems;
			showSkills = false;
		}

		if(GUI.Button(new Rect(10, 200,150, 30), "Flee") && GameState.STATE != GameState.gameState.gameover) {
			if(mainChar.initiative >= 1) {
				ExitBattle();
				mainChar.player.charMesh.transform.position -= mainChar.player.charMesh.transform.forward * 3;
			}
		}
	}
	void skillWin(int windowID) {
		for(int i = 0; i < pscript.player.skills.Count; i++) {
			if(GUI.Button(new Rect(10,25+i*100,155,30), pscript.player.skills[i].name)) {
				if(mainChar.initiative >= pscript.player.skills[i].initiativeCost &&
				   mainChar.player.job.Mana >= pscript.player.skills[i].manaCost) {
					pscript.executeSkill(mainChar.player.job, i);
					mainChar.player.job.Mana -= pscript.player.skills[i].manaCost;
					mainChar.initiative -= pscript.player.skills[i].initiativeCost;
				}
			}
			GUI.Box(new Rect(10,60+i*100,75,30), pscript.player.skills[i].initiativeCost + " Initiative");
			GUI.Box(new Rect(90,60+i*100,75,30), pscript.player.skills[i].manaCost + " Mana");
		}
	}
	void itemWin(int windowID) {
		for(int i = 0; i < iscript.consumables.Count; i++) {
			if(GUI.Button(new Rect(10,25+i*30,155,25), iscript.consumables[i].consItem.name)) {
				iscript.selectedItem = iscript.consumables[i].consItem;
				iscript.UseConsumables();
			}
		}
	}
	void speedWin(int windowID) {
		if(battleSpeed != 0.5f) {
			if(GUI.Button (new Rect(10,25,45,25),"1/2x"))
			   battleSpeed = 0.5f;
		}
		if(battleSpeed != 1.0f) {
			if(GUI.Button (new Rect(60,25,45,25),"1x"))
				battleSpeed = 1.0f;
		}
		if(battleSpeed != 2.0f) {
			if(GUI.Button (new Rect(110,25,45,25),"2x"))
				battleSpeed = 2.0f;
		}

		if(battleSpeed == 0.5f) {
			GUI.Box (new Rect(10,22.5f,45,30),"1/2x");
		}
		if(battleSpeed == 1.0f) {
			GUI.Box (new Rect(60,22.5f,45,30),"1x");
		}
		if(battleSpeed == 2.0f) {
			GUI.Box (new Rect(110,22.5f,45,30),"2x");
		}
	}
	
	void Update () {
		DontDestroyOnLoad(gameObject);
		if(pscript == null) {
			findScripts();
			setBattle();
		}

		if(inBattle && GameState.STATE != GameState.gameState.gameover) {
			InitiativeCheck();
			Targeting();

			//Villians Attack
			foreach( Villian v in villians) {
				if(v.turnGuage < 100) {
					v.turnGuage += v.enemy.Speed * Time.deltaTime * battleSpeed;
				}
				if(v.turnGuage >= 100) {
					if(v.enemy.enemyStatus == Status.Paralyzed && v.paralyzeCounter == 0) {
						v.enemy.enemyStatus = Status.Normal;
						v.turnGuage = 0;
					}

					else if(v.enemy.enemyStatus == Status.Blind) {
						int blindroll = Random.Range(0,10);
						if(blindroll < 5) {
							Debug.Log ("Blind Attack");
							v.turn = true;
							bool ea = true;
							int i = RowDamageModifier(ea,v);
							//Enemy Actions
							mainChar.player.job.Health -= ascript.Attack.Attack(v.enemy.Damage, mainChar.player.job.Defense)/i;
							v.turnGuage = 0;
							
							if(defending) {
								attacked = true;
								Destroy (TempDefMesh);
							}
						}
						else {
							Debug.Log("Blind Miss");
							v.turnGuage = 0;
						}
					}
					else if(v.enemy.enemyStatus == Status.Sleep) {
						int sleeproll = Random.Range(0,10);
						if(sleeproll < 4) {
							v.enemy.enemyStatus = Status.Normal;
						}
						v.turnGuage = 0;
					}
					if(v.paralyzeCounter == 0 && v.enemy.enemyStatus == Status.Normal) {
						v.turn = true;
						bool ea = true;
						int i = RowDamageModifier(ea,v);
						//Enemy Actions
						mainChar.player.job.Health -= ascript.Attack.Attack(v.enemy.Damage, mainChar.player.job.Defense)/i;
						v.turnGuage = 0;
						
						if(defending) {
							attacked = true;
							Destroy (TempDefMesh);
						}
					}

					if(v.enemy.enemyStatus == Status.Paralyzed && v.paralyzeCounter > 0) {
						v.paralyzeCounter -= 1;
						v.turnGuage = 0;
					}
				}
				v.enemy.enemyGO.transform.LookAt(mainChar.heroGameObject.transform);
			}


			//Main Character Initiative
			if(defending) {
					PDefend();
				}

			if(mainChar.turnGuage < 100 && mainChar.initiative < mainChar.maxInitiative) {
				mainChar.turnGuage += pscript.player.job.Speed * Time.deltaTime * battleSpeed;
				}
			if(mainChar.turnGuage >= 100) {
				if(mainChar.initiative < mainChar.maxInitiative) {
					mainChar.turn = true;
					mainChar.initiative++;
					mainChar.turnGuage = 0;
				}
			}

			//Player health
			if(mainChar.player.job.Health <= 0 && GameState.STATE != GameState.gameState.gameover) {
				if(!Enemies.BATTLING_BOSS2) {
					battleSpeed = 0.0f;
					audscript.GameOverMusic();
					GameState.STATE = GameState.gameState.gameover;
				}
				else if(Enemies.BATTLING_BOSS2) {
					Battle.inBattle = false;
					ScreenFade.FADING = true;
					GameState.STATE = GameState.gameState.ending2;
					AudioManager.ENDING_MUSIC_CHANGE = true;
				}
			}

			//Vilian health
			List<Villian> villiansToRemove = new List<Villian>();
			foreach(Villian v in villians) {
				if(v.enemy.Health <= 0) {
					v.enemy.Health = 0;
						defeatedVillians.Add(v);
						villiansToRemove.Add(v);
						TempKnockOut_X = Instantiate(KnockOut_X) as GameObject;
						TempKO_X.Add(TempKnockOut_X);
						TempKnockOut_X.transform.position = new Vector3 (v.enemy.enemyGO.transform.position.x,
					                                            	 	 v.enemy.enemyGO.transform.position.y + 5,
					                                         		 	 v.enemy.enemyGO.transform.position.z);
					}
				}
			foreach(Villian v in villiansToRemove) {
					villians.Remove(v);
					if(villians.Count >= 1) {
						selectedVillian = villians[0];
						TargetingArrow();
					}
				}
			if(villians.Count > 0 && selectedVillian == null) {
				selectedVillian = villians[0];
				TargetingArrow();
			}
			if(villians.Count == 0) {
					foreach(Villian v in defeatedVillians) {
					v.enemy.battled = true;
					}
					Reward();
					ExitBattle();
				}

			mainChar.heroGameObject.transform.LookAt(selectedVillian.enemy.enemyGO.transform);
		}
	}

	void Targeting() {
		if(villians.Count == 3) {
			if(Input.GetKeyUp(KeyCode.A)) {
				if(selectedVillian == villians[0])
					selectedVillian = villians[2];
				else if(selectedVillian == villians[1])
					selectedVillian = villians[0];
				else if(selectedVillian == villians[2])
					selectedVillian = villians[1];

				TargetingArrow();
			}
			if(Input.GetKeyUp(KeyCode.D)) {
				if(selectedVillian == villians[0])
					selectedVillian = villians[1];
				else if(selectedVillian == villians[2])
					selectedVillian = villians[0];
				else if(selectedVillian == villians[1])
					selectedVillian = villians[2];

				TargetingArrow();
			}
		}
		if(villians.Count == 2) {
			if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
				if(selectedVillian == villians[1])
					selectedVillian = villians[0];
				else if(selectedVillian == villians[0])
					selectedVillian = villians[1];

				TargetingArrow();
			}
		}
	}

	private void TargetingArrow() {
		OldArrow = NewArrow;
		NewArrow = Instantiate(Target_Arrow) as GameObject;
		NewArrow.transform.position = 	new Vector3 (selectedVillian.enemy.enemyGO.transform.position.x,
		                        					 selectedVillian.enemy.enemyGO.transform.position.y + 5,
		                        					 selectedVillian.enemy.enemyGO.transform.position.z);
		Destroy(OldArrow);
	}

	int RowDamageModifier (bool enemyAttack, Villian v) {
		int i = 1;

		if(enemyAttack) {
			if(v.enemy.etype == Enemy.type.Melee) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 3;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 2;
				}
				else {
					i = 1;
				}
			}
			if(v.enemy.etype == Enemy.type.Ranged) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 1;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 2;
				}
				else {
					i = 3;
				}
			}
			if(v.enemy.etype == Enemy.type.Magic) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 2;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 1;
				}
				else {
					i = 2;
				}
			}
		}

		else {
			if(iscript.equipedWeapon.wtype == Weapon.type.Melee) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 3;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 2;
				}
				else {
					i = 1;
				}
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Ranged) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 1;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 2;
				}
				else {
					i = 3;
				}
			}
			if(iscript.equipedWeapon.wtype == Weapon.type.Magic) {
				if(!mainChar.frontRow && !v.frontRow) { 
					i = 2;
				}
				else if(!mainChar.frontRow || !v.frontRow) {
					i = 1;
				}
				else {
					i = 2;
				}
			}
		}
		return i;
	}

	void ExitBattle() {
		Menu.GAME_PAUSED = false;
		pscript.player.job.Attack = unalteredJobStats.Attack;
		pscript.player.job.Ranged = unalteredJobStats.Ranged;
		pscript.player.job.Magic = unalteredJobStats.Magic;
		pscript.player.job.Defense = unalteredJobStats.Defense;
		pscript.player.job.Speed = unalteredJobStats.Speed;
		//battleSpeed = 1.0f;
		defending = false;
		inBattle = false;
		cam.cullingMask = (1 << LayerMask.NameToLayer("World"));
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().enabled = true;
		cam.GetComponent<CameraScript>().enabled = true;
		mainChar.initiative = 0;
		mainChar.turnGuage = 0;
		GameObject.FindWithTag("MainCamera").transform.position = new Vector3(pscript.player.charMesh.transform.position.x,
		                                                                      pscript.player.charMesh.transform.position.y + 10,
		                                                                      pscript.player.charMesh.transform.position.z);

		showItems = false;
		showSkills = false;
	
		foreach(Villian v in defeatedVillians) {
		Destroy(v.enemy.enemyGO);
		}
		foreach(Villian v in villians) {
			Destroy (v.enemy.enemyGO);
		}

		Destroy(NewArrow);
		Destroy (OldArrow);
		Destroy(TempDefMesh);
		foreach(GameObject g in TempKO_X) {
			Destroy (g);
		}

		villians.Clear();
		defeatedVillians.Clear ();
		audscript.WorldMusic();
	}

	void Reward() {
		foreach(Villian v in defeatedVillians) {
		mainChar.player.Experience += mainChar.player.NextLevelExp*v.enemy.Level/mainChar.player.Level/5;
		}
		if(destroyWorldObject) {
			Destroy (enemyWorldObject);
		}
		//MANAGE BOSS DEFEAT
		if(Enemies.BATTLING_BOSS1) {
			Event1.BOSS1_DEFEATED = true;
			Enemies.BATTLING_BOSS1 = false;
		}
		if(Enemies.BATTLING_BOSS2) {
			ScreenFade.FADING = true;
			Enemies.BATTLING_BOSS2 = false;
			GameState.STATE = GameState.gameState.ending3;
			AudioManager.ENDING_MUSIC_CHANGE = true;
		}
	}
	
	void PDefend() {
		if(!dadjusted) {
			defenseI = mainChar.player.job.Defense;
			mainChar.player.job.Defense *= 2;
			dadjusted = true;
			}
		else if(attacked) {
			mainChar.player.job.Defense = defenseI;
			defending = false;
			dadjusted = false;
			attacked = false;
			if(mainChar.initiative+2 > mainChar.maxInitiative) {
				mainChar.initiative = mainChar.maxInitiative;
			}
			else {
				mainChar.initiative += 2;
			}
		}
	}

	void InitiativeCheck() {
		if (mainChar.initiative > mainChar.maxInitiative) {
			mainChar.initiative = mainChar.maxInitiative;
		}
	}
}
