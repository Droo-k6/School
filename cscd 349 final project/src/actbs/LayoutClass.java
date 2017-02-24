package actbs;

enum LayoutClass implements SpotLayoutClass {
	// enum values
	// Small, 3 columns, gap 1&2
	// Medium, 4 columns, gap 2&3
	// Wide, 10 columns, gap 3&4, 7&8
	small("small",3,1), 
	medium("medium",4,2),
	wide("wide",10,3,7);
	
	// members
	// display name
	private String name;
	// number of columns for layout
	private int columns;
	// which columns are aisle columns
	private int[] aisleColumns;
	
	// no error checking on inputs
	// closed system
	
	// constructor for layout w/ 1 gap
	private LayoutClass(String _name, int _columns, int _gap) {
		// construct the array for what columns are aisles, pass to base constructor
		this(_name, _columns,new int[]{_gap-1,_gap});
	}
	// constructor for layout w/ 2 gaps
	private LayoutClass(String _name, int _columns, int _gap1, int _gap2) {
		// construct the array for what columns are aisles, pass to base constructor
		this(_name,_columns,new int[]{_gap1-1,_gap1,_gap2-1,_gap2});
	}
	// constructor
	// takes in # of columns for layout
	// and int array of which columns are aisle seats (from 0 to (n-1))
	private LayoutClass(String _name, int _columns, int[] _gaps) {
		name = _name;
		columns = _columns;
		aisleColumns = _gaps;
	}
	
	// check if given column is a window seat
	boolean isWindowSeat(int _column) throws InvalidLayoutColumn {
		// check that given column is valid
		if ((_column < 0) || (_column >= columns)) {
			throw new InvalidLayoutColumn(_column, columns);
		}
		
		// assume a window seat is one on the edge
		return ((_column == 0)||(_column == (columns-1)));
	}
	// check if given column is a aisle seat
	boolean isAisleSeat(int _column) throws InvalidLayoutColumn {
		// check that given column is valid
		if ((_column < 0) || (_column >= columns)) {
			throw new InvalidLayoutColumn(_column, columns);
		}
		
		// check if column matches one of the gaps
		for (int _aisleColumn : aisleColumns) {
			if (_column == _aisleColumn) {
				return true;
			}
		}
		return false;
	}
	
	// get number of columns
	int getColumns() {
		return columns;
	}
	
	
	public String toString() {
		return this.name;
	}
	
	static SpotLayoutClass fromString(String _name) {
		_name = _name.toLowerCase();
		switch (_name) {
		case "small": return small;
		case "medium": return medium;
		case "wide": return wide;
		default: return null;
		}
	}
	
	static String fromFileString(String _name) {
		switch (_name) {
		case "S": return "small";
		case "M": return "medium";
		case "W": return "wide";
		default: return null;
		}
	}
	public String toFileString() {
		switch (this) {
		case small: return "S";
		case medium: return "M";
		case wide: return "W";
		default: return "N";
		}
	}
	
	static String getTypeName() {return "Layout";};
	
	static String getOptions(String _prefix) {
		String _str = "";
		_str += _prefix + small.toString() + "\n";
		_str += _prefix + medium.toString() + "\n";
		_str += _prefix + wide.toString() + "\n";
		return _str;
	}
}
