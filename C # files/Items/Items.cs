using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

	public Texture invPic;
	public GameObject invMesh;

	public Equipment invEquip;
	public Item invItem;
	public Weapon invWep;
	public Consumable invCons;

	public string description;
	public bool isWeapon;
	public bool isEquipment;
	public bool isConsumable;

	public Inventory invScript;

	// Use this for initialization
	void Start() {
		invItem = new Item();
		invItem.name = gameObject.name;
		invItem.picture = invPic;
		invItem.description = description;
		invItem.isWeapon = isWeapon;
		invItem.isEquipement = isEquipment;
		invItem.isConsumable = isConsumable;
		//invMesh = (GameObject)Resources.Load (gameObject.name);
		//invItem.itemGameObject = invMesh;

		if(isWeapon) {
			WeaponStats wscript = GetComponent<WeaponStats>();
			invWep = new Weapon();
			if(wscript.isMeleeW)
				invWep.wtype = Weapon.type.Melee;
			if(wscript.isRangedW)
				invWep.wtype = Weapon.type.Ranged;
			if(wscript.isMagicW)
				invWep.wtype = Weapon.type.Magic;
			invWep.wepItem = invItem;
			invWep.wepAttack = wscript.wAttack;
			invWep.wepRange = wscript.wRanged;
			invWep.wepMagic = wscript.wMagic;
			invMesh = (GameObject)Resources.Load ("Weapons/"+gameObject.name);
			invItem.itemGameObject = invMesh;

			invItem.description += "\n ";
			invItem.description += "\n Attack: " + invWep.wepAttack.ToString();
			invItem.description += "     Ranged: " + invWep.wepRange.ToString();
			invItem.description += "\n Magic: " + invWep.wepMagic.ToString();
			invItem.description += "     Type: " + invWep.wtype.ToString();

		}
		if(isEquipment) {
			EquipmentStats escript = GetComponent<EquipmentStats>();
			invEquip = new Equipment();
			invEquip.eItem = invItem;
			invEquip.eDefense = escript.eDefense;
			invEquip.eSpeed = escript.eSpeed;
			invEquip.eHealth = escript.eHealth;
			invEquip.eMana = escript.eMana;
			invEquip.slot = escript.slot;
			invMesh = (GameObject)Resources.Load ("Equipment/"+gameObject.name);
			invItem.itemGameObject = invMesh;

			//string slotString = "Equipment";
			//if(escript.slot == 0)
			//	slotString = "Boots";
			//if(escript.slot == 1)
			//	slotString = "Gloves";
			//if(escript.slot == 2)
			//	slotString = "Helmet";//

			invItem.description += "\n ";
			invItem.description += "\n Defense: " + invEquip.eDefense.ToString();
			invItem.description += "     Speed: " + invEquip.eSpeed.ToString();
			invItem.description += "\n Health: " + invEquip.eHealth.ToString();
			invItem.description += "     Mana: " + invEquip.eMana.ToString();
		}

		if(isConsumable) {
			ConsumableStats cscript = GetComponent<ConsumableStats>();
			invCons = new Consumable();
			invCons.consItem = invItem;
			invCons.effect = cscript.effectiveness;
			if(cscript.isCure) {
				invCons.ctype = Consumable.type.Cure;
			}
			if(cscript.isPotion) {
				invCons.ctype = Consumable.type.Potion;
				if(cscript.isHpotion) {
					invCons.cpotion = Consumable.potion.HPotion;
					invCons.consItem.description = "A potion that restores health";
					invCons.consItem.description += "\n Effectiveness: " + invCons.effect + "%";
				}
				if(cscript.isMpotion) {
					invCons.cpotion = Consumable.potion.MPotion;
					invCons.consItem.description = "A potion that restores mana";
					invCons.consItem.description += "\n Effectiveness: " + invCons.effect + "%";
				}
			}
			invMesh = (GameObject)Resources.Load ("Consumable/"+gameObject.name);
			invItem.itemGameObject = invMesh;
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if(other.collider.gameObject.CompareTag("Player")) {
			Debug.Log ("Picked Up");
			invScript.AddItem(invItem,invWep, invEquip, invItem.itemGameObject, invCons);

			if(invScript.added)
				Destroy (gameObject);
		}
	}

	public void Effect(Item i) {

	}

	void Update() { 
		invScript = GameObject.FindWithTag("Player").GetComponent<Inventory>();
	}
}
