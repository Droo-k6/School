/**
 * Class to cover the AWS S3 interactions
 */

package cscd467.hw7;

import java.io.File;
import java.util.Enumeration;
import java.util.List;

import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;

import com.amazonaws.AmazonClientException;
import com.amazonaws.auth.profile.ProfileCredentialsProvider;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.s3.AmazonS3;
import com.amazonaws.services.s3.AmazonS3ClientBuilder;
import com.amazonaws.services.s3.model.Bucket;
import com.amazonaws.services.s3.model.GetObjectRequest;
import com.amazonaws.services.s3.model.ListObjectsRequest;
import com.amazonaws.services.s3.model.ObjectListing;
import com.amazonaws.services.s3.model.ObjectMetadata;
import com.amazonaws.services.s3.model.S3ObjectSummary;

class S3Interaction {
	// member variables
	private final GUI _gui;
	private final AmazonS3 _s3;
	private Status _status = Status.IDLE;
	
	/**
	 * Constructor
	 * Established connection
	 */
	public S3Interaction(GUI gui) throws AmazonClientException {
		_gui = gui;
		_s3 = connect("default", Regions.US_WEST_2);
	}
	
	/**
	 * Attempts connection to AmazonS3 w/ profile name, and given region
	 * @param profileName
	 * @param region
	 * @return
	 */
	private static AmazonS3 connect(String profileName, Regions region) throws AmazonClientException {
		// load credentials provider
		ProfileCredentialsProvider credentialsProvider = null;
		try {
			credentialsProvider = new ProfileCredentialsProvider(profileName);
		} catch (Exception e) {
			throw new AmazonClientException(String.format("Cannot load credentials for profile \"%s\".", profileName), e);
		}
		
		// establish connection
		try {
			AmazonS3 s3 = AmazonS3ClientBuilder.standard().withCredentials(credentialsProvider).withRegion(region).build();
			return s3;
		}catch(Exception e) {
			throw new AmazonClientException("Failed to build s3 client.", e);
		}
	}
	
	/**
	 * Get status of s3 connection
	 * @return
	 */
	public synchronized Status getStatus() {
		return _status;
	}
	
	/**
	 * Set status of s3 connection
	 * @param status
	 */
	private synchronized void setStatus(Status status) {
		_status = status;
	}
	
	/**
	 * Builds tree from current bucket setup
	 * @return
	 */
	public DefaultTreeModel buildTreeModel() {
		// set status to updating tree
		synchronized(this) {
			try {
				// wait
				while(getStatus() != Status.IDLE) {
					wait();
				}
			} catch (InterruptedException e) {
				System.err.printf("build tree interrupted while waiting for idle status: \"%s\"%n", e.getMessage());
				return null;
			}
			setStatus(Status.UPDATING);
		}
		
		// get location
		String rootName = String.format("Buckets(\"%s\")", _s3.getRegionName());
		
		// root node
		DefaultMutableTreeNode root = new DefaultMutableTreeNode(rootName);
		
		DefaultTreeModel model = new DefaultTreeModel(root);
		
		// check if buckets available
		List<Bucket> buckets = _s3.listBuckets();
		if (buckets.size() <= 0) {
			// no buckets available
			DefaultMutableTreeNode noBuckets = new DefaultMutableTreeNode("No buckets available ...");
			noBuckets.setAllowsChildren(false);
			root.add(noBuckets);
		}else{
			// add all buckets
			for(Bucket b : buckets) {
				DefaultMutableTreeNode node = buildBucketNode(b);
				root.add(node);
			}
		}
		
		setStatus(Status.IDLE);
		
		return model;
	}
	
	/**
	 * Builds tree node for a bucket
	 * @param bucket
	 * @return
	 */
	private DefaultMutableTreeNode buildBucketNode(Bucket bucket) {
		String bucketName = bucket.getName();
		DefaultMutableTreeNode node;
		
		// check if within same region
		String regionStr = _s3.getBucketLocation(bucketName);
		
		if (!regionStr.equalsIgnoreCase(_s3.getRegionName())) {
			String nodeName = String.format("[Different region(\"%s\")] %s", regionStr, bucketName);
			node = new DefaultMutableTreeNode(nodeName, false);
			return node;
		}
		
		node = new DefaultMutableTreeNode(bucketName);
		
		// get listing
		//ObjectListing objectListing = _s3.listObjects(new ListObjectsRequest().withBucketName(bucketName).withPrefix("My"));
		ListObjectsRequest request = new ListObjectsRequest().withBucketName(bucketName);
		ObjectListing listing = _s3.listObjects(request);
		
		while(true) {
			// go through listing
			for (S3ObjectSummary summary : listing.getObjectSummaries()) {
				// build child
				buildChildNode(node, summary.getKey());
			}
			
			// check if additional listings available
			if (listing.isTruncated()) {
				listing = _s3.listNextBatchOfObjects(listing);
			}else{
				break;
			}
		}
		
		return node;
	}
	
