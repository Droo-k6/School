/**
 * Worker thread
 * @author Mathew McCain
 */

package cscd467.hw45;

class Worker extends Thread {
	// member variables
	private final long _id;
	private final JobQueue _queue;
	private boolean _waiting = false;
	private final int _slowdown;
	
	/**
	 * Constructor
	 * @param id, id of thread
	 * @param queue, job queue to be used
	 * @param slowdown, minimum delay in seconds between jobs (for testing)
	 */
	public Worker(long id, JobQueue queue, int slowdown) {
		_id = id;
		_queue = queue;
		_slowdown = slowdown * 1000;
	}
	
	/**
	 * Run method for thread
	 * Sychronized so the Worker can be checked if its in a waiting state
	 */
	@Override
	public void run() {
		// Worker loop
		try {
			while(!isInterrupted()) {
				synchronized(this) {
					_waiting = true;
				}
				// get next job, execute
				Job job = _queue.dequeue();
				synchronized(this) {
					_waiting = false;
				}
				// execute job
				Log.logf("Worker(%d): running job%n", _id);
				job.run();
				Log.logf("Worker(%d): finished job%n", _id);
				
				if (_slowdown > 0) {
					Log.logf("Worker(%d): sleeping%n", _id);
					sleep(_slowdown);
				}
			}
		} catch (InterruptedException e) {
			Log.logf("Worker(%d): interrupted%n", _id);
		}
	}
	
	/**
	 * Check if the worker is waiting for a new job
	 * @return
	 */
	public synchronized boolean isWaiting() {
		return _waiting;
	}
}
