using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item {
	public string name;
	public string description;
	public int stack;
	public Texture picture;
	public bool isWeapon = false;
	public bool isEquipement = false;
	public bool isConsumable = false;
	public int weaponLocation;
	public int equipLocation;
	public GameObject itemGameObject;
	public int consLocation;
}

public class Weapon {
	public Item wepItem;
	public enum type { None = 0, Melee = 1, Ranged = 2, Magic = 3 };
	public type wtype;
	public bool equipped = false;
	public int wepAttack = 0;
	public int wepRange = 0;
	public int wepMagic = 0;
}

public class Equipment {
	public Item eItem;
	public int slot;
	public bool equipped = false;
	public int eDefense = 0;
	public int eSpeed = 0;
	public int eMana = 0;
	public int eHealth = 0;
}

public class Consumable {
	public Item consItem;
	public enum type {Potion, Cure};
	public enum potion {HPotion, MPotion};
	public enum cure {Poison, Bleeding, Burning};
	public type ctype;
	public potion cpotion;
	public cure ccure;

	public int effect;
	public int Potion (int health, int heal) {
		int i =0;
		i = health + heal;
		return i;
	}
}

public class Inventory : MonoBehaviour {
	public Player pscript;
	Rect invWin = new Rect(20, 20, 350, 500);
	Rect bagWin = new Rect(390, 20, 580, 500);
	Rect charWin = new Rect(990, 20, 350, 500);
	Rect skillsWin = new Rect(20, 20, 350, 500);
	public GUISkin skin;
	private bool showInvWin = false;
	private bool showCharWin = false;
	private bool showSkillWin = false;
	public Texture tex;


	public List<Item> inv = new List<Item>();
	public Item selectedItem;
	public Item noItem = new Item();

	public List<Weapon> weps = new List<Weapon>();
	public Weapon equipedWeapon;
	public Weapon noWeapon = new Weapon();

	public List<Equipment> equipment = new List<Equipment>();
	public List<Equipment> equipedE = new List<Equipment>();
	public Equipment noEquipment = new Equipment();

	public List<Consumable> consumables = new List<Consumable>();


	private bool showWeps = true;
	private bool showEquip = false;
	private bool showCons = false;
	public bool added = false;

	public static bool inInventory;

	public void AddItem(Item i, Weapon w, Equipment e, GameObject g, Consumable c) {
		if(i.isWeapon && weps.Count < 5) {
			inv.Add(i);
			i.itemGameObject = g.gameObject;
			added = true;
			if(weps.Count == 0 && equipment.Count == 0 && consumables.Count == 0) {
				showWeps = true;
				showEquip = false;
				showCons = false;
			}
			weps.Add(w);
			i.weaponLocation = weps.IndexOf(w);
		}
		else if(i.isEquipement && equipment.Count < 5) {
			inv.Add(i);
			i.itemGameObject = g.gameObject;
			added = true;
			if(weps.Count == 0 && equipment.Count == 0 && consumables.Count == 0) {
				showEquip = true;
				showWeps = false;
				showCons = false;
			}
			equipment.Add(e);
			i.equipLocation = equipment.IndexOf(e);
		}
		else if(i.isConsumable && consumables.Count < 5) {
			inv.Add(i);
			i.itemGameObject = g.gameObject;
			added = true;
			if(weps.Count == 0 && equipment.Count == 0 && consumables.Count == 0) {
				showCons = true;
				showEquip = false;
				showWeps = false;
			}
			consumables.Add(c);
			i.consLocation = consumables.IndexOf(c);
		}
		else
			added = false;
	}
	
	void Start () {
		pscript = GameObject.FindWithTag("Player").GetComponent<Player>();
		noItem.name = "None";
		noItem.picture = tex;
		noWeapon.wtype = Weapon.type.None;

		noWeapon.wepItem = noItem;
		noEquipment.eItem = noItem;

		equipedE.Add(noEquipment);
		equipedE.Add(noEquipment);
		equipedE.Add(noEquipment);
	}

