// Mathew McCain
// cscd371 midterm project
// WatcherEventBuffer class

using System;
using System.Collections.Generic;
//using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mccainmmidterm {
	// class to contain a buffer of watcher events to be written to db
	// uses generic Queue, ensures is thread safe
	class WatcherEventBuffer {
		// Queue
		// could use ConcurrentQueue
		private Queue<WatcherEventArgs> buffer;
		// lock object
		private object lockObj;

		// constructor
		public WatcherEventBuffer() {
			buffer = new Queue<WatcherEventArgs>();
			lockObj = new object();
		}

		// empty property
		public bool Empty {
			get {
				lock (lockObj) {
					return (buffer.Count <= 0);
				}
				
			}
		}

		// Enqueue
		public void Enqueue(WatcherEventArgs watcherEvent) {
			lock(lockObj) {
				buffer.Enqueue(watcherEvent);
			}
		}

		// Dequeue list
		// dequeues whole buffer to another queue
		public Queue<WatcherEventArgs> DequeueToQueue() {
			Queue<WatcherEventArgs> nQueue = new Queue<WatcherEventArgs>();

			// lock
			lock(lockObj) {
				while (buffer.Count > 0) {
					nQueue.Enqueue(buffer.Dequeue());
				}
			}

			return nQueue;
		}
	}
}
