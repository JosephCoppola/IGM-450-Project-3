using UnityEngine;
using UnityEngine.EventSystems;

public class DotScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public LayerMask dragLayerMask;

	public SpriteRenderer mySpriteRenderer;
	public SpriteRenderer borderSpriteRenderer;
	public TextMesh numberText;

	private DotColor.ColorValue colorValue;
	private int dragLength;

	private bool dragging;

	public void OnPointerClick( PointerEventData e )
	{
		if( IsSecondaryColor( colorValue ) )
		{
			MatchMaster.Instance.CheckMatch( this );
		}
	}

	public void OnBeginDrag( PointerEventData e )
	{
		if( colorValue < DotColor.ColorValue.PRIMARY_COLORS )
		{
			DragManager.Instance.StartDrag( this );
			dragging = true;
			DragManager.Instance.DragMove( Camera.main.ScreenToWorldPoint( e.position ) );
		}
	}

	public void OnDrag( PointerEventData e )
	{
		if( !dragging )
		{
			// Early return
			return;
		}
		Vector2 pos = Camera.main.ScreenToWorldPoint( e.position );
		DragManager.Instance.DragMove( pos );
		RaycastHit2D hit = Physics2D.Raycast( pos, Vector2.zero, 0.0f, dragLayerMask );

		if( hit.collider != null )
		{
			GameObject go = hit.collider.gameObject;
			if( go != gameObject )
			{
				DragManager.Instance.DragOverDot( go.GetComponent<DotScript>() );
			}
		}
	}

	public void OnEndDrag( PointerEventData e )
	{
		if( !dragging )
		{
			// Early return
			return;
		}

		DragManager.Instance.EndDrag();
	}

	public int DragLength
	{
		get { return dragLength; }
		set { dragLength = value; }
	}

	public DotColor.ColorValue ColorValue
	{
		get { return colorValue; }
	}

	public void SetColor( DotColor.ColorValue color )
	{
		borderSpriteRenderer.enabled = false;
		colorValue = color;
		Color newColor = DotColor.GetColor( color );
		mySpriteRenderer.color = newColor;

		if( color >= DotColor.ColorValue.PRIMARY_COLORS )
		{
			SetDragLength( 0 );
		}
	}

	public void SetDragLength( int num )
	{
		dragLength = num;
		numberText.text = (num > 0 ? "" + num : "");
	}

	public void MarkToBlend( DotColor.ColorValue incomingColor )
	{
		DotColor.ColorValue blend = DotColor.GetBlend( incomingColor, colorValue );

		Color newColor = DotColor.GetColor( blend );
		mySpriteRenderer.color = DotColor.GetColor( DotColor.ColorValue.WHITE );
		borderSpriteRenderer.enabled = true;
		borderSpriteRenderer.color = newColor;

		if( IsSecondaryColor( blend ) )
		{
			numberText.text = "";
		}
		else if( colorValue == incomingColor )
		{
			numberText.text = "" + ( dragLength + 1 );
		}
		else if( colorValue == DotColor.ColorValue.WHITE )
		{
			numberText.text = "1";
		}
	}

	private bool IsSecondaryColor( DotColor.ColorValue col )
	{
		return col >= DotColor.ColorValue.PRIMARY_COLORS && col < DotColor.ColorValue.SECONDARY_COLORS;
	}
}
