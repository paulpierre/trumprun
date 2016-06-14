////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;


public class ByteByffer  {

	private byte[] _buffer;
	public int pointer = 0;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public ByteByffer(byte[] buf) {
		_buffer = buf;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public byte ReadByte() {
		byte v = _buffer [pointer];
		pointer += 1;
		return v;
	}

	public int ReadInt() {
		int v =  System.BitConverter.ToInt32 (_buffer, pointer);
		pointer += 4;
		return v;
	}

	public float ReadFloat() {
		float v =  System.BitConverter.ToSingle (_buffer, pointer);
		pointer += 4;
		return v;
	}


	public long ReadLong() {
		long v =  System.BitConverter.ToInt64 (_buffer, pointer);
		pointer += 8;
		return v;
	}


	public bool ReadBool() {
		bool v =  System.BitConverter.ToBoolean (_buffer, pointer);
		pointer += 1;
		return v;
	}


	public short ReadShort() {
		short v =  System.BitConverter.ToInt16 (_buffer, pointer);
		pointer += 2;
		return v;
	}


	public string ReadString(int length) {


		byte[] array1 = new byte[length];

		System.Buffer.BlockCopy(_buffer, pointer, array1, 0, length);
		//System.BitConverter.GetBytes()
		//string v =  System.BitConverter.ToString(buffer, pointer, length);


		pointer += length;
		return System.Text.Encoding.UTF8.GetString(array1);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

	public byte[] buffer {
		get {
			return _buffer;
		}
	}
}