	void OnGUI() {
		GUI.skin = skin;

		if(!Battle.inBattle) {
			if(GUI.Button(new Rect(Screen.width * 6/16 + 30, Screen.height * 18/20, Screen.width * 1/16, Screen.height * 1/16), "Inventory") && !NPC.inDialouge) {
				showInvWin = !showInvWin;
				showCharWin = !showCharWin;
			}
			if(GUI.Button(new Rect(Screen.width * 7/16 + 45, Screen.height * 18/20, Screen.width * 1/16, Screen.height * 1/16), "Skills") && !NPC.inDialouge) {
				showSkillWin = !showSkillWin;
			}
		}

		if(showInvWin) {
			invWin = GUI.Window(0, invWin, InvWin, " ");
			bagWin = GUI.Window(1, bagWin, BagWin, " ");
			inInventory = true;
		}
		if(showCharWin) {
			charWin = GUI.Window(2, charWin, CharWin, " ");
			inInventory = true;
		}
		if(showSkillWin) {
			skillsWin = GUI.Window(3, skillsWin, SkillsWin, " ");
			inInventory = true;
		}
		if(!showCharWin && !showInvWin && !showSkillWin)
			inInventory = false;
	}

	void InvWin(int windowID) {
		GUI.Box(new Rect(20,20,310,40),"Equipment");
		GUI.Box(new Rect(75,65,200,40),"Weapon: " + equipedWeapon.wepItem.name + " (" + equipedWeapon.wtype.ToString() + ")");
		GUI.DrawTexture(new Rect(75,110,200,200),equipedWeapon.wepItem.picture,ScaleMode.StretchToFill);
		GUI.Box(new Rect(20,320,310,40),"Boots: " + equipedE[0].eItem.name);
		GUI.Box(new Rect(20,370,310,40),"Gloves: " + equipedE[1].eItem.name);
		GUI.Box(new Rect(20,420,310,40),"Helmet: " + equipedE[2].eItem.name);
	}

	void BagWin(int windowID) {

		if(GUI.Button(new Rect(20,80,90,25), "Weapons")) {
			showWeps = true;
			showEquip = false;
			showCons = false;
		}

		if(GUI.Button(new Rect(115,80,90,25), "Equipment")) {
			showEquip = true;
			showWeps = false;
			showCons = false;
		}

		if(GUI.Button(new Rect(210,80,90,25), "Consumable")) {
			showCons = true;
			showEquip = false;
			showWeps = false;
		}

		GUI.Box(new Rect(15,20,550,40),"Inventory");
		if(showWeps) {
			for(int x = 0; x < weps.Count; x++) {
			if(GUI.Button(new Rect(20,110+x*30,275,25), weps[x].wepItem.name)) {
					selectedItem = weps[x].wepItem;
				}
			}
			GUI.Box(new Rect(20,300,275,50),"Weapons: " + weps.Count + " / 5");
		}

		if(showEquip) {
			for(int x = 0; x < equipment.Count; x++) {
				if(GUI.Button(new Rect(20,110+x*30,275,25), equipment[x].eItem.name)) {
					selectedItem = equipment[x].eItem;
				}
			}
			GUI.Box(new Rect(20,300,275,50),"Equipment: " + equipment.Count + " / 5");
		}

		if(showCons) {
			for(int x = 0; x < consumables.Count; x++) {
				if(GUI.Button(new Rect(20,110+x*30,275,25), consumables[x].consItem.name)) {
					selectedItem = consumables[x].consItem;
				}
			}
			GUI.Box(new Rect(20,300,275,50),"Consumables: " + consumables.Count + " / 5");
		}

		GUI.DrawTexture (new Rect(315,80,250,250),selectedItem.picture,ScaleMode.StretchToFill);
		GUI.Box (new Rect(315,330,250,100),selectedItem.description);

		string action = " ";
		if(selectedItem.isWeapon && !weps[selectedItem.weaponLocation].equipped || selectedItem.isEquipement && !equipment[selectedItem.equipLocation].equipped) {
			action = "Equip";
		}
		if(selectedItem.isWeapon && weps[selectedItem.weaponLocation].equipped || selectedItem.isEquipement && equipment[selectedItem.equipLocation].equipped) {
			action = "Unequip";
		}
		if(!selectedItem.isWeapon && !selectedItem.isEquipement) {
			action = "Use";
		}

		if(GUI.Button (new Rect(315,430,125,50),action)){
			if(selectedItem.isWeapon && weps[selectedItem.weaponLocation].equipped) {
				equipedWeapon.equipped = false;
				AlterStats(-1);
				equipedWeapon = null;
			}
			else if(selectedItem.isWeapon && !weps[selectedItem.weaponLocation].equipped) {
				equipedWeapon.equipped = false;
				AlterStats(-1);
				equipedWeapon = weps[selectedItem.weaponLocation];
				equipedWeapon.equipped = true;
				AlterStats(1);
			}
			else if(selectedItem.isEquipement && equipment[selectedItem.equipLocation].equipped) {
				equipedE[equipment[selectedItem.equipLocation].slot].equipped = false;
				AlterStatsE(-1, equipedE[equipment[selectedItem.equipLocation].slot]);
				equipedE[equipment[selectedItem.equipLocation].slot] = noEquipment;
			}
			else if(selectedItem.isEquipement && !equipment[selectedItem.equipLocation].equipped) {
				equipedE[equipment[selectedItem.equipLocation].slot].equipped = false;
				AlterStatsE(-1, equipedE[equipment[selectedItem.equipLocation].slot]);

				equipedE[equipment[selectedItem.equipLocation].slot] = equipment[selectedItem.equipLocation];
				equipedE[equipment[selectedItem.equipLocation].slot].equipped = true;
				AlterStatsE(1, equipedE[equipment[selectedItem.equipLocation].slot]);
			}
			else if (selectedItem.isConsumable) {
				UseConsumables();
			}
		}

		if(GUI.Button (new Rect(440,430,125,50),"Drop")){ //Unequips and removes item
			if(selectedItem.isWeapon && weps[selectedItem.weaponLocation].equipped) {
				equipedWeapon.equipped = false;
				AlterStats(-1);
				weps.Remove(equipedWeapon);
				equipedWeapon = null;
			}
			else if(selectedItem.isEquipement && equipedE[equipment[selectedItem.equipLocation].slot].equipped) {
				AlterStatsE(-1, equipedE[equipment[selectedItem.equipLocation].slot]);
				equipedE[equipment[selectedItem.equipLocation].slot].equipped = false;
				equipedE[equipment[selectedItem.equipLocation].slot] = noEquipment;
				equipment.Remove(equipment[selectedItem.equipLocation]);
			}
			else if(selectedItem.isConsumable) {
				consumables.Remove(consumables[selectedItem.consLocation]);
			}
			else if(selectedItem.isWeapon) {
				weps.Remove(weps[selectedItem.weaponLocation]);
			}
			else if(selectedItem.isEquipement) {
				equipment.Remove(equipment[selectedItem.equipLocation]);
			}
			//Drops clone of item;
			GameObject droppedItem = Instantiate (selectedItem.itemGameObject,new Vector3(transform.position.x,transform.position.y + 2,transform.position.z -4), transform.rotation) as GameObject;
			droppedItem.name = selectedItem.name;
			inv.Remove(selectedItem);
			selectedItem = null;

			//Reassigns weapon locations
			foreach(Weapon w in weps) {
				w.wepItem.weaponLocation = weps.IndexOf(w);
			}
			foreach(Equipment e in equipment) {
				e.eItem.equipLocation = equipment.IndexOf(e);
			}
			foreach(Consumable c in consumables) {
				c.consItem.consLocation = consumables.IndexOf(c);
			}
		}
	}

