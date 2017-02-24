// Mathew McCain
// cscd371 midterm project
// contains static Watcher class for use with FormMonitor, contains inner class WatcherFilter to act as a filter class (thread safe)
// as well as Delegate (WatcherEventHandler) and Event Args (WatcherEventArgs) class for Watcher events to the form


using System;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Concurrent;

// assignment 2 namespace
namespace mccainmmidterm {
	// WatcherEventArgs, contains arguments for WatcherEvent
	// could be a struct
	class WatcherEventArgs {
		// contained vars
		private string itemType, itemName, path, eventType;
		private DateTime eventDateTime;
		// get properties (don't want set public)
		public string ItemType { get { return itemType; } }
		public string ItemName { get { return itemName; } }
		public string Path { get { return path; } }
		public string EventName { get { return eventType; } }
		public DateTime EventDateTime { get { return eventDateTime; } }
		// constructor
		public WatcherEventArgs(string _type, string _name, string _path, string _eventType, DateTime _datetime) {
			itemType = _type.ToLower();
			itemName = _name;
			path = _path;
			eventType = _eventType;
			eventDateTime = _datetime;
		}
	}

	// Delegate for an event handler from Watcher to MonitorForm
	delegate void WatcherEventHandler(WatcherEventArgs args);

	// watcher class
	static class Watcher {
		// internal WatchFilter, thread-safe list of strings to filter for
		// had to use because had difficulties getting List<string> to be thread safe
		// and ConcurrentBag<string> did not have a Contains() method that I could get to work
		class WatcherFilter {
			// internal list
			private List<string> filters;
			// lock obj
			private object lockObj;

			// constructor
			public WatcherFilter() {
				// create copy of given list
				filters = new List<string>();
				lockObj = new object();
			}

			// Set filters to
			public void Set(List<string> list) {
				lock (lockObj) {
					// copy list to new list, assign
					// make each string lowercase
					filters = new List<string>();
					foreach (string str in list) {
						filters.Add(str.ToLower());
					}
				}
			}

			// check if given item type is filtered
			public bool Filtered(string type) {
				lock(lockObj) {
					// if filter is empty, don't filter
					if (filters.Count <= 0) {
						return false;
					}

					// filter type if not in list
					return !(filters.Contains(type.ToLower()));
				}
			}
		}

		// uses watcher example provided on MSDN
		// https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx

		// Event for Watcher event to Form
		private static event WatcherEventHandler watcherEvent;

		// FileSystemWatcher obj
		private static FileSystemWatcher watcher;

		// WatcherFilter obj
		private static WatcherFilter filters;

		// constructor
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		static Watcher() {
			// create watcher class
			watcher = new FileSystemWatcher();

			// set notification flags
			// watch for changes to
			//	CreationTime, File Name, LastWrite, Attributes, Security, Size
			watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.Size;

			// Set to watch subdirectories
			watcher.IncludeSubdirectories = true;

			// set filter to only watch for names with '.'
			watcher.Filter = "*.*";

			// set event handlers
			watcher.Created += new FileSystemEventHandler(EH_OnChanged);
			watcher.Deleted += new FileSystemEventHandler(EH_OnChanged);
			watcher.Renamed += new RenamedEventHandler(EH_OnRenamed);
			watcher.Changed += new FileSystemEventHandler(EH_OnChanged);

			// create List for filters
			filters = new WatcherFilter();
		}


		// Properties

		// add event handler for a WatcherEvent
		public static WatcherEventHandler WatcherEvent {
			set {
				watcherEvent += value;
			}
		}

		// set path
		// watcher.Path = ".."
		// validate path? watcher may check for sure
		// length check
		// Path.IsPathRooted(path)
		// Directory.Exists(path)
		public static string Path {
			get {
				return watcher.Path;
			}
			set {
				// will throw an error if path invalid
				watcher.Path = value;
			}
		}

		// enable watching
		public static bool Watching {
			get {
				return watcher.EnableRaisingEvents;
			}
			set {
				watcher.EnableRaisingEvents = value;
			}
		}

		// set filter
		// takes a string array of filters to apply for
		// does not uses FileSystemWatcher filter
		// converts List to ConcurrentBag
		public static List<string> Filter {
			set {
				filters.Set(value);
			}
		}


		// methods

		// log method, compiles event message, reports to screen/log file
		// string fullPath, absolute path to item changed
		// string change, change that occurred
		private static void Event(string fullPath, string change) {
			// determine if item was file or directory for message
			// cannot use DirectoryInfo/FileInfo because if created file is changed/renames right after they will not function properly
			// various other edge cases like that
			// also does not work with deletes
			string itemType = "unknown";
			string itemName = fullPath;

			// tokenize by '\'
			string[] toks = fullPath.Split('\\');
			// grab last token as itemName
			itemName = toks[toks.Length - 1];

			// treat as, if contains '.' then is a file - else directory
			if (itemName.Contains(".")) {
				// get extension name
				toks = itemName.Split('.');
				itemType = toks[toks.Length - 1];
			} else {
				itemType = "Directory";
			}

			// ignore updates on "unknown" or "directory" types
			if (itemType == "unknown" || itemType == "Directory")
				return;
			
			// check if item type is filtered
			if (filters.Filtered(itemType)) {
				return;
			}

			// Build arguments for event
			WatcherEventArgs args = new WatcherEventArgs(itemType, itemName, fullPath, change, DateTime.Now);

			// Invoke event for Form
			// ? to check for null, in case of race conditions
			// but shouldn't happen as long as Monitor sets up the event fnc before path and watching variable
			watcherEvent?.Invoke(args);
		}


		// event handlers

		// on file/directory created/changed/deleted event
		private static void EH_OnChanged(object source, FileSystemEventArgs e) {
			// send relevant info to Event()
			Event(e.FullPath, e.ChangeType.ToString());
		}

		// on file/directory renamed event
		private static void EH_OnRenamed(object source, RenamedEventArgs e) {
			// send relevant info to Log()
			Event(e.OldFullPath, e.ChangeType.ToString());
		}
	}
}
