/**
 * Queue for jobs, acts as a monitor for the worker threads
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.util.LinkedList;
import java.util.Queue;

class JobQueue {
	// member variables
	private final int _maxJobs;
	private final Queue<Job> _queue;
	
	/**
	 * Constructor
	 * @param maxJobs, max amount of jobs
	 */
	public JobQueue(int maxJobs) {
		_maxJobs = maxJobs;
		_queue = new LinkedList<Job>();
	}
	
	/**
	 * Enqueue a new job
	 * @param job, job to enqueue
	 * @return if was enqueued successfully
	 */
	public synchronized boolean enqueue(Job job) {
		if (isFull()) {
			return false;
		}
		_queue.add(job);
		notifyAll();
		return true;
	}
	
	/**
	 * Dequeue the next job
	 * @return the next job
	 * @throws InterruptedException 
	 */
	public synchronized Job dequeue() throws InterruptedException {
		while(isEmpty()) {
			wait();
		}
		return _queue.remove();
	}
	
	/**
	 * Check if the queue is empty
	 * @return
	 */
	public synchronized boolean isEmpty() {
		return (_queue.isEmpty());
	}
	
	/**
	 * check if the queue is full
	 * @return
	 */
	public synchronized boolean isFull() {
		return (_queue.size() == _maxJobs);
	}
	
	/**
	 * Get size of the queue
	 * @return
	 */
	public synchronized int size() {
		return _queue.size();
	}
	
	/**
	 * Get max size of the queue
	 * @return
	 */
	public synchronized int getMax() {
		return _maxJobs;
	}
}
