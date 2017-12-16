/**
 * Status enum for the state of the S3 connection
 */

package cscd467.hw7;

/*
 * IDLE - doing nothing, can upload
 * UPDATING - updating tree model, must wait
 * UPLOADING - uploading a file, must wait
 * DOWNLOADING - downloading a file, must wait
 */
enum Status {
	IDLE, UPDATING, UPLOADING, DOWNLOADING;
}
