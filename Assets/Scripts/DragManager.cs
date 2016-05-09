using UnityEngine;
using System.Collections.Generic;

public class DragManager : MonoBehaviour
{
	public const int MAX_DRAG_LENGTH = 4;

	public LayerMask neighborSearchLayerMask;
	public GameObject boxPrefab;

	private DotScript[] draggedDots;
	private List<DotScript> draggableNeighbors;

	private GameObject[] dragLines;
	private GameObject[] toNeighborLines;

	private Color toNeighborLineColor;

	private int currDragLength = 0;
	private int maxDragLength;

	private float neighborSearchRadius = 0.85f;

	private bool dragging;

	public static DragManager Instance
	{
		get;
		private set;
	}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		toNeighborLineColor = DotColor.GetColor( DotColor.ColorValue.BLACK );
		toNeighborLineColor.a = 0.2f;

		draggableNeighbors = new List<DotScript>();
	}

	public void StartDrag( DotScript startingDot )
	{
		if( dragging )
		{
			// Early return
			return;
		}
		
		dragging = true;

		currDragLength = 0;
		maxDragLength = startingDot.DragLength;
		dragLines = new GameObject[ maxDragLength + 1 ];
		draggedDots = new DotScript[ maxDragLength + 1 ]; // +1 to also hold start
		draggedDots[ 0 ] = startingDot;

		toNeighborLines = new GameObject[ 0 ];

		GetDraggableNeighbors();
		//SpawnDragLine();
	}

	public void DragOverDot( DotScript dotToAdd )
	{
		if( !dragging )
		{
			// early return
			return;
		}

		if( currDragLength > 0 && dotToAdd == draggedDots[ currDragLength - 1 ] )
		{
			RemoveLastDot();
			// also early return
			return;
		}

		if( currDragLength < maxDragLength )
		{
			if( draggableNeighbors.Contains( dotToAdd ) )
			{
				SpawnDragLine( dotToAdd.transform.position );
				//DrawDragLine( dotToAdd.transform.position );
				currDragLength++;
				draggedDots[ currDragLength ] = dotToAdd;
				dotToAdd.MarkToBlend( draggedDots[ 0 ].ColorValue );
				draggedDots[ 0 ].SetDragLength( draggedDots[ 0 ].DragLength - 1 );

				ClearToNeighborLines();

				if( currDragLength < maxDragLength )
				{
					//SpawnDragLine();
					GetDraggableNeighbors();
				}
			}
		}
	}

	public void EndDrag()
	{
		if( !dragging )
		{
			// Early return
			return;
		}

		DotScript startDot = draggedDots[ 0 ];

		for( int i = 1; i < draggedDots.Length; i++ )
		{
			DotScript dot = draggedDots[ i ];
			if( dot == null )
			{
				break;
			}

			if( dot.ColorValue == startDot.ColorValue )
			{
				dot.SetDragLength( dot.DragLength + 1 );
				dot.SetColor( dot.ColorValue );
				continue;
			}
			else if( dot.ColorValue == DotColor.ColorValue.WHITE )
			{
				dot.SetDragLength( 1 );
			}

			DotColor.ColorValue newColor = DotColor.GetBlend( startDot.ColorValue, dot.ColorValue );
			dot.SetColor( newColor );
		}
		
		//int newLength = startDot.DragLength - currDragLength;
		//startDot.SetDragLength( newLength );
		if( startDot.DragLength <= 0 )
		{
			//startDot.SetColor( DotColor.ColorValue.WHITE );
			Destroy( startDot.gameObject );
			EventManager.TriggerEvent( "RepopGrid" );
		}

		ClearDragLines();
		ClearToNeighborLines();

		dragging = false;
	}

	private void GetDraggableNeighbors()
	{
		draggableNeighbors.Clear();
		draggableNeighbors = new List<DotScript>( 4 ); // Only 4 possible neighbors

		DotScript currDot = draggedDots[ currDragLength ];
		Vector2 pos = currDot.transform.position;

		Collider2D[] cols = Physics2D.OverlapCircleAll( pos, neighborSearchRadius, neighborSearchLayerMask );
		for( int i = 0; i < cols.Length; i++ )
		{
			DotScript dot = cols[ i ].gameObject.GetComponent<DotScript>();

			if( dot == currDot )
			{
				// Don't include the one we're dragging
				continue;
			}
				
			if( IsDraggableNeighbor( dot, draggedDots[ 0 ].ColorValue ) )
			{
				draggableNeighbors.Add( dot );
			}
		}

		ClearToNeighborLines();
		SpawnToNeighborLines();
	}

	private bool IsDraggableNeighbor( DotScript dot, DotColor.ColorValue dragColor )
	{
		bool isPrimary = ( dot.ColorValue < DotColor.ColorValue.PRIMARY_COLORS );
		bool stackFull = ( dot.DragLength >= MAX_DRAG_LENGTH ) && ( dot.ColorValue == dragColor );

		if( currDragLength > 0 )
		{
			if( dot == draggedDots[ currDragLength - 1 ] )
			{
				return false;
			}
		}

		for( int i = currDragLength - 2; i >= 0; i-- )
		{
			if( dot == draggedDots[ i ] )
			{
				return false;
			}
		}

		return ( isPrimary && !stackFull );
	}

	private void RemoveLastDot()
	{
		DotScript startDot = draggedDots[ 0 ]; 
		startDot.SetDragLength( startDot.DragLength + 1 );
		ClearToNeighborLines();
		draggedDots[ currDragLength ].Unmark();
		draggedDots[ currDragLength ] = null;
		currDragLength--;
		Destroy( dragLines[ currDragLength ] );

		GetDraggableNeighbors();
	}

	private void SpawnToNeighborLines()
	{
		toNeighborLines = new GameObject[ draggableNeighbors.Count ];
		for( int i = 0; i < draggableNeighbors.Count; i++ )
		{
			Vector2 targetPos = draggableNeighbors[ i ].transform.position;
			toNeighborLines[ i ] = SpawnLine( targetPos, toNeighborLineColor );
		}
	}

	private void ClearToNeighborLines()
	{
		for( int i = 0; i < toNeighborLines.Length; i++ )
		{
			Destroy( toNeighborLines[ i ] );
		}
	}

	private void SpawnDragLine( Vector2 targetPos )
	{
		Color lineColor = DotColor.GetColor( draggedDots[ 0 ].ColorValue );
		dragLines[ currDragLength ] = SpawnLine( targetPos, lineColor );
	}

	private GameObject SpawnLine( Vector3 end, Color lineColor )
	{
		GameObject line = (GameObject)Instantiate( boxPrefab );
		line.transform.parent = transform;
		dragLines[ currDragLength ] = line;
		line.transform.position = draggedDots[ currDragLength ].transform.position;

		line.GetComponent<SpriteRenderer>().color = lineColor;

		Vector3 linePos = line.transform.position;
		Vector3 pos2 = end - linePos;

		float angle = Mathf.Atan2( pos2.y, pos2.x ) * Mathf.Rad2Deg;

		line.transform.rotation = Quaternion.Euler( new Vector3( 0, 0, angle ) );
		float length = Mathf.Min( 1.0f, Vector2.Distance( linePos, end ) ) * 4.0f;
		line.transform.localScale = new Vector3( length, 1.0f, 1.0f );

		return line;
	}

	private void ClearDragLines()
	{
		for( int i = 0; i < dragLines.Length; i++ )
		{
			Destroy( dragLines[ i ] );
		}
	}
}
