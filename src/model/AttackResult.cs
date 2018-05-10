
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// AttackResult gives the result after a shot has been made
[Serializable]
public class AttackResult
{
	private ResultOfAttack _Value;
	private Ship _Ship;
	private string _Text;
	private int _Row;

	private int _Column;

	public ResultOfAttack Value {
		get { return _Value; }
	}

	// The ships, if any, involved in this result
	public Ship Ship {
		get { return _Ship; }
	}

	// A textual description of the result
	public string Text {
		get { return _Text; }
	}

	public int Row {
		get { return _Row; }
	}

	public int Column {
		get { return _Column; }
	}

	// Set the _Value to the PossibleAttack value
	public AttackResult(ResultOfAttack value, string text, int row, int column)
	{
		_Value = value;
		_Text = text;
		_Ship = null;
		_Row = row;
		_Column = column;
	}

	// Set the _Value to the PossibleAttack value, and the _Ship to the Ship
	public AttackResult(ResultOfAttack value, Ship ship, string text, int row, int column) : this(value, text, row, column)
	{
		_Ship = ship;
	}

	// Displays the textual information about the attack
	public override string ToString()
	{
		if (_Ship == null) {
			return Text;
		}

		return Text + " " + _Ship.Name;
	}
}