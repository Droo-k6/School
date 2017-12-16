/**
 * Class to handle a pool of threads
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.util.ArrayList;

class ThreadPool {
	// member variables
	private final int _minWorkers, _maxWorkers;
	private final ArrayList<Worker> _workers;
	private final JobQueue _queue;
	private final int _slowdown;
	private long _threadID = 1;
	
	private boolean _running = false;
	
	/**
	 * Constructor
	 * @param queue, job queue
	 * @param minWorkers, minimum amount of workers
	 * @param maxWorkers, maximum amount of workers
	 * @param slowdown, slowdow time (seconds) for worker between jobs
	 */
	public ThreadPool(JobQueue queue, int minWorkers, int maxWorkers, int slowdown) {
		_queue = queue;
		_minWorkers = minWorkers;
		_maxWorkers = maxWorkers;
		_workers = new ArrayList<Worker>();
		_slowdown = slowdown;
	}
	
	/**
	 * Increase number of threads in pool by amount
	 * @param count, amount to increase by
	 */
	public void increaseThreads(int count) {
		// check that will not go over max
		// determine amount to make
		int size = Math.min(_maxWorkers, _workers.size()+count);
		int diff = size - _workers.size();
		
		if (diff <= 0) {
			Log.log("Pool: not increasing workers above maximum");
			return;
		}
		
		// add new workers
		for (int i = 0; i < diff; ++i) {
			Log.log("Worker created");
			Worker w = new Worker(_threadID++, _queue, _slowdown);
			w.start();
			_workers.add(w);
		}
		
		Log.logf("Pool: workers increased to %d%n", size());
	}
	
	/**
	 * Decrease number of threads in pool by amount
	 * @param count, amount to decrease by
	 */
	public void decreaseThreads(int count) {
		// check that will not go under minimum
		// determine amount to remove
		int size = Math.max(_minWorkers, _workers.size()-count);
		int diff = _workers.size() - size;
		
		if (diff <= 0) {
			Log.log("Pool: not decreasing workers below minimum");
			return;
		}
		
		Log.logf("Pool: decreasing workers to %d%n", size);
		
		// find idle workers to remove
		ArrayList<Worker> toRemove = new ArrayList<Worker>();
		for(Worker w : _workers) {
			if (diff <= 0) {
				break;
			}
			if (w.isInterrupted() || w.isWaiting()) {
				w.interrupt();
				toRemove.add(w);
				diff--;
			}
		}
		
		// if still have to remove more, grab from front of list
		for(Worker w : _workers) {
			if (toRemove.contains(w)) {
				continue;
			}
			w.interrupt();
			toRemove.add(w);
			diff--;
			if (diff <= 0) {
				break;
			}
		}
		
		// join/remove workers
		for(Worker w : toRemove) {
			try {
				Log.log("Removing worker");
				_workers.remove(w);
				w.join();
			} catch (InterruptedException e) {
				Log.elog("Interrupted while joining a worker thread");
			}
		}
		
		Log.logf("Pool: workers decreased to %d%n", size());
	}
	
	/**
	 * Start the pool
	 */
	public void start() {
		if (_running) {
			throw new IllegalStateException("Pool already running");
		}
		increaseThreads(_minWorkers);
		_running = true;
	}
	
	/**
	 * Stop the pool
	 */
	public void stop() {
		if (!_running) {
			throw new IllegalStateException("Pool not running");
		}
		
		// stop all workers
		for(Worker w : _workers) {
			w.interrupt();
		}
		// join/remove all workers
		while(_workers.size() > 0) {
			Worker w = _workers.remove(0);
			try {
				w.join();
			} catch (InterruptedException e) {
				Log.elog("Interrupted while joining worker thread for stopping pool");
			}
		}
		
		_running = false;
	}
	
	/**
	 * Get number of workers
	 * @return
	 */
	public int size() {
		return _workers.size();
	}
	
	/**
	 * Get minimum amount of workers
	 * @return
	 */
	public int getMin() {
		return _minWorkers;
	}
	
	/**
	 * Get maximum amount of workers
	 * @return
	 */
	public int getMax() {
		return _maxWorkers;
	}
}
