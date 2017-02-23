package actbs;

enum CabinClass implements SpotClass {
	family(4),deluxFamily(6),couples(2),deluxCouples(2);
	
	private int maxPeople;
	
	CabinClass(int _count) {
		maxPeople = _count;
	}
	
	public String toString() {
		switch (this) {
		case family: return String.format("Family (holds %1$d people)", maxPeople);
		case deluxFamily: return String.format("Delux Family (holds %1$d people)", maxPeople);
		case couples: return String.format("Couples (holds %1$d people)", maxPeople);
		case deluxCouples: return String.format("Delux Couples (holds %1$d people)", maxPeople);
		// will never reach here
		default: return "invalid";
		}
	}
	
	static public SpotClass fromString(String _name) {
		_name = _name.toLowerCase();
		switch (_name) {
		case "family": return family;
		case "deluxfamily": return deluxFamily;
		case "couples": return couples;
		case "deluxcouples": return deluxCouples;
		default: return null;
		}
	}
	
	static String fromFileString(String _name) {
		_name = _name.toUpperCase();
		switch (_name) {
		case "F": return "family";
		case "DF": return "deluxFamily";
		case "C": return "couples";
		case "DC": return "deluxCouples";
		default: return null;
		}
	};
	public String toFileString() {
		switch (this) {
		case family: return "F";
		case deluxFamily: return "DF";
		case couples: return "C";
		case deluxCouples: return "DC";
		default: return "N";
		}
	}
	
	static String getTypeName() {return "Cabin";};
	
	static String getOptions(String _prefix) {
		String _str = "";
		_str += _prefix + family.toString() + ": family" + "\n";
		_str += _prefix + deluxFamily.toString() + ": deluxFamily" + "\n";
		_str += _prefix + couples.toString() + ": couples" + "\n";
		_str += _prefix + deluxCouples.toString() + ": deluxCouples" + "\n";
		return _str;
	}
}
