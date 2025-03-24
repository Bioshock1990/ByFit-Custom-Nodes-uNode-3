#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class ExampleAnimals : MonoBehaviour {	
	[SerializeReference]
	public List<animalTypes> ListAnimals;
}
[System.Serializable]
public class animalTypes {	
	public virtual void Action() {
	}
	
	public virtual int GetDamage() {
		return 0;
	}
	
	public virtual int GetHealth() {
		return 0;
	}
}
public class cat : animalTypes {	
	public string say = "myu";
	public int health;
	public float newVariable;
	public bool newVariable2;
	public GameObject newVariable5;
	
	public override int GetHealth() {
		return health;
	}
	
	public override void Action() {
		Debug.Log(say);
	}
}
public class bear : animalTypes {	
	public int health;
	public string say = "";
	
	public override void Action() {
		Debug.Log(say);
	}
	
	public override int GetHealth() {
		return health;
	}
}
public class lion : animalTypes {	
	public int health;
	public string say = "";
	public int claws;
	
	public override int GetHealth() {
		return health;
	}
	
	public override void Action() {
		Debug.Log(say);
	}
}