	//Displays Player Stats
	void CharWin(int windowID) {
		GUI.Box(new Rect(20,20,310,40),"Character");
		GUI.Box(new Rect(20,145,150,30), "Health: " + pscript.player.job.Health + "/" + pscript.player.job.maxHealth);
		GUI.Box(new Rect(180,145,150,30), "Mana: " + pscript.player.job.Mana + "/" + pscript.player.job.maxMana);
		GUI.Box(new Rect(20,180,150,30), "Experience: " + pscript.player.Experience + "/" + pscript.player.NextLevelExp);
		GUI.Box(new Rect(180,180,150,30), "Stat Points: " + pscript.player.LevelupPoints);
		GUI.Box(new Rect(20,100,310,40), "Level " + pscript.player.Level.ToString() + " " + pscript.player.job.name);
		GUI.Box(new Rect(20,240,150,40), "Attack: " + pscript.player.job.Attack.ToString());	
		GUI.Box(new Rect(20,290,150,40), "Ranged: " + pscript.player.job.Ranged.ToString());	
		GUI.Box(new Rect(20,340,150,40), "Magic: " + pscript.player.job.Magic.ToString());	
		GUI.Box(new Rect(20,390,150,40), "Defense: " + pscript.player.job.Defense.ToString());	
		GUI.Box(new Rect(20,440,150,40), "Speed: " + pscript.player.job.Speed.ToString());

		if(pscript.player.LevelupPoints > 0) {
			if(GUI.Button(new Rect(180,240,40,40),"+")) {
				pscript.player.job.Attack += 5;
				pscript.player.LevelupPoints -= 5;
			}
			if(GUI.Button(new Rect(180,290,40,40),"+")) {
				pscript.player.job.Ranged += 5;
				pscript.player.LevelupPoints -= 5;
			}
			if(GUI.Button(new Rect(180,340,40,40),"+")) {
				pscript.player.job.Magic += 5;
				pscript.player.LevelupPoints -= 5;
			}
			if(GUI.Button(new Rect(180,390,40,40),"+")) {
				pscript.player.job.Defense += 5;
				pscript.player.LevelupPoints -= 5;
			}
			if(GUI.Button(new Rect(180,440,40,40),"+")) {
				pscript.player.job.Speed += 5;
				pscript.player.LevelupPoints -= 5;
			}
		}
	}

