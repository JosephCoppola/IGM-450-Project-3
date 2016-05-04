using UnityEngine;
using System.Collections.Generic;

public class DragManager : MonoBehaviour
{
	public LayerMask neighborSearchLayerMask;
	public GameObject boxPrefab;

	private DotScript[] draggedDots;
	private List<DotScript> draggableNeighbors;
	private GameObject[] dragLines;

	private Vector2 pointerPos;

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
		draggableNeighbors = new List<DotScript>();
	}

	void Update()
	{
		if( currDragLength >= maxDragLength || !dragging )
		{
			// Early return
			return;
		}

		DrawDragLine( pointerPos );
	}

	public void StartDrag( DotScript startingDot )
	{
		dragging = true;

		currDragLength = 0;
		maxDragLength = startingDot.DragLength;
		dragLines = new GameObject[ maxDragLength + 1 ];
		draggedDots = new DotScript[ maxDragLength + 1 ]; // +1 to also hold start
		draggedDots[ 0 ] = startingDot;

		GetDraggableNeighbors();
		SpawnDragLine();
	}

	public void DragMove( Vector2 pointer )
	{
		pointerPos = pointer;
	}

	public void DragOverDot( DotScript dotToAdd )
	{
		if( currDragLength >= maxDragLength )
		{
			// Early return
			return;
		}

		if( draggableNeighbors.Contains( dotToAdd ) )
		{
			DrawDragLine( dotToAdd.transform.position );
			currDragLength++;
			draggedDots[ currDragLength ] = dotToAdd;
			dotToAdd.MarkToBlend( draggedDots[ 0 ].ColorValue );
			draggedDots[ 0 ].SetDragLength( draggedDots[ 0 ].DragLength - 1 );
			GetDraggableNeighbors();

			if( currDragLength < maxDragLength )
			{
				SpawnDragLine();
			}
		}
	}

	public void EndDrag()
	{
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
			startDot.SetColor( DotColor.ColorValue.WHITE );
		}

		ClearDragLines();

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

			if( IsDraggableNeighbor( dot ) )
			{
				draggableNeighbors.Add( dot );
			}
		}
	}

	private bool IsDraggableNeighbor( DotScript dot )
	{
		return ( dot.ColorValue < DotColor.ColorValue.PRIMARY_COLORS || dot.ColorValue == DotColor.ColorValue.WHITE );
	}

	private void RemoveLastDot()
	{
		draggedDots[ currDragLength ] = null;
		currDragLength--;
	}

	private void SpawnDragLine()
	{
		GameObject dragLine = (GameObject)Instantiate( boxPrefab );
		dragLine.transform.parent = transform;
		dragLines[ currDragLength ] = dragLine;
		dragLine.transform.position = draggedDots[ currDragLength ].transform.position;

		Color lineColor = DotColor.GetColor( draggedDots[ 0 ].ColorValue );
		dragLine.GetComponent<SpriteRenderer>().color = lineColor;
	}

	private void DrawDragLine( Vector2 targetPos )
	{
		if( dragLines[ currDragLength ] != null )
		{
			GameObject line = dragLines[ currDragLength ];

			Vector2 linePos = line.transform.position;
			Vector2 pos2 = targetPos - linePos;

			float angle = Mathf.Atan2( pos2.y, pos2.x ) * Mathf.Rad2Deg;

			line.transform.rotation = Quaternion.Euler( new Vector3( 0, 0, angle ) );
			float length = Mathf.Min( 1.0f, Vector2.Distance( linePos, targetPos ) ) * 4.0f;
			line.transform.localScale = new Vector3( length, 1.0f, 1.0f );
		}
	}

	private void ClearDragLines()
	{
		for( int i = 0; i < dragLines.Length; i++ )
		{
			Destroy( dragLines[ i ] );
		}
	}
}
