/**
 * Manager for threadpool
 * @author Mathew McCain
 */

package cscd467.hw45;

class ThreadManager {
	// member variables
	private final JobQueue _queue;
	private final ThreadPool _pool;
	private final int _threshhold1, _threshhold2, _interval;
	private ThreshholdState _currentThreshhold = ThreshholdState.NONE;
	private double _lastCheck;
	
	/**
	 * Constructor
	 * @param queue, job queue
	 * @param pool, threadpool
	 * @param t1, threshhold 1
	 * @param t2, threshhold 2
	 * @param v, manager interval (in seconds)
	 */
	public ThreadManager(JobQueue queue, ThreadPool pool, int t1, int t2, int v) {
		_queue = queue;
		_pool = pool;
		_threshhold1 = t1;
		_threshhold2 = t2;
		_interval = v * 1000;
		_lastCheck = System.currentTimeMillis();
	}
	
	/**
	 * Run manager, will check if time since last check have reached the interval set
	 */
	public void run() {
		// check if interval reached
		long currentTime = System.currentTimeMillis();
		if (currentTime >= (_lastCheck + _interval)) {
			manage();
			_lastCheck = currentTime;
		}
	}
	
	/**
	 * Manager the threadpool
	 */
	private void manage() {
		// check number of jobs on queue
		int jobs = _queue.size();
		// check state, transition as necessary
		switch(_currentThreshhold) {
		case NONE:
			// no threshhold reached, transition ?
			if (jobs > _threshhold1 && jobs <= _threshhold2) {
				toThreshhold1();
			}else if(jobs > _threshhold2) {
				toThreshhold2();
			}else{
				toThreshhold0();
			}
			break;
		case ONE:
			// transition from threshhold 1 ?
			if(jobs <= _threshhold1) {
				toThreshhold0();
			}else if (jobs > _threshhold2) {
				toThreshhold2();
			}
			break;
		case TWO:
			// transition from threshhold 2 ?
			if(jobs <= _threshhold1) {
				toThreshhold0();
			}else if (jobs > _threshhold1 && jobs <= _threshhold2) {
				toThreshhold1();
			}else{
				toThreshhold2();
			}
			break;
		}
	}
	
	/**
	 * Switch to no threshhold from any state
	 */
	private void toThreshhold0() {
		// check current state
		switch(_currentThreshhold){
		case NONE:
			// half worker count
			Log.log("Manager(threshhold 0): halfing worker count");
			_pool.decreaseThreads(_pool.size() / 2);
			_currentThreshhold = ThreshholdState.NONE;
			break;
		case ONE:
			// half worker count
			Log.log("Manager(threshhold 1 to 0): halfing worker count");
			_pool.decreaseThreads(_pool.size() / 2);
			_currentThreshhold = ThreshholdState.NONE;
			break;
		case TWO:
			// to threshhold 1
			toThreshhold1();
			break;
		}
	}
	
	/**
	 * Switch to threshhold 1 from any state
	 */
	private void toThreshhold1() {
		// check current state
		switch(_currentThreshhold){
		case NONE:
			// double worker count
			Log.log("Manager(threshhold 0 to 1): doubling worker count");
			_pool.increaseThreads(_pool.size() * 2);
			_currentThreshhold = ThreshholdState.ONE;
			break;
		case ONE:
			// do nothing
			break;
		case TWO:
			// half worker count
			Log.log("Manager(threshhold 2 to 1): halfing worker count");
			_pool.decreaseThreads(_pool.size() / 2);
			_currentThreshhold = ThreshholdState.ONE;
			break;
		}
	}
	
	/**
	 * Switch to threshhold 2 from any state
	 * Or if at threshhold 2, keep increasing the worker count
	 */
	private void toThreshhold2() {
		// check current state
		switch(_currentThreshhold){
		case NONE:
			// to threshhold 1
			toThreshhold1();
			break;
		case ONE:
			// double worker count
			Log.log("Manager(threshhold 1 to 2): doubling worker count");
			_pool.increaseThreads(_pool.size() * 2);
			_currentThreshhold = ThreshholdState.TWO;
			break;
		case TWO:
			// double worker count
			Log.log("Manager(threshhold 2): doubling worker count");
			_pool.increaseThreads(_pool.size() * 2);
			break;
		}
		
	}
	
	/**
	 * Enum for the current threshhold
	 * NONE - less than threshhold 1
	 * ONE - between theshhold 1 and 2
	 * TWO - reached threshhold 2
	 */
	static enum ThreshholdState {
		NONE, ONE, TWO;
	}
}
