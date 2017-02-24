// Mathew McCain
// cscd371 midterm project
// EventDatabase class

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace mccainmmidterm {
	// EventDatabase class, for writing WatcherEventArgs to a DB, thread safe
	class EventDatabase {
		// SQLite connection and command objects
		private SQLiteConnection sqlite_connection;

		// current table name
		private string currentTableName;

		// lock object
		private object lockObj;

		// Constructor
		public EventDatabase() {
			sqlite_connection = null;
			currentTableName = "";
			lockObj = new object();
		}

		// Destructor
		~EventDatabase() {
			// close up everything..
			/*
			if (sqlite_connection != null) {
				sqlite_connection.Close();
				sqlite_connection.Dispose();
				sqlite_connection = null;
			}*/
		}

		// property if db connection is active
		public bool Active {
			get {
				lock(lockObj) {
					return AliveCheck();
				}
			}
		}

		// get name, assumes caller locks
		private string ActiveName {
			get {
				// DataSource vs Database property?
				// DataSource - name of file (no '.db')
				// Database - returns Main
				return (sqlite_connection == null) ? "" : sqlite_connection.DataSource;
			}
		}

		// get current db name
		public string Name {
			get {
				lock (lockObj) {
					return ActiveName;
				}
			}
		}

		// alive check, will make sure connection is still ongoing - if not retry
		// does not lock, assumes caller locks
		private bool AliveCheck() {
			try {
				// check for connection object
				if (sqlite_connection == null)
					return false;

				// check state
				ConnectionState state = sqlite_connection.State;

				bool closed = (state == ConnectionState.Closed);
				bool broken = (state == ConnectionState.Broken);

				// return true if connection is not broken or closed
				if (!broken && !closed) {
					return true;
				}

				// if broken, close connection
				if (broken)
					sqlite_connection.Close();

				// attempt to reopen
				sqlite_connection.Open();

				return true;
			} catch (SQLiteException e) {
				// cleanup connection
				// don't need null check, handled at beginning of try block
				// warn user that connection was lost
				System.Windows.Forms.MessageBox.Show(string.Format("CONNECTION TO '{0}' LOST:\n{1}", ActiveName, e.Message));

				sqlite_connection.Dispose();
				sqlite_connection = null;

				return false;
			}
		}

		// generates table name to use
		private string GenerateTableName() {
			// not using just so its easier to test out query
			//string name = string.Format("monitor_{0:MM_dd_yy_HH_mm}", DateTime.Now);
			string name = "monitor_results";
			return name;
		}

		// get schema definition for table
		private string TableSchemaDefinition() {
			// schema
			//	event # : int		primary key			eventID INTEGER primary key
			//	filename : str							itemName TEXT
			//	item type: str							itemType TEXT
			//	full path: str							fullPath TEXT
			//	event name: str							eventType TEXT
			//	unixDateTime: long						unixDateTime INTEGER
			string schema = "(";
			schema += "eventID INTEGER primary key";
			schema += ", itemName TEXT";
			schema += ", itemType TEXT";
			schema += ", fullPath TEXT";
			schema += ", eventName TEXT";
			schema += ", unixDateTime INTEGER";
			schema += ")";
			return schema;
		}
		// get schema for table insertion (ommits eventID (primary key) so SQLite auto generates)
		private string TableSchema() {
			return "(itemName,itemType,fullPath,eventName,unixDateTime)";
		}

		// connect to given db
		// returns if successful
		// only called by Use() method (so no need to lock db obj)
		private bool Connect(string name) {
			// don't need to validate name, Use() does already
			// temp sqlite variables
			SQLiteConnection testConnection = null;
			SQLiteCommand testCommand = null;

			try {
				// don't need to test if db exists, SQLite will create by default
				// connect to db
				testConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", name));

				// open connection
				testConnection.Open();

				// create command
				testCommand = testConnection.CreateCommand();

				// generate table name
				string tableName = GenerateTableName();

				// query to create table if doesn't exist
				// if the table exists but the schema doesn't match, let the exception come
				testCommand.CommandText = string.Format("CREATE TABLE IF NOT EXISTS {0} {1};", tableName, TableSchemaDefinition());
				testCommand.ExecuteNonQuery();

				// dispose command
				testCommand.Dispose();

				// assign connection and tablename
				sqlite_connection = testConnection;
				currentTableName = tableName;

				return true;
			} catch (SQLiteException e) {
				// if current connection is active, alert user that still connected to
				string msg = string.Format("CONNECTION TO '{0}' FAILED:\n{1}\n", name, e.Message);
				if (sqlite_connection != null) {
					msg += string.Format("\nReverted back to database '{0}'", Name);
				}
				System.Windows.Forms.MessageBox.Show(msg);
			}catch(Exception e) {
				System.Windows.Forms.MessageBox.Show("CONNECT EXCEPTION:\n" + e.Message);
			}

			// clean up attempted connection
			// don't want in finally block, since at end of try is a return true
			if (testConnection != null) {
				if (testCommand != null) {
					testCommand.Dispose();
				}
				testConnection.Close();
				testConnection.Dispose();
			}

			return false;
		}

		// use given db name, returns if was able to use correctly
		public bool Use(string name) {
			// name validation
			// strip name, check if empty
			name = name.Trim();
			if (name.Length <= 0) {
				return false;
			}
			// check if ends in ".db"
			if (!name.EndsWith(".db")) {
				// add to end
				name += ".db";
			}

			// lock db
			lock (lockObj) {
				// attempt to connect to db
				return Connect(name);
			}
		}

		// generates Insert statement for args
		private string GenerateInsert(WatcherEventArgs args) {
			// convert DateTime from args to unix time (in seconds)
			DateTimeOffset offset = args.EventDateTime;
			long unixTimeStamp = offset.ToUnixTimeSeconds();
			// doesn't save UTC offset of machine, so on db query conversion will use current UTC machine to convert

			// generate and return insert
			return string.Format("INSERT INTO '{0}' {1} VALUES ('{2}','{3}','{4}','{5}',{6});",
				currentTableName,
				TableSchema(),
				args.ItemName,
				args.ItemType,
				args.Path,
				args.EventName,
				unixTimeStamp);
		}

		// generates query statement for extension list
		private string GenerateQuery(List<string> extList) {
			// SELECT * from {0} where {1};
			// {0}, currentTableName
			// {1}, where clause to generate

			// generate where clause
			string whereClause = "";
			if (extList.Count <= 0) {
				// if list is empty, grab all
			}else{
				// surround and join
				whereClause = "(itemType='";
				whereClause += string.Join("')or(itemType='", extList);
				whereClause += "')";
			}

			// generate final statement
			return string.Format("SELECT * from {0} WHERE {1};", currentTableName, whereClause);
		}

		// parses reader into WatcherEventArgs
		private WatcherEventArgs ParseRead(SQLiteDataReader reader) {
			// pull data from result
			// (itemName,itemType,fullPath,eventName,unixDateTime)
			string _itemName = (string) reader["itemName"];
			string _itemType = (string) reader["itemType"];
			string _path = (string) reader["fullPath"];
			string _event = (string) reader["eventName"];
			long _unixTimeSeconds = (long) reader["unixDateTime"];

			// convert unix time (in seconds) to DateTime
			DateTimeOffset _offset = DateTimeOffset.FromUnixTimeSeconds(_unixTimeSeconds);
			DateTime _dateTime = _offset.ToLocalTime().DateTime;

			// build obj and return
			return new WatcherEventArgs(_itemType, _itemName, _path, _event, _dateTime);
		}
		
		// write given buffer to db
		// displays form to user to please wait
		// could make use of BackgroundWorker to prevent lockup, or async
		public void Write(WatcherEventBuffer buffer) {
			// get lock on db
			lock (lockObj) {
				// ensure the connection is alive first
				if (!AliveCheck()) {
					return;
				}

				// get buffer as new queue (to avoid having to keep is locked during DB write)
				Queue<WatcherEventArgs> queue = buffer.DequeueToQueue();

				// form to display to user while writing to db
				FormWriteWait waitForm = new FormWriteWait();
				waitForm.Show();
				waitForm.Refresh();

				try {
					// create command obj
					SQLiteCommand com = sqlite_connection.CreateCommand();

					while (queue.Count > 0) {
						// dequque
						WatcherEventArgs args = queue.Dequeue();
						// generate insert and execute
						com.CommandText = GenerateInsert(args);
						com.ExecuteNonQuery();
					}
				} catch (Exception e) {
					// check if is an SQLite exception
					// just to avoid having to do 2 of almost the same catch blocks
					if (e is SQLiteException) {
						System.Windows.Forms.MessageBox.Show(string.Format("Failed to write to database\nSQLite Exception: {0}", e.Message));
					} else {
						System.Windows.Forms.MessageBox.Show(string.Format("Failed to write to database\nException: {0}", e.Message));
					}
				}

				// close form
				waitForm.Close();

				// pass remaining queue back to buffer (if there was an error)
				while (queue.Count > 0) {
					buffer.Enqueue(queue.Dequeue());
				}
			}
		}

		// query for list of extensions
		public List<WatcherEventArgs> QueryFor(List<string> extList) {
			// get lock on db
			lock(lockObj) {
				List<WatcherEventArgs> queryResults = new List<WatcherEventArgs>();

				// ensure the connection is alive first
				if (!AliveCheck())
					return queryResults;

				// sqlite command obj
				SQLiteCommand com = null;
				// sqlite reader obj
				SQLiteDataReader reader = null;

				try {
					// create command
					com = sqlite_connection.CreateCommand();

					// build query
					com.CommandText = GenerateQuery(extList);

					// create data reader
					reader = com.ExecuteReader();

					// loop through reader
					while (reader.Read()) {
						// retrieve info, build new args, add to list
						WatcherEventArgs parsed = ParseRead(reader);
						if (parsed != null)
							queryResults.Add(parsed);
					}

				} catch (Exception e) {
					// check if is an SQLite exception
					// just to avoid having to do 2 of almost the same catch blocks
					if (e is SQLiteException) {
						System.Windows.Forms.MessageBox.Show(string.Format("Failed to query database\nSQLite Exception: {0}", e.Message));
					} else {
						System.Windows.Forms.MessageBox.Show(string.Format("Failed to query database\nException: {0}", e.Message));
					}
				} finally {
					// cleanup 
					if (com != null) {
						if (reader != null) {
							reader.Dispose();
						}
						com.Dispose();
					}
				}

				return queryResults;
			}
		}

		// query database
		// displays a form to user
		public void Query() {
			// get lock on db
			// isn't really needed since a form will block input to the original form
			//	but just in case
			lock (lockObj) {
				// ensure the connection is alive first
				if (!AliveCheck()) {
					return;
				}

				// create form, pass event database
				FormQuery form = new FormQuery(this);
				// show dialog
				form.ShowDialog();
			}
		}
	}
}