	/**
	 * Builds child node for given key (path)
	 * @param bucketNode
	 * @param key
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	private void buildChildNode(DefaultMutableTreeNode bucketNode, String key) {
		// determine if is a folder
		boolean isFolder = key.charAt(key.length() - 1) == '/';
		// split key to path
		String[] path = key.split("/");
		String name = path[path.length-1];
		
		// create child
		DefaultMutableTreeNode child = new DefaultMutableTreeNode(name, isFolder);
		
		// determine node to add to
		DefaultMutableTreeNode trav = bucketNode;
		int curPath = 0;
		while(curPath < (path.length - 1)) {
			boolean foundPath = false;
			for(Enumeration children = trav.children(); children.hasMoreElements();) {
				DefaultMutableTreeNode travChild = (DefaultMutableTreeNode) children.nextElement();
				String childName = (String) travChild.getUserObject();
				if (childName.equalsIgnoreCase(path[curPath])) {
					foundPath = true;
					trav = travChild;
					curPath++;
					break;
				}
			}
			// check if a path was found
			if (!foundPath) {
				// create path now
				DefaultMutableTreeNode pathNode = new DefaultMutableTreeNode(path[curPath]);
				trav.add(pathNode);
				curPath++;
			}
		}
		
		// add node
		trav.add(child);
		
	}
	
	/**
	 * Check if a file can be uploaded to the given path and if the status allows so
	 * @param path
	 * @return
	 */
	public boolean canUpload(TreePath path) {
		// check status
		if (getStatus() != Status.IDLE) {
			return false;
		}
		
		// check if a path exists
		if (path == null) {
			return false;
		}
		
		// check if root		
		if (path.getParentPath() == null) {
			return false;
		}
		
		// check that is an item, or invalid bucket (no children allowed)
		DefaultMutableTreeNode node = (DefaultMutableTreeNode) path.getLastPathComponent();
		if (!node.getAllowsChildren()) {
			return false;
		}
		
		return true;
	}
	
	/**
	 * Attempt to upload given file
	 * @param path
	 * @param name
	 * @param file
	 * @return
	 */
	public boolean upload(TreePath path, String name, File file) {
		// check if can upload, set status
		synchronized(this) {
			if (!canUpload(path)) {
				return false;
			}
			setStatus(Status.UPLOADING);
		}
		
		// upload file
		_gui.setStatus(String.format("Uploading \"%s\"", name));
		
		// build key
		PathInfo info = new PathInfo(path, name);
		
		// check if key exists already, prompt ?
		
		// upload to bucket
		if (!uploadFile(info.getBucketName(), info.getKey(), file)) {
			// if failed
			_gui.setStatus(String.format("Failed to upload \"%s\"", name));
			setStatus(Status.IDLE);
			return false;
		}
		
		// update tree if necessary
		_gui.createNode(path, name);
		
		_gui.setStatus(String.format("Uploaded \"%s\"", name));
		setStatus(Status.IDLE);
		
		return true;
	}
	
	/**
	 * Upload file to bucket w/ given information
	 * @param bucketName
	 * @param key
	 * @param file
	 * @return
	 */
	private boolean uploadFile(String bucketName, String key, File file) {
		// catch any exceptions
		try {
			_s3.putObject(bucketName, key, file);
			return true;
		}catch(Exception e) {
			// handle issue
			System.err.printf("Failed to build / process put request (\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			return false;
		}
	}
	
	/**
	 * Check if the path is valid for a download
	 * @param path
	 * @return
	 */
	public boolean canDownload(TreePath path) {
		// check status
		if (getStatus() != Status.IDLE) {
			return false;
		}
		
		// check a path is available
		if (path == null) {
			return false;
		}
		
		// check if is allowed to have children (folders)
		DefaultMutableTreeNode node = (DefaultMutableTreeNode) path.getLastPathComponent();
		if (node.getAllowsChildren()) {
			return false;
		}
		
		// check that not an invalid bucket
		if (node.getLevel() < 2) {
			return false;
		}
		
		return true;
	}
	
	/**
	 * 
	 * @param path
	 * @return
	 */
	public boolean download(TreePath path) {
		// check if can download, set status
		synchronized(this) {
			if (!canDownload(path)) {
				return false;
			}
			setStatus(Status.DOWNLOADING);
		}
		
		// parse information, build key
		PathInfo info = new PathInfo(path);
		String name = info.getName();
		
		// upload file
		_gui.setStatus(String.format("Downloading \"%s\"", name));
		
		// determine download path
		String filePath = String.format("./downloads/%s", name);
		
		// download
		if (!downloadFile(info.getBucketName(), info.getKey(), filePath)) {
			// if failed
			_gui.setStatus(String.format("Failed to download \"%s\"", name));
			setStatus(Status.IDLE);
			return false;
		}
		
		_gui.setStatus(String.format("Downloaded \"%s\"", name));
		_gui.info(String.format("Downloaded to \"%s\"", filePath));
		setStatus(Status.IDLE);
		
		return true;
	}
	
	/**
	 * Grab object and store to filename/path
	 * @param bucketName
	 * @param key
	 * @param destination
	 * @return
	 */
	@SuppressWarnings("unused")
	public boolean downloadFile(String bucketName, String key, String destination) {
		try {
			File file = new File(destination);
			GetObjectRequest getRequest = new GetObjectRequest(bucketName, key);
			ObjectMetadata meta = _s3.getObject(getRequest, file);
			
			return true;
		}catch(Exception e) {
			// handle issue
			String msg = String.format("Failed to download(\"%s\") to \"%s\"%n(\"%s\"): \"%s\"", key, destination, e.getClass(), e.getMessage());
			System.err.println(msg);
			_gui.warning(msg);
			return false;
		}
		
	}
	
}
