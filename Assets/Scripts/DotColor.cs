using UnityEngine;
using System.Collections.Generic;

public class DotColor
{
	public struct BlendPair
	{
		public ColorValue color1;
		public ColorValue color2;

		public BlendPair( ColorValue color1, ColorValue color2 )
		{
			this.color1 = color1;
			this.color2 = color2;
		}
	}

	public enum ColorValue
	{
		RED,
		BLUE,
		YELLOW,
		PRIMARY_COLORS,

		PURPLE = PRIMARY_COLORS,
		GREEN,
		ORANGE,
		SECONDARY_COLORS,

		BLACK = SECONDARY_COLORS,
		WHITE,
		NUM_COLORS
	}

	public static readonly Color[] colors = new Color[] {
		HexToRGB( "FF4D4D" ),
		HexToRGB( "5C85D6" ),
		HexToRGB( "FFE066" ),
		HexToRGB( "DF80FF" ),
		HexToRGB( "40BF80" ),
		HexToRGB( "FFA64D" ),
		HexToRGB( "333333" ),
		HexToRGB( "FFFFFF" )
	};

	private static readonly Dictionary<BlendPair, ColorValue> ms_blendTable;

	static DotColor()
	{
		ms_blendTable = new Dictionary<BlendPair, ColorValue>();

		ms_blendTable.Add( new BlendPair( ColorValue.BLUE, ColorValue.RED ), ColorValue.PURPLE );
		ms_blendTable.Add( new BlendPair( ColorValue.RED, ColorValue.BLUE ), ColorValue.PURPLE );

		ms_blendTable.Add( new BlendPair( ColorValue.BLUE, ColorValue.YELLOW ), ColorValue.GREEN );
		ms_blendTable.Add( new BlendPair( ColorValue.YELLOW, ColorValue.BLUE ), ColorValue.GREEN );

		ms_blendTable.Add( new BlendPair( ColorValue.RED, ColorValue.YELLOW ), ColorValue.ORANGE );
		ms_blendTable.Add( new BlendPair( ColorValue.YELLOW, ColorValue.RED ), ColorValue.ORANGE );

		ms_blendTable.Add( new BlendPair( ColorValue.RED, ColorValue.WHITE ), ColorValue.RED );
		ms_blendTable.Add( new BlendPair( ColorValue.BLUE, ColorValue.WHITE ), ColorValue.BLUE );
		ms_blendTable.Add( new BlendPair( ColorValue.YELLOW, ColorValue.WHITE ), ColorValue.YELLOW );

		ms_blendTable.Add( new BlendPair( ColorValue.RED, ColorValue.RED ), ColorValue.RED );
		ms_blendTable.Add( new BlendPair( ColorValue.BLUE, ColorValue.BLUE ), ColorValue.BLUE );
		ms_blendTable.Add( new BlendPair( ColorValue.YELLOW, ColorValue.YELLOW ), ColorValue.YELLOW );
	}

	public static Color GetColor( ColorValue color )
	{
		return colors[ (int)color ];
	}

	public static ColorValue GetBlend( ColorValue color1, ColorValue color2 )
	{
		BlendPair bp = new BlendPair( color1, color2 );

		if( ms_blendTable.ContainsKey( bp ) )
		{
			return ms_blendTable[ bp ];
		}
		else
		{
			return ColorValue.WHITE;
		}
	}

	// Functions from http://wiki.unity3d.com/index.php?title=HexConverter
	private static int HexToInt ( char hexChar )
	{
		string hex = "" + hexChar;
		switch ( hex )
		{
		case "0": return 0;
		case "1": return 1;
		case "2": return 2;
		case "3": return 3;
		case "4": return 4;
		case "5": return 5;
		case "6": return 6;
		case "7": return 7;
		case "8": return 8;
		case "9": return 9;
		case "A": return 10;
		case "B": return 11;
		case "C": return 12;
		case "D": return 13;
		case "E": return 14;
		case "F": return 15;
		default: return 0;
		}
	}

	private static Color HexToRGB ( string color )
	{
		float red = ( HexToInt( color[ 1 ] ) + HexToInt( color[ 0 ] ) * 16.0f ) / 255.0f;
		float green = ( HexToInt( color[ 3 ] ) + HexToInt( color[ 2 ] ) * 16.0f ) / 255.0f;
		float blue = ( HexToInt( color[ 5 ] ) + HexToInt( color[ 4 ] ) * 16.0f ) / 255.0f;
		Color finalColor = new Color();
		finalColor.r = red;
		finalColor.g = green;
		finalColor.b = blue;
		finalColor.a = 1.0f;
		return finalColor;
	}
}
