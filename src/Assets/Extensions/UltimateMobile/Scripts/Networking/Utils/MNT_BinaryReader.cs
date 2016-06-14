using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MNT_BinaryReader  {

	protected ByteByffer _ReceivedDate;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public MNT_BinaryReader(byte[] data) {
		_ReceivedDate =  new ByteByffer(data);
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	
	public T ReadValue<T> () {
		if (typeof(T).Equals (typeof(byte))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadByte(), typeof(T));
		} else if (typeof(T).Equals(typeof(int))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadInt(), typeof(T));
		} else if (typeof(T).Equals(typeof(float))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadFloat(), typeof(T));
		} else if (typeof(T).Equals(typeof(long))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadLong(), typeof(T));
		} else if (typeof(T).Equals(typeof(bool))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadBool(), typeof(T));
		} else if (typeof(T).Equals(typeof(short))) {
			return (T)System.Convert.ChangeType(ReceivedDate.ReadShort(), typeof(T));
		} else if (typeof(T).Equals(typeof(string))) {
			int length = ReadValue<int>();
			return (T)System.Convert.ChangeType(ReceivedDate.ReadString(length), typeof(T));
		} else {
			return default(T);
		}
	}
	

	
	public List<T> ReadList<T>() {
		List<T> result = new List<T> ();
		
		int size = ReadValue<int> ();
		for (int i = 0; i < size; i++) {
			result.Add(ReadValue<T>());
		}
		
		return result;
	}
	

	public T[] ReadArray<T>() {
		return ReadList<T> ().ToArray ();
	}
	

	
	public Dictionary<K, V> ReadDictionary<K, V>() {
		Dictionary<K, V> result = new Dictionary<K, V> ();
		
		int size = ReadValue<int> ();
		for (int i = 0; i < size; i++) {
			result.Add(ReadValue<K>(), ReadValue<V>());
		}
		
		return result;
	}



	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	public ByteByffer ReceivedDate {
		get {
			return _ReceivedDate;
		}
	}

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
	
	

}
