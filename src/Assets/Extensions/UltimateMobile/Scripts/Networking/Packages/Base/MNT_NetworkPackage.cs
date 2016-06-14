////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class MNT_NetworkPackage  {

	protected MemoryStream buffer = null;
	protected BinaryWriter writer = null;

	protected ByteByffer _ReceivedDate;

	private int _Id;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public MNT_NetworkPackage(int id) {
		buffer =  new MemoryStream();
		writer = new BinaryWriter (buffer);

		_Id = id;
		WriteValue<int>(id);
	}

	public MNT_NetworkPackage(byte[] data) {

		_ReceivedDate =  new ByteByffer(data);
		_Id = ReadValue<int>();

	}
	

	public byte[] GetBytes() {
		if(buffer == null) {
			return _ReceivedDate.buffer;
		} else {
			return buffer.ToArray ();
		}
	}

	public string GetBase64DataFormat() {
		return System.Convert.ToBase64String(GetBytes());
	}
	

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	

	public void WriteValue<T> (T data) {


		if (typeof(T).Equals (typeof(byte))) {
			writer.Write (System.Convert.ToByte (data));
		} else if (typeof(T).Equals (typeof(int))) {
			writer.Write (System.Convert.ToInt32 (data));
		} else if (typeof(T).Equals (typeof(float))) {
			writer.Write (System.Convert.ToSingle (data));
		} else if (typeof(T).Equals (typeof(long))) {
			writer.Write (System.Convert.ToInt64 (data));
		} else if (typeof(T).Equals (typeof(bool))) {
			writer.Write (System.Convert.ToBoolean (data));
		} else if (typeof(T).Equals (typeof(short))) {
			writer.Write (System.Convert.ToInt16 (data));
		} else if (typeof(T).Equals (typeof(string))) {
			string strData = System.Convert.ToString (data);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes (strData);
			WriteValue<int> (bytes.Length);
			writer.Write (bytes);
		} else if (typeof(T).Equals (typeof(Vector3))) {
			Vector3 vect = (Vector3) System.Convert.ChangeType(data, typeof(Vector3));

			writer.Write(vect.x);
			writer.Write(vect.y);
			writer.Write(vect.z);
		} else {


			Type ObjectType = typeof(T);
			while(ObjectType != null) {
				if(ObjectType.Equals (typeof(MNT_NetworkPackage))) {
					WriteMNTPack(data as MNT_NetworkPackage);
					return;
				}
				ObjectType = ObjectType.BaseType;
			}
			Debug.Log("MNT_NetworkPackage::WriteValue Unsupported Type Ignored " + typeof(T));
		}
	}

	private void WriteMNTPack(MNT_NetworkPackage pack) {
		byte[] bytes = pack.GetBytes();
		WriteArray<byte>(bytes);
	}

	public Vector3 ReadVector3() {
		Vector3 vect =  new Vector3();
		
		vect.x = ReadValue<float>();
		vect.y = ReadValue<float>();
		vect.z = ReadValue<float>();

		return vect;
	}

	public MNT_NetworkPackage ReadNetworkPackage() {
		byte[] bytes = ReadArray<byte>();
		MNT_NetworkPackage pack =  new MNT_NetworkPackage(bytes);

		return pack;
	}

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

		} else if (typeof(T).Equals (typeof(MNT_NetworkPackage))) {
			byte[] bytes = ReadArray<byte>();
			MNT_NetworkPackage pack =  new MNT_NetworkPackage(bytes);
			return (T)System.Convert.ChangeType(pack, typeof(T));

		} else if (typeof(T).Equals (typeof(Vector3))) {
			Vector3 vect =  new Vector3();
			
			vect.x = ReadValue<float>();
			vect.y = ReadValue<float>();
			vect.z = ReadValue<float>();
			return (T)System.Convert.ChangeType(vect, typeof(T));
		} else {
			return default(T);
		}
	}


	public void WriteList<T>(List<T> data) {
		WriteValue<int> (data.Count);
		foreach (T e in data) {
			WriteValue<T>(e);
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
	

	public void WriteArray<T>(T[] data) {
		WriteList<T> (new List<T> (data));
	}

	public T[] ReadArray<T>() {
		return ReadList<T> ().ToArray ();
	}

	public void WriteDictionary<K, V>(Dictionary<K, V> data) {
		WriteValue<int> (data.Count);
		foreach(KeyValuePair<K, V> pair in data) {
			WriteValue<K>(pair.Key);
			WriteValue<V>(pair.Value);
		}
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
	//  GET/SET
	//--------------------------------------

	public int Id {
		get {
			return _Id;
		}
	}

	public ByteByffer ReceivedDate {
		get {
			return _ReceivedDate;
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
