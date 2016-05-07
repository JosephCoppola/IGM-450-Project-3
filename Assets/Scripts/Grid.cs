using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
	public DotScript dotPrefab;
	//public float percWhite = 0.1f;

	private Transform[,] gridSpaces;

	void Start()
	{
		EventManager.AddEventListener( "RepopGrid", StartRepop );

		InitGrid();
		SpawnInitialDots();
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			ClearGrid();
			SpawnInitialDots();
		}
	}

	private void StartRepop()
	{
		StartCoroutine( Repopulate() );
	}

	private IEnumerator Repopulate()
	{
		yield return new WaitForSeconds( 0.3f );

		int numSpawned = 0;

		for( int i = 0; i < gridSpaces.GetLength( 0 ); i++ )
		{
			for( int j = gridSpaces.GetLength( 1 ) - 1; j >= 0; j-- )
			{
				Transform gridSpace = gridSpaces[ i, j ];
				if( gridSpace.childCount == 0 )
				{
					bool foundChild = false;
					for( int h = j - 1; h >= 0; h-- )
					{
						Transform nextGridSpace = gridSpaces[ i, h ];
				
						if( nextGridSpace.childCount > 0 )
						{
							foundChild = true;
							nextGridSpace.GetChild( 0 ).parent = gridSpace;
							break;
						}
					}

					if( !foundChild )
					{
						StartCoroutine( FancySpawnDot( gridSpace, numSpawned ) );
						numSpawned++;
						//SpawnDotAtGridSpace( gridSpace );
					}
				}
			}
		}
	}

	private void InitGrid()
	{
		int columns = transform.childCount;
		int rows = transform.GetChild( 0 ).childCount;

		gridSpaces = new Transform[ columns, rows ];

		foreach( Transform column in transform )
		{
			// Strings can be indexed to get chars
			int columnNum = CharToColumnNumber( column.name[ 0 ] );

			// Doing this the long way to guarantee order
			foreach( Transform rowSpace in column )
			{
				int rowNum = int.Parse( rowSpace.name );
				gridSpaces[ columnNum, rowNum ] = rowSpace;
			}
		}

		/*for( int i = 0; i < columns; i++ )
		{
			string output = "[ ";
			for( int j = 0; j < rows; j++ )
			{
				Transform gridSpace = gridSpaces[ i, j ];
				//print( gridSpace.parent.name + gridSpace.name );
				output += gridSpace.parent.name + gridSpace.name + " ";
			}
			output += "]";
			print( output );
		}*/
	}

	private void SpawnInitialDots()
	{
		for( int i = 0; i < gridSpaces.GetLength( 0 ); i++ )
		{
			//for( int j = 0; j < gridSpaces.GetLength( 1 ); j++ )
			for( int j = gridSpaces.GetLength( 1 ) - 1; j >= 0; j-- )
			{
				Transform gridSpace = gridSpaces[ i, j ];
				//SpawnDotAtGridSpace( gridSpace );
				int offset = gridSpaces.GetLength( 1 ) - 1 - j;
				StartCoroutine( FancySpawnDot( gridSpace, i * 5 + offset ) );
				//StartCoroutine( FancySpawnDot( gridSpace, j ) );
			}
		}
	}

	private void SpawnDotAtGridSpace( Transform gridSpace, DotColor.ColorValue color = DotColor.ColorValue.BLACK )
	{
		if( color == DotColor.ColorValue.BLACK )
		{
			color = (DotColor.ColorValue) Random.Range( 0, 3 );
		}

		Vector3 pos = gridSpace.position + Vector3.up * 0.9375f * 5.0f;
		DotScript dot = (DotScript)Instantiate( dotPrefab, pos, Quaternion.identity );
		dot.transform.parent = gridSpace;
		dot.SetColor( color );

		int dragLength = color != DotColor.ColorValue.WHITE ? Random.Range( 1, gridSpaces.GetLength( 0 ) ) : 0;
		dot.SetDragLength( dragLength );
	}

	private IEnumerator FancySpawnDot( Transform gridSpace, int offset )
	{
		yield return new WaitForSeconds( 0.05f * offset );
		SpawnDotAtGridSpace( gridSpace );
	}

	// Converts a char code to a 0-based index, starting at 'A'
	private int CharToColumnNumber( char letter )
	{
		return ( (int)letter - (int)'A' );
	}

	private void ClearGrid()
	{
		for( int i = 0; i < gridSpaces.GetLength( 0 ); i++ )
		{
			for( int j = 0; j < gridSpaces.GetLength( 1 ); j++ )
			{
				Transform gridSpace = gridSpaces[ i, j ];
				if( gridSpace.childCount > 0 )
				{
					Destroy( gridSpace.GetChild( 0 ).gameObject );
				}
			}
		}
	}
}