	void SkillsWin (int windowID) {
		GUI.Box(new Rect(20, 20, 310, 40),"Skills");
		for(int i = 0; i < pscript.player.skills.Count;i++) {
			GUI.Box(new Rect(75,90+i*140,200,35),pscript.player.skills[i].name);
			GUI.Box(new Rect(25,130+i*140,300,35),pscript.player.skills[i].description);
			GUI.Box(new Rect(75,170+i*140,200,35),"Cost: " + pscript.player.skills[i].initiativeCost + " Initiative " + 
			        									pscript.player.skills[i].manaCost + " Mana");
		}
	}

	void Update () {
		NoWeaponType();
		if(!Battle.inBattle) {
			if(Input.GetKeyUp(KeyCode.M)) {
				showInvWin = !showInvWin;
				showCharWin = !showCharWin;
			}
			if(showInvWin || showCharWin || showSkillWin) {
				Time.timeScale = 0;
			}
			else {
				Time.timeScale = 1;
			}
			if(equipedWeapon == null) {
				equipedWeapon = noWeapon;
			}
			if(selectedItem == null) {
				selectedItem = noItem;;
			}
		}
	}
	void AlterStats (int i) {
		pscript.player.job.Attack += equipedWeapon.wepAttack*i;
		pscript.player.job.Magic += equipedWeapon.wepMagic*i;
		pscript.player.job.Ranged += equipedWeapon.wepRange*i;
	}
	void AlterStatsE (int i, Equipment e) {
		pscript.player.job.Defense += e.eDefense*i;
		pscript.player.job.Speed += e.eSpeed*i;
		pscript.player.job.maxHealth += e.eHealth*i;
		pscript.player.job.Health += e.eHealth*i;
		pscript.player.job.Mana += e.eMana*i;
		pscript.player.job.maxMana += e.eMana*i;
		if(pscript.player.job.Health <= 0) {
			pscript.player.job.Health = 1;
		}
	}

	public void UseConsumables () {

		bool used = false;
		if(consumables[selectedItem.consLocation].ctype == Consumable.type.Potion) {
			if(consumables[selectedItem.consLocation].cpotion == Consumable.potion.HPotion && pscript.player.job.Health < pscript.player.job.maxHealth) {
				Debug.Log ("Potion");
				pscript.player.job.Health += consumables[selectedItem.consLocation].effect * pscript.player.job.maxHealth / 100;
				used = true;
				if(pscript.player.job.Health > pscript.player.job.maxHealth) 
					pscript.player.job.Health = pscript.player.job.maxHealth;
			}
			else if(consumables[selectedItem.consLocation].cpotion == Consumable.potion.MPotion && pscript.player.job.Mana < pscript.player.job.maxMana) {
				pscript.player.job.Mana += consumables[selectedItem.consLocation].effect * pscript.player.job.maxMana / 100;
				used = true;
				if(pscript.player.job.Mana > pscript.player.job.maxMana) 
					pscript.player.job.Mana = pscript.player.job.maxMana;
			}
		}

		if(used) {
			consumables.Remove(consumables[selectedItem.consLocation]);
			inv.Remove(selectedItem);
			selectedItem = noItem;
			foreach(Consumable c in consumables) {
				c.consItem.consLocation = consumables.IndexOf(c);
			}
		}
	}

	public void NoWeaponType() {
		if(pscript.player != null) {
			if(pscript.player.job == pscript.sunWarrior) {
				noWeapon.wtype = Weapon.type.Melee;
			}
			if(pscript.player.job == pscript.starMage) {
				noWeapon.wtype = Weapon.type.Magic;
			}
			if(pscript.player.job == pscript.skyRouge) {
				noWeapon.wtype = Weapon.type.Ranged;
			}
		}
	}
}
