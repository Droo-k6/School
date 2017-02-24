**cscd 371 midterm project**

C#/.NET

Project was to use .NET's FileSystemWatcher with windows forms to build a gui and offer user to log to a database using SQLite.

[.NET FileSystemWatcher](https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher(v=vs.110).aspx)

[.NET SQLite](https://system.data.sqlite.org/)

---
Source files inside the visual studio project folder

**Main.cs**

Contains main class that creates/displays the main form.
Also contains a static method used by FormMonitor and FormQuery


**FormMonitor.cs**

Contains partial FormMonitor class, the main form/gui for the project.
Rest of class within **FormMonitor.Designer.cs**


**FormQuery.cs**

Contains partial FormQuery class, form used for quering the selected SQLite database, allows filtering for specific file types (used in SQLite query).
Rest of class within **FormQuery.Designer.cs**


**FormWriteWait.cs**

Contains partial FormWriteWait class, used to display a message to the user while writing to SQLite database since whole program will lockup during that time.
Rest of class within **FormWriteWait.Designer.cs**


**Watcher.cs**

Contains Watcher class, a static class that handles setting up the FileSystemWatcher and allows path and watching bool to be set via properties. Also allows new event methods to be added to be called later (although no method removal).
Also defines the WatcherEventHandler delegate, used by Watcher and FormMonitor for file system events. 
WatcherEventArgs, a small class to represent a watcher event entry, used by Watcher, FormMonitor, and EventDatabase.


**WatcherEventBuffer.cs**

WatcherEventBuffer class, used between FormMonitor and EventDatabase. Thread safe, allows WatcherEventArgs to be enqued onto it, to be written to the database when ready. Allows Enqueueing of single objects, however can only dequeue back a .NET Queue<WatcherEventArgs> object that the EventDatabase will used to ensure if insertions fail, it can write back the queue contents. Also to prevent the EventDatabase from constantly writing to the database on one write call (possible deadlock for user). 


**EventDatabase.cs**

Handles SQLite connection, creating database/table and inserting/queueing for events. Created/called by FormMonitor.


---
**Reflection**

More of the logic for the FormMonitor class that handles parsing string inputs/database interaction could be moved to a seperate class, but most of that is already offloaded to other classes such as EventDatabase and Watcher.

WatchEventArgs should just be changed to a structure, does not need to be a class. Just have to make it read only after construction (which is possible).

SQLite queries are vulnerable to SQL injection, should be corrected which is a simple change using the AddParameters() method from the SQLiteCommand class.

Writing to the SQLite database could also be sped up by switching to use 1 large insert statement instead of 1 per item, was not done initially in case of connection loss during insertions (and remaining values are added back to the buffer to be readded). Or by employing a BackgroundWorker/seperate thread to handle the insertion without locking up the main form.


---
Some demo images

![alt text](https://github.com/Droo-k6/School/blob/master/cscd%20371%20midterm/images/MainForm1.PNG)
![alt text](https://github.com/Droo-k6/School/blob/master/cscd%20371%20midterm/images/MainForm2.PNG)
![alt text](https://github.com/Droo-k6/School/blob/master/cscd%20371%20midterm/images/QueryForm1.PNG)
![alt text](https://github.com/Droo-k6/School/blob/master/cscd%20371%20midterm/images/QueryForm2.PNG)
