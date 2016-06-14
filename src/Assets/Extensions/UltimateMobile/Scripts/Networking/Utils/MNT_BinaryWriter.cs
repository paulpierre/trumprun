using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MNT_BinaryWriter  {

	protected BinaryWriter writer = null;
	protected MemoryStream buffer = null;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	public MNT_BinaryWriter() {
		buffer =  new MemoryStream();
		writer = new BinaryWriter (buffer);
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	public byte[] GetBytes() {

		return buffer.ToArray ();
	}

	public void WriteValue<T> (T data) {
		if (typeof(T).Equals(typeof(byte))) {
			writer.Write (System.Convert.ToByte(data));
		}else if (typeof(T).Equals(typeof(int))) {
			writer.Write (System.Convert.ToInt32(data));
		} else if (typeof(T).Equals(typeof(float))) {
			writer.Write (System.Convert.ToSingle(data));
		} else if (typeof(T).Equals(typeof(long))) {
			writer.Write(System.Convert.ToInt64(data));
		} else if (typeof(T).Equals(typeof(bool))) {
			writer.Write(System.Convert.ToBoolean(data));
		} else if (typeof(T).Equals(typeof(short))) {
			writer.Write(System.Convert.ToInt16(data));
		} else if (typeof(T).Equals(typeof(string))) {
			string strData = System.Convert.ToString(data);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strData);
			WriteValue<int>(bytes.Length);
			writer.Write(bytes);
		} else {
			//Just Empty
		}
	}
		
	public void WriteList<T>(List<T> data) {
		WriteValue<int> (data.Count);
		foreach (T e in data) {
			WriteValue<T>(e);
		}
	}

	public void WriteArray<T>(T[] data) {
		WriteList<T> (new List<T> (data));
	}

	public void WriteDictionary<K, V>(Dictionary<K, V> data) {
		WriteValue<int> (data.Count);
		foreach(KeyValuePair<K, V> pair in data) {
			WriteValue<K>(pair.Key);
			WriteValue<V>(pair.Value);
		}
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
	
	

}
